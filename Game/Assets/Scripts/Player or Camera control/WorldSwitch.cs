using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSwitch : MonoBehaviour {
    private GameObject _cameraRoot;
    private GameObject _cameraSetInstance;
    private GameObject _holdingObject;
    private Camera _cameraA;
    private Camera _cameraB;
    public RenderTexture _renderTexture;
	// Use this for initialization
	void Start () {
        _renderTexture.width = Screen.width;
        _renderTexture.height = Screen.height;
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
                gameObject.layer = LayerMask.NameToLayer((_cameraA.targetTexture == null ? "WorldB" : "WorldA"));
                _holdingObject.layer = gameObject.layer;
                var sceneCamera = _cameraA.targetTexture == null ? _cameraA : _cameraB;
                var backgroundCamera = sceneCamera.GetInstanceID() == _cameraA.GetInstanceID() ? _cameraB : _cameraA;
                sceneCamera.targetTexture = _renderTexture;
                backgroundCamera.targetTexture = null;
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
        // var worldSwitchEffect = cameraSetInstance.transform.Find("CameraA").gameObject.AddComponent<WorldSwitchSphere>();
        // worldSwitchEffect._theOtherWorldTexture = _theOtherWorldTexture;
        _cameraB = _cameraSetInstance.transform.Find("CameraB").gameObject.GetComponent<Camera>();
        _cameraA.targetTexture = null;
        _cameraB.targetTexture = _renderTexture;
        var holder = _cameraSetInstance.transform.Find("Holder").gameObject;
        var holderMat = holder.GetComponent<Renderer>().material;
        holderMat.SetTexture("_MainTex", _renderTexture);
        // Tell camera set to follow the root
        _cameraSetInstance.SendMessage("SetupRoot", _cameraRoot);
    }
}
