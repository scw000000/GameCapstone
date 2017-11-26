﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundControl : MonoBehaviour {
    public AudioSource _worldAAudio;
    public AudioSource _worldBAudio;
    private Camera _cameraA;
    private AudioListener _cutsceneCameraA;
    // Use this for initialization
    void Start () {
        var audios = gameObject.GetComponents<AudioSource>();
        _worldAAudio = audios[0];
        _worldBAudio = audios[1];
        _cameraA = GameObject.Find("CameraA").GetComponent<Camera>();
        _cutsceneCameraA = GameObject.Find("CutsceneCameraA").GetComponent<AudioListener>();
        StartCoroutine("UpdateAudioSource");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator UpdateAudioSource()
    {
        while (true)
        {
            // In world A
            bool isInWorldA = (_cameraA.GetComponent<AudioListener>().enabled && _cameraA.gameObject.activeInHierarchy)
                || _cutsceneCameraA.enabled;
           // Debug.Log(Camera.current.name);
            if (isInWorldA && _worldAAudio.mute == true)
            {
                _worldAAudio.mute = false;
                _worldBAudio.mute = true;
            }
            else if (!isInWorldA && _worldBAudio.mute == true)
            {
                _worldAAudio.mute = true;
                _worldBAudio.mute = false;
            }
            yield return new WaitForSeconds(0.5f);
        }

       // yield return null;
    }
}
