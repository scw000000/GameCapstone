﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSwitch : MonoBehaviour {
    public AnimationCurve _speedCurve;
    public AnimationCurve _fovCurve;
    public Color _barColor;
    public float _barAlpha = 0.3f;
    public float _gradientColorShift = 1f;
    public float _gradientColorUVShift = 1f;
    public float _maxFOV;
    public float _minFOV;
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
                var sceneSwitchComp = _sceneCamera.gameObject.GetComponent<WorldSwitchSphere>();
                sceneSwitchComp.enabled = false;
                var backSwitchComp = backgroundCamera.gameObject.GetComponent<WorldSwitchSphere>();
                backSwitchComp.Reset();
                backSwitchComp.enabled = true;
                _sceneCamera = backgroundCamera;

            }
            else{
                Debug.Log("Cannot switch");
            }
            
        }

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

        if (Input.GetKeyDown(KeyCode.F)) {
            foreach (var cam in cameras)
            {
                var worldSwitchEffect = cam.gameObject.GetComponent<WorldSwitchSphere>();
                worldSwitchEffect.SetUpdating(!worldSwitchEffect._isUpdating);
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

        Camera[] cameras = new Camera[] { _cameraA, _cameraB };
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
            worldSwitchEffect.Init();
        }

        var holder = _cameraSetInstance.transform.Find("Holder").gameObject;
        var holderMat = holder.GetComponent<Renderer>().material;
        holderMat.SetTexture("_MainTex", _renderTexture);
        
        // Tell camera set to follow the root
        _cameraSetInstance.SendMessage("SetupRoot", _cameraRoot);
    }
}
