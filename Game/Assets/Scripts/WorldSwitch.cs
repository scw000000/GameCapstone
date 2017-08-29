using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSwitch : MonoBehaviour {
    public GameObject CameraSetPrefab;

    private GameObject CameraRoot;
    private GameObject CameraSet;
    private GameObject HoldingObject;
    private Camera CameraA;
    private Camera CameraB;
    public RenderTexture RenderTexture;
	// Use this for initialization
	void Start () {
        if (CameraSetPrefab == null) {
            Debug.LogError("CameraSetPrefab is not defined");
            return;
        }
        CameraRoot = gameObject.transform.Find("CameraRoot").gameObject;
        if (CameraRoot == null) {
            Debug.LogError("Cannot find camera root");
        }
        
        CameraSet = Instantiate(CameraSetPrefab, CameraRoot.transform.position, CameraRoot.transform.rotation);
        HoldingObject = CameraSet.transform.Find("Holder").gameObject;
        HoldingObject.layer = LayerMask.NameToLayer("WorldA");
        // cameraSetInstance.transform.parent = CameraRoot.transform;
        CameraA = CameraSet.transform.Find("CameraA").gameObject.GetComponent<Camera>();
        CameraB = CameraSet.transform.Find("CameraB").gameObject.GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        // update postion here to get smooth camera control
        CameraSet.transform.position = CameraRoot.transform.position;
        CameraSet.transform.rotation = Quaternion.Slerp(CameraSet.transform.rotation, CameraRoot.transform.rotation, 10f * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.X)) {
            var collider = gameObject.GetComponent<CapsuleCollider>();
            var overlappers = Physics.OverlapCapsule(gameObject.transform.position, gameObject.transform.position, collider.radius);
            bool switchable = true;
            // Only when the player is not
            foreach (var overlap in overlappers) {
                if (overlap.gameObject.layer != gameObject.layer) {
                    switchable = false;
                }
            }
            if (switchable) {
                Debug.Log("Switch!");
                gameObject.layer = LayerMask.NameToLayer((CameraA.targetTexture == null ? "WorldB" : "WorldA"));
                HoldingObject.layer = gameObject.layer;
                var sceneCamera = CameraA.targetTexture == null ? CameraA : CameraB;
                var backgroundCamera = sceneCamera.GetInstanceID() == CameraA.GetInstanceID() ? CameraB : CameraA;
                sceneCamera.targetTexture = RenderTexture;
                backgroundCamera.targetTexture = null;
            }
            else{
                Debug.Log("Cannot switch");
            }
            
        }
	}
}
