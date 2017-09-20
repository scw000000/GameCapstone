using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {
    private bool _isPlayerAlive = true;
    public GameObject _currentPortal;
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.G)){
            _isPlayerAlive = false;
        }
    }

    // Cheap Killing Detection
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("Deadly"))
        {
            _isPlayerAlive = false;
        }

    }

    public bool IsAlive() {
        return _isPlayerAlive;
    }
}
