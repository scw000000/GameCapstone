﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInEffect : MonoBehaviour {
    public string ShaderFilePath;
    public UnityEngine.AnimationCurve TransitionCurve;
    public float TransitionTime = 10f;
    public float CurrentTime = 0f;
    private Material Material;
    public bool Reverse = false;
    public bool Updating = false;
    public Texture FadeInTexture;
    RenderTexture IntermediateRT;
    // Creates a private material used to the effect
    void Awake() {
        Material = new Material(Shader.Find(ShaderFilePath));
        
    }

    // Use this for initialization
    void Start () {
        IntermediateRT = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.Default);
        Material.SetTexture("_FadePattern", FadeInTexture);
    }
	
	// Update is called once per frame
	void Update () {
        if (Updating) {
            // Debug.Log(CurrentTime);
            CurrentTime += (Time.deltaTime / TransitionTime);
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
        Material.SetFloat("_Threshold", TransitionCurve.Evaluate(Reverse?1f - CurrentTime : CurrentTime));
        Graphics.Blit(source, destination, Material);
    }
}
