using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSwitchSphere : MonoBehaviour {
    public string _shaderFilePath = "Hidden/WorldSwitch";
    private Material _material;
    public RenderTexture _theOtherWorldTexture;
    RenderTexture _theOtherWorldDepthTexture;
    public float _sphereRadius = 0f;
    public float _sphereWidth = 15f;
    public Color _barColor = new Color(0, 1, 0);
    public Color _midColor = new Color(0, 0, 1);
    public float _edgeSharpness = 3f;
    private float _maxSphereRadius;
    private float _switchTime = 20f;
    private float _currentTime = 0f;
    public void SetTheOtherWorldTexture( RenderTexture theOtherWorldTexture) {
        _theOtherWorldTexture = theOtherWorldTexture;
       //       
        }
    public void SetTheOtherWorldDepthTexture( RenderTexture theOtherWorldDepthTex ) {
        _theOtherWorldDepthTexture = theOtherWorldDepthTex;
            //_theOtherWorldDepthTex = value;
        //    
    }
    public void Init() {
        _material = new Material(Shader.Find(_shaderFilePath));
        _material.SetTexture("_TheOtherWorldTex", _theOtherWorldTexture);
        _material.SetTexture("_TheOtherWorldDepthTex", _theOtherWorldDepthTexture);
        var sceneCamera = gameObject.GetComponent<Camera>();
        _maxSphereRadius = sceneCamera.farClipPlane;
        var inverseView = sceneCamera.worldToCameraMatrix.inverse;
        _material.SetMatrix("_InverseViewMat", inverseView);
        var test = _material.GetMatrix("_InverseViewMat");
        _material.SetColor("_BarColor", _barColor);
        _material.SetColor("_MidColor", _midColor);
        _material.SetFloat("_EdgeSharpness", _edgeSharpness);

        // Shader.SetGlobalMatrix("_InverseViewMat", inverseView);
    }

    // Use this for initialization
    void Start () {
        Destroy(this, _switchTime);
    }
	
	// Update is called once per frame
	void Update () {
        _currentTime += Time.deltaTime;
        _material.SetFloat("_SphereRadius", Mathf.Lerp(0, _maxSphereRadius, _currentTime / _switchTime));
        _material.SetFloat("_SphereWidth", _sphereWidth);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        var inverseView = gameObject.GetComponent<Camera>().worldToCameraMatrix.inverse;
        _material.SetMatrix("_InverseViewMat", inverseView);
        // _material.SetVector("_SphereCenter", gameObject.transform.position);
        
        Graphics.Blit(source, destination, _material);
    }
}
