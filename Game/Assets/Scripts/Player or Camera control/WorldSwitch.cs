using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSwitch : MonoBehaviour {
    private GameObject _cameraRoot;
    private GameObject _cameraSetInstance;
    private GameObject _holdingObject;
    private Camera _cameraA;
    private Camera _cameraB;
    private RenderTexture _renderTexture;
    private RenderTexture _depthTexture;
    private Camera _sceneCamera;
    // Use this for initialization
    void Start () {
     //   _renderTexture.width = Screen.width;
     //   _renderTexture.height = Screen.height;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.X)) {
            var collider = gameObject.GetComponent<CapsuleCollider>();
            var overlappers = Physics.OverlapCapsule(gameObject.transform.position, gameObject.transform.position, collider.radius);
            bool switchable = true;
            // Only alow switch when the player is not in overlapp with object in another world
            foreach (var overlap in overlappers) {
                if (overlap.gameObject.layer != gameObject.layer) {
                    switchable = false;
                }
            }
            if (switchable) {
                Debug.Log("Switch!");
                bool isCamASceneCam = _sceneCamera.GetInstanceID() == _cameraA.GetInstanceID();
                gameObject.layer = LayerMask.NameToLayer((isCamASceneCam? "WorldB" : "WorldA"));
                _holdingObject.layer = gameObject.layer;
                var backgroundCamera = isCamASceneCam ? _cameraB : _cameraA;
                _sceneCamera.SetTargetBuffers(_renderTexture.colorBuffer, _depthTexture.depthBuffer);
                backgroundCamera.targetTexture = null;
                _sceneCamera = backgroundCamera;

                var worldSwitchEffect = _sceneCamera.gameObject.AddComponent<WorldSwitchSphere>();
                worldSwitchEffect.SetTheOtherWorldTexture(_renderTexture);
                worldSwitchEffect.SetTheOtherWorldDepthTexture(_depthTexture);
                worldSwitchEffect.Init();
            }
            else{
                Debug.Log("Cannot switch");
            }
            
        }
    }

    public void SetUpCamera(GameObject cameraSetInstance) {
        _cameraSetInstance = cameraSetInstance;
        _cameraRoot = gameObject.transform.Find("CameraRoot").gameObject;
        _holdingObject = _cameraSetInstance.transform.Find("Holder").gameObject;
        _holdingObject.layer = LayerMask.NameToLayer("WorldA");
        _cameraA = _cameraSetInstance.transform.Find("CameraA").gameObject.GetComponent<Camera>();
        _sceneCamera = _cameraA;
        _cameraB = _cameraSetInstance.transform.Find("CameraB").gameObject.GetComponent<Camera>();
        _cameraA.targetTexture = null;
        _renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
        _renderTexture.Create();
        _depthTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Depth);
        _depthTexture.Create();
        _cameraB.SetTargetBuffers(_renderTexture.colorBuffer, _depthTexture.depthBuffer);

        
        var holder = _cameraSetInstance.transform.Find("Holder").gameObject;
        var holderMat = holder.GetComponent<Renderer>().material;
        holderMat.SetTexture("_MainTex", _renderTexture);
        
        
        // Tell camera set to follow the root
        _cameraSetInstance.SendMessage("SetupRoot", _cameraRoot);
    }
}
