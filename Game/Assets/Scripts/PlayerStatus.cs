using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {
    public bool _isAlive { set; get; }
    public GameObject _currentPortal;
	// Use this for initialization
	void Start () {
        _isAlive = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.G)){
            _isAlive = false;
        }
    }
}
