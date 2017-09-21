using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineBlur : MonoBehaviour {
    // public UnityEngine.AnimationCurve _transitionCurve;
    // public float _transitionTime = 10f;
    // public float _currentTime = 0f;
    private Material _blurMaterial;
    private Material _substractMaterial;
    public RenderTexture _blurTemp1;
    public RenderTexture _blurTemp2;

    // public bool _reverse = false;
    // public bool _updating = false;
    // public Texture _fadeInTexture;
    // Creates a private material used to the effect
    void Awake()
    {
        _blurMaterial = new Material(Shader.Find("Hidden/OutlineBlur"));
        _substractMaterial = new Material(Shader.Find("Hidden/OutlineSubstract"));

        _blurTemp1 = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
        _blurTemp1.Create();

        _blurTemp2 = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
        _blurTemp2.Create();
    }

    // Use this for initialization
    void Start()
    {

        _blurMaterial.SetVector("_BlurSize", new Vector2(_blurTemp1.texelSize.x * 5f, _blurTemp1.texelSize.y * 5f));
        //_material.SetTexture("_FadePattern", _fadeInTexture);
    }

    // Update is called once per frame
    void Update()
    {
        //if (_updating)
        //{
        //    // Debug.Log(CurrentTime);
        //    _currentTime += (Time.deltaTime / _transitionTime);
        //}
    }

    void EndTransition()
    {
        // Debug.Log("Transition End");
        // Updating = false;
        // Destroy(this);
    }

    // Postprocess the image
    // source should be blurred texutre
    // destination should be cutted texture
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // _material.SetFloat("_Threshold", _transitionCurve.Evaluate(_reverse ? 1f - _currentTime : _currentTime));
        // Graphics.Blit(source, destination, _material);
        // Copy to blur temp 1
        Graphics.Blit(source, _blurTemp1);
        
        // Start blurring
        for (int i = 0; i < 4; i++)
        {
            _blurMaterial.SetTexture("_MainTex", _blurTemp1);
            Graphics.Blit(_blurTemp1, _blurTemp2, _blurMaterial, 0);
            _blurMaterial.SetTexture("_MainTex", _blurTemp2);
            Graphics.Blit(_blurTemp2, _blurTemp1, _blurMaterial, 1);
        }
       //  _blurMaterial.SetTexture("_MainTex", source);
        _substractMaterial.SetTexture("_BlurTex", _blurTemp1);
        Graphics.Blit(source, destination, _substractMaterial);
    }
}
