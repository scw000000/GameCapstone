using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEvent : MonoBehaviour {
    public GameObject[] _gameObjects;
    public bool[] _setActiveValues;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        for (int i = 0; i < _gameObjects.Length; ++i)
        {
            _gameObjects[i].SetActive(_setActiveValues[i]);
        }
    }
}
