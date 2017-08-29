using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {
    private bool IsPlayerAlive = true; 
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.G)){
            IsPlayerAlive = false;
        }
    }

    // Cheap Killing Detection
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("Deadly"))
        {
            IsPlayerAlive = false;
        }

    }

    public bool IsAlive() {
        return IsPlayerAlive;
    }
}
