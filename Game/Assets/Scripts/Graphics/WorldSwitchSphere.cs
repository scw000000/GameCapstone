using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSwitchSphere : MonoBehaviour {
    public string _shaderFilePath = "Hidden/WorldSwitch";
    private Material _material;
    public RenderTexture _theOtherWorldTexture;
    RenderTexture _theOtherWorldDepthTexture;
   // public float _sphereRadius = 0f;
    public float _sphereWidth = 15f;
    public Color _barColor { get; set; }
    public float _barAlpha;
    public float _gradientColorShift;
    public float _gradientColorUVShift;
    public bool _isUpdating = true;
    public AnimationCurve _animationCurve { get; set; }
    public AnimationCurve _fovCurve { get; set; }
    private float _maxSphereRadius;
    private float _switchTime = 4f;
    public float _vignetteTime;
    private float _currentTime = 0f;
    public Camera _theOtherCamera { get; set; }
    private Camera _myCamera;
    public float _minFOV;
    public float _maxFOV;
    public void SetTheOtherWorldTexture( RenderTexture theOtherWorldTexture) {
        _theOtherWorldTexture = theOtherWorldTexture;
       //       
        }
    public void SetTheOtherWorldDepthTexture( RenderTexture theOtherWorldDepthTex ) {
        _theOtherWorldDepthTexture = theOtherWorldDepthTex;
    }
    public void Init() {
        _material = new Material(Shader.Find(_shaderFilePath));
        _material.SetTexture("_TheOtherWorldTex", _theOtherWorldTexture);
        _material.SetTexture("_TheOtherWorldDepthTex", _theOtherWorldDepthTexture);
        _myCamera = gameObject.GetComponent<Camera>();
        _maxSphereRadius = _myCamera.farClipPlane;
        _material.SetFloat("_SphereRadius", 0);
        _material.SetFloat("_SphereWidth", _sphereWidth);
        _material.SetColor("_BarColor", _barColor);
        _material.SetFloat("_BarAlpha", _barAlpha);
        _material.SetFloat("_GradientColorShift", _gradientColorShift);
        _material.SetFloat("_GradientColorUVShift", _gradientColorUVShift);
    }

    public void Reset() {
        _currentTime = 0f;
        _isUpdating = true;
        SetVignette(true);
        // Invoke("DisableSelf", _switchTime);
    }

    public void DisableSelf() {
        _currentTime = 0f;
        enabled = false;
    }

    public void SetUpdating(bool enabled) {
        _isUpdating = enabled;
    }

    private void SetVignette(bool enabled) {
        var ppComp = gameObject.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();
        ppComp.profile.vignette.enabled = enabled;
    }

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
        if (_isUpdating) {
            _currentTime += Time.deltaTime / _switchTime;
            if (_currentTime * _switchTime >= _vignetteTime) {
                SetVignette(false);
            }
            if (_currentTime >= 1f) {
                DisableSelf();
            }
        }
        // temperal put in here for debugging
        _material.SetFloat("_SphereRadius", (Mathf.Lerp(0, _maxSphereRadius, _animationCurve.Evaluate(_currentTime))));
        _material.SetFloat("_SphereWidth", _sphereWidth);
        _material.SetColor("_BarColor", _barColor);
        _material.SetFloat("_BarAlpha", _barAlpha);
        _material.SetFloat("_GradientColorShift", _gradientColorShift);
        _material.SetFloat("_GradientColorUVShift", _gradientColorUVShift);

        _myCamera.fieldOfView = Mathf.Lerp(_minFOV, _maxFOV, _fovCurve.Evaluate(_currentTime));
        _theOtherCamera.fieldOfView = _myCamera.fieldOfView;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        var inverseView = gameObject.GetComponent<Camera>().worldToCameraMatrix.inverse;

        _material.SetMatrix("_InverseViewMat", inverseView);
        Graphics.Blit(source, destination, _material);
        // var ppComponent = gameObject.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();
       // ppComponent.OnRenderImage(source, destination);
        //OnRenderImage
    }
}
