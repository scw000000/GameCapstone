using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour {
    public AudioSource _worldAAudio;
    public AudioSource _worldBAudio;
    private bool _inWorldA;
    private bool _inWorldB;
    // Use this for initialization
    void Start () {
        var audios = gameObject.GetComponents<AudioSource>();
        _worldAAudio = audios[0];
        _worldBAudio = audios[1];
        _inWorldA = true;
        _inWorldB = false;
        _worldAAudio.loop = true;
        _worldBAudio.loop = true;

        _worldAAudio.mute = false;
        _worldBAudio.mute = true;

        _worldAAudio.Play();
        _worldBAudio.Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_inWorldA)
            {
                //From A to B
                _inWorldA = false;
                _inWorldB = true;
                _worldAAudio.mute = true;
                _worldBAudio.mute = false;
            }
            else
            {
                //From B to A
                _inWorldA = true;
                _inWorldB = false;
                _worldAAudio.mute = false;
                _worldBAudio.mute = true;
            }
        }
	}
}
