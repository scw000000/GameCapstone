using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineCombine : MonoBehaviour {
    private RenderTexture _outlineCombineRT;
    private Material _combineMaterial;
	// Use this for initialization
	void Start () {
        

	}
	
	// Update is called once per frame
	void Update () {
    }

    public void SetUpRT(RenderTexture rt, Camera camA, Camera camB) {
        _outlineCombineRT = rt;
        _combineMaterial = new Material(Shader.Find("Hidden/OutlineCombine"));
        _combineMaterial.SetTexture("_CombineTex", _outlineCombineRT);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // _material.SetFloat("_Threshold", _transitionCurve.Evaluate(_reverse ? 1f - _currentTime : _currentTime));
        // Graphics.Blit(source, destination, _material);
        // Copy to blur temp 1
        if (_combineMaterial != null){
            Graphics.Blit(source, destination, _combineMaterial);
        }
        
    }
}
