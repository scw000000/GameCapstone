using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongPlayerState : MonoBehaviour {

    private bool IsPlayerAlive = true;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            IsPlayerAlive = false;
        }
    }

    // Cheap Killing Detection
    void OnCollisionEnter(Collision other)
    {

    }

    public bool IsAlive()
    {
        return IsPlayerAlive;
    }
}
