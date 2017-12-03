using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeUILogic : MonoBehaviour {
    public GameObject _listeningGO;
    public Color _inactiveColor;
    private AudioListener _listeningAudioListener;
    private Color _origColor;
	// Use this for initialization
	void Start () {
        _listeningAudioListener = _listeningGO.GetComponent<AudioListener>();
        _origColor = gameObject.GetComponent<UnityEngine.UI.Image>().color;

    }
	
	// Update is called once per frame
	void Update () {
        if (_listeningAudioListener.enabled)
        {
            gameObject.GetComponent<UnityEngine.UI.Image>().color = _origColor;
        }
        else
        {
            gameObject.GetComponent<UnityEngine.UI.Image>().color = _inactiveColor;
        }
	}
}
