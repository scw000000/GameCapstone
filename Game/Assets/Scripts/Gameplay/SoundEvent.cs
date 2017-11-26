using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEvent : MonoBehaviour {
    private bool _isPlaying = false;
    private AudioSource _soundSrc;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_soundSrc == null)
        {
            _soundSrc = GetComponent<AudioSource>();
        }
        if (!_soundSrc.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (_soundSrc == null)
        {
            _soundSrc = GetComponent<AudioSource>();
        }
        if (!_soundSrc.isPlaying )
        {
            TryPlaySound();
        }
    }

    private void TryPlaySound()
    {
        _soundSrc.Play();
    }
}
