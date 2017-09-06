using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSwitchSphere : MonoBehaviour {
    public string _shaderFilePath = "Hidden/WorldSwitch";
    private Material _material;
    public Texture2D _theOtherWorldTexture {
        get { return _theOtherWorldTexture; }
        set { _theOtherWorldTexture = value;
              _material.SetTexture("_TheOtherWolrdTex", _theOtherWorldTexture); }
        }
    // Use this for initialization
    void Start () {
        _material = new Material(Shader.Find(_shaderFilePath));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        _material.SetVector("_SphereCenter", gameObject.transform.position);
        // Graphics.Blit(source, destination, _material);
    }
}
