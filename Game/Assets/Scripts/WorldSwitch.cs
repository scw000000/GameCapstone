using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSwitch : MonoBehaviour {
    public Camera m_CameraA;
    public Camera m_CameraB;
    public RenderTexture m_RenderTexture;
    public GameObject m_Avatar;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.X)) {
            var collider = m_Avatar.GetComponent<CapsuleCollider>();
            var overlappers = Physics.OverlapCapsule(m_Avatar.transform.position, m_Avatar.transform.position, collider.radius);
            bool switchable = true;
            foreach (var overlap in overlappers) {
                if (overlap.gameObject.layer != m_Avatar.layer) {
                    switchable = false;
                }
            }
            if (switchable) {
                Debug.Log("Switch!");
                gameObject.layer = LayerMask.NameToLayer((m_CameraA.targetTexture == null ? "WorldB" : "WorldA"));
                m_Avatar.layer = LayerMask.NameToLayer((m_CameraA.targetTexture == null ? "WorldB" : "WorldA"));
                var sceneCamera = m_CameraA.targetTexture == null ? m_CameraA : m_CameraB;
                var backgroundCamera = sceneCamera.GetInstanceID() == m_CameraA.GetInstanceID() ? m_CameraB : m_CameraA;
                sceneCamera.targetTexture = m_RenderTexture;
                backgroundCamera.targetTexture = null;
            }
            else{
                Debug.Log("Cannot switch");
            }
            
        }
	}
}
