using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSwitch : MonoBehaviour {
    public AnimationCurve _speedCurve;
    public AnimationCurve _fovCurve;
    public UnityEngine.PostProcessing.PostProcessingProfile _activePPProfile;
    public UnityEngine.PostProcessing.PostProcessingProfile _backgroundPPProfile;
    public Color _barColor;
    public float _barAlpha = 0.3f;
    public float _gradientColorShift = 1f;
    public float _gradientColorUVShift = 1f;
    public float _maxFOV;
    public float _minFOV;
    public float _vignetteTime = 1f;
    public bool _insidePortal = false;
    public Texture2D _gradientTexutre;
    private GameObject _cameraRoot;
    private GameObject _cameraSetInstance;
    private GameObject _holdingObject;
    private Camera _cameraA;
    private Camera _cameraB;
    private Camera _outlineCamera;
    private RenderTexture _renderTexture;
    private RenderTexture _depthTexture;
    private RenderTexture _outlineCaptureRenderTexture;
    private RenderTexture _outlineDepthTexture;
    private Camera _sceneCamera;
    private int _worldALayer;
    private int _worldBLayer;
    // Use this for initialization
    void Start () {
        //   _renderTexture.width = Screen.width;
        //   _renderTexture.height = Screen.height;
        _worldALayer = LayerMask.NameToLayer("WorldA");
        _worldBLayer = LayerMask.NameToLayer("WorldB");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("SwitchWorld")) {
            PerformSwitch(true);            
        }
        // Temporal for debugging
        Camera[] cameras = new Camera[] { _cameraA, _cameraB };
        for(int i = 0; i < cameras.Length; ++i)
        {
            var cam = cameras[i];
            var worldSwitchEffect = cam.gameObject.GetComponent<WorldSwitchSphere>();
            worldSwitchEffect._barColor = _barColor;
            worldSwitchEffect._barAlpha = _barAlpha;
            worldSwitchEffect._gradientColorShift = _gradientColorShift;
            worldSwitchEffect._gradientColorUVShift = _gradientColorUVShift;
            worldSwitchEffect._fovCurve = _fovCurve;
            worldSwitchEffect._maxFOV = _maxFOV;
            worldSwitchEffect._minFOV = _minFOV;
            worldSwitchEffect._theOtherCamera = cameras[1 - i];
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            foreach (var cam in cameras)
            {
                var worldSwitchEffect = cam.gameObject.GetComponent<WorldSwitchSphere>();
                worldSwitchEffect.SetUpdating(!worldSwitchEffect._isUpdating);
            }
        }
    }

    public void SetPortalStatus(bool isInside) {
        if (_insidePortal != isInside){
            SwitchCollisionVolume();
        }
        _insidePortal = isInside;
    }

    public void SwitchCollisionVolume() {
        gameObject.layer = gameObject.layer == _worldALayer?_worldBLayer:_worldALayer;
    }

    public void PerformSwitch(bool enableAnimation) {
        var collider = gameObject.GetComponent<CapsuleCollider>();
        var overlappers = Physics.OverlapCapsule(gameObject.transform.position, gameObject.transform.position, collider.radius);
        bool switchable = true;
        // Only alow switch when the player is not in overlapp with object in another world
        foreach (var overlap in overlappers)
        {
            // overlap.gameObject.layer != LayerMask.NameToLayer("Default") &&
            if ((overlap.gameObject.layer == _worldALayer || overlap.gameObject.layer == _worldBLayer)
                && overlap.gameObject.layer != gameObject.layer
                && overlap.isTrigger == false )
            {
                switchable = false;
            }
        }
        if (switchable)
        {
            Debug.Log("Switch!");
            bool isCamASceneCam = _sceneCamera.GetInstanceID() == _cameraA.GetInstanceID();
            SwitchCollisionVolume();
            _holdingObject.layer = _holdingObject.layer == _worldALayer? _worldBLayer: _worldALayer;
            var backgroundCamera = isCamASceneCam ? _cameraB : _cameraA;
            // We don't want both of the camera be
            // int baselinecullMask = -1 ^ (1 << LayerMask.NameToLayer("Outline"));
            // backgroundCamera.cullingMask = baselinecullMask ^ (1 << LayerMask.NameToLayer((isCamASceneCam ? "WorldA" : "WorldB")));
            // _sceneCamera.cullingMask = baselinecullMask ^ (1 << LayerMask.NameToLayer((isCamASceneCam ? "WorldB" : "WorldA")));
            // _sceneCamera.cullingMask = -1;
            _sceneCamera.renderingPath = RenderingPath.Forward;
            backgroundCamera.renderingPath = RenderingPath.DeferredShading;
            _sceneCamera.SetTargetBuffers(_renderTexture.colorBuffer, _depthTexture.depthBuffer);
            _sceneCamera.targetTexture = _renderTexture;
            backgroundCamera.targetTexture = null;

            var ppComp = _sceneCamera.gameObject.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();
            // ppComp.profile = Instantiate(_backgroundPPProfile);
            ppComp.profile = _backgroundPPProfile;
            // ppComp.enabled = false;
            // ppComp.enabled = true;
            // ppComp.profile.grain.enabled = true;

            ppComp = backgroundCamera.gameObject.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();
            // ppComp.profile = Instantiate(_activePPProfile);
            ppComp.profile = _activePPProfile;
            // ppComp.enabled = false;
            // ppComp.enabled = true;
            Camera tempCam = _sceneCamera;
            _sceneCamera = backgroundCamera;
            backgroundCamera = tempCam;
            if (enableAnimation) {
                StartSwitchAnimation(_sceneCamera, backgroundCamera);
            }
            
        }
        else
        {
            Debug.Log("Cannot switch");
        }
    }

    private void StartSwitchAnimation(Camera newSceneCam, Camera newBackCam) {
        var newBackSwitchComp = newBackCam.gameObject.GetComponent<WorldSwitchSphere>();
        newBackSwitchComp.enabled = false;
        var newSceneSwitchComp = newSceneCam.gameObject.GetComponent<WorldSwitchSphere>();
        newSceneSwitchComp.Reset();
        newSceneSwitchComp.enabled = true;
    }

    public void SetUpCamera(GameObject cameraSetInstance) {
        _cameraSetInstance = cameraSetInstance;
        _cameraRoot = gameObject.transform.Find("CameraRoot").gameObject;
        _holdingObject = _cameraSetInstance.transform.Find("Holder").gameObject;
        _holdingObject.layer = LayerMask.NameToLayer("WorldA");
        _cameraA = _cameraSetInstance.transform.Find("CameraA").gameObject.GetComponent<Camera>();
        _cameraA.renderingPath = RenderingPath.DeferredShading;
        // _cameraA.cullingMask = -1 ^ (1 << LayerMask.NameToLayer("WorldB"));
        _sceneCamera = _cameraA;
        _cameraA.targetTexture = null;
        _cameraB = _cameraSetInstance.transform.Find("CameraB").gameObject.GetComponent<Camera>();
        _cameraB.renderingPath = RenderingPath.Forward;
        // _cameraB.cullingMask = -1 ^ ( 1 << LayerMask.NameToLayer("WorldA") ); 
        _renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
        _renderTexture.Create();
        _depthTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Depth);
        _depthTexture.Create();
        _cameraB.SetTargetBuffers(_renderTexture.colorBuffer, _depthTexture.depthBuffer);
        // Why will this work????
        _cameraB.targetTexture = _renderTexture;

        _outlineCamera = _cameraSetInstance.transform.Find("OutlineCapture").gameObject.GetComponent<Camera>();
        _outlineCaptureRenderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
        _outlineCaptureRenderTexture.Create();
        _outlineDepthTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Depth);
        _outlineDepthTexture.Create();
        _outlineCamera.SetTargetBuffers(_outlineCaptureRenderTexture.colorBuffer, _outlineDepthTexture.depthBuffer);
        _outlineCamera.targetTexture = _outlineCaptureRenderTexture;
        // _outlineCamera.
        // _cameraSetInstance.transform.Find("OutlineCapture").GetComponent<Skybox>().enabled = false;

        Camera[] cameras = new Camera[] { _cameraB, _cameraA };
        foreach (var cam in cameras)
        {
            var worldSwitchEffect = cam.gameObject.AddComponent<WorldSwitchSphere>();
            worldSwitchEffect.SetTheOtherWorldTexture(_renderTexture);
            worldSwitchEffect.SetTheOtherWorldDepthTexture(_depthTexture);
            worldSwitchEffect._animationCurve = _speedCurve;
            worldSwitchEffect._fovCurve = _fovCurve;
            worldSwitchEffect._maxFOV = _maxFOV;
            worldSwitchEffect._minFOV = _minFOV;
            worldSwitchEffect._barColor = _barColor;
            worldSwitchEffect._barAlpha = _barAlpha;
            worldSwitchEffect._gradientColorShift = _gradientColorShift;
            worldSwitchEffect._gradientColorUVShift = _gradientColorUVShift;
            worldSwitchEffect.enabled = false;
            worldSwitchEffect._gradientTexutre = _gradientTexutre;
            worldSwitchEffect._vignetteTime = _vignetteTime;
            worldSwitchEffect.Init();
            worldSwitchEffect._outlineCamera = _outlineCamera;
            var outlineDetctComp = cam.gameObject.AddComponent<OutlineDetection>();
            outlineDetctComp._camera = cam;
            var combineComp = cam.gameObject.AddComponent<OutlineCombine>();
            combineComp.SetUpRT(_outlineCaptureRenderTexture, _cameraA, _cameraB);
            
        }
        var ppComp = _cameraA.gameObject.AddComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();
        ppComp.profile = _activePPProfile;
        ppComp = _cameraB.gameObject.AddComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();
        ppComp.profile = _backgroundPPProfile;

        var holder = _cameraSetInstance.transform.Find("Holder").gameObject;
        var holderMat = holder.GetComponent<Renderer>().material;
        holderMat.SetTexture("_MainTex", _renderTexture);
        
        // Tell camera set to follow the root
        _cameraSetInstance.SendMessage("SetupRoot", _cameraRoot);
    }
}
