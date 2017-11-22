using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneEvent : MonoBehaviour {
    public GameObject _cutsceneGO;
    public bool _playOnce = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        if (_cutsceneGO == null || _cutsceneGO.GetComponent<CutSceneControl>() == null) {
            return;
        }

        _cutsceneGO.GetComponent<CutSceneControl>().StartTimeLine();

        gameObject.SetActive(false);

        if (_playOnce)
        {
            enabled = false;
        }
    }
}
