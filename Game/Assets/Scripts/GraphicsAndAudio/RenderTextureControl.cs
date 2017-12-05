using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureControl : MonoBehaviour {
    private Material[] _materials;
    private GameObject _playerGO;
	// Use this for initialization
	void Start () {
        if (gameObject.GetComponent<MeshRenderer>() != null) {
            _materials = gameObject.GetComponent<MeshRenderer>().materials;
        }
        else if (gameObject.GetComponent<SkinnedMeshRenderer>() != null) {
            _materials = gameObject.GetComponent<SkinnedMeshRenderer>().materials;
        }
        _playerGO = GameObject.FindGameObjectWithTag("Player");


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnWillRenderObject() {
        // Debug.Log(Camera.current.name);
        // This is background camera, so render it no matter it's inside sphere or not
        float scalar = 0f;
        if (((Camera.current.name.Equals("CameraA") || Camera.current.name.Equals("CutsceneCameraA")) && (gameObject.layer == LayerMask.NameToLayer("WorldA") || gameObject.layer == LayerMask.NameToLayer("WorldAInPortal")))
                || ((Camera.current.name.Equals("CameraB") || Camera.current.name.Equals("CutsceneCameraB")) && (gameObject.layer == LayerMask.NameToLayer("WorldB") || gameObject.layer == LayerMask.NameToLayer("WorldBInPortal"))))
        {
            scalar = 1f;
            // material.SetFloat("_OutOrInScalar", 1f);
        }
        else if(((Camera.current.name.Equals("CameraA") || Camera.current.name.Equals("CutsceneCameraA")) && (gameObject.layer == LayerMask.NameToLayer("WorldB") || gameObject.layer == LayerMask.NameToLayer("WorldBInPortal")))
                || ((Camera.current.name.Equals("CameraB") || Camera.current.name.Equals("CutsceneCameraB")) && (gameObject.layer == LayerMask.NameToLayer("WorldA") || gameObject.layer == LayerMask.NameToLayer("WorldAInPortal"))))
        {
            scalar = -1f;
            // material.SetFloat("_OutOrInScalar", -1f);
        }

        foreach (var material in _materials) {
            // it default
            material.SetFloat("_OutOrInScalar", scalar);
        }
        
    }
}
