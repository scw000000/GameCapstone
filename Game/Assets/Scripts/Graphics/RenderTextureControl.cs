using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureControl : MonoBehaviour {
    private Material _material;
	// Use this for initialization
	void Start () {
        _material = gameObject.GetComponent<MeshRenderer>().material;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnWillRenderObject() {
        // Debug.Log(Camera.current.name);
        // This is background camera, so render it no matter it's inside sphere or not
        if (Camera.current.renderingPath == RenderingPath.Forward || gameObject.layer == LayerMask.NameToLayer("Default")){
            _material.SetFloat("_OutOrInScalar", 0f);
        }
        else {
            if ( ( Camera.current.name.Equals("CameraA") && gameObject.layer == LayerMask.NameToLayer("WorldA"))
                || (Camera.current.name.Equals("CameraB") && gameObject.layer == LayerMask.NameToLayer("WorldB"))){
                _material.SetFloat("_OutOrInScalar", 1f);
            }
            else {
                _material.SetFloat("_OutOrInScalar", -1f);
            }
            
        }
    }
}
