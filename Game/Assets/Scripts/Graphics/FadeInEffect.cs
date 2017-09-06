using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInEffect : MonoBehaviour {
    public string _shaderFilePath;
    public UnityEngine.AnimationCurve _transitionCurve;
    public float _transitionTime = 10f;
    public float _currentTime = 0f;
    private Material _material;
    public bool _reverse = false;
    public bool _updating = false;
    public Texture _fadeInTexture;
    // Creates a private material used to the effect
    void Awake() {
        _material = new Material(Shader.Find(_shaderFilePath));
        
    }

    // Use this for initialization
    void Start () {
        _material.SetTexture("_FadePattern", _fadeInTexture);
    }
	
	// Update is called once per frame
	void Update () {
        if (_updating) {
            // Debug.Log(CurrentTime);
            _currentTime += (Time.deltaTime / _transitionTime);
        }
    }

    void EndTransition() {
        Debug.Log("Transition End");
        // Updating = false;
        // Destroy(this);
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        _material.SetFloat("_Threshold", _transitionCurve.Evaluate(_reverse?1f - _currentTime : _currentTime));
        Graphics.Blit(source, destination, _material);
    }
}
