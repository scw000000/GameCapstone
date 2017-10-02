using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantKill : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player")) {
            var aliveComp = other.GetComponent<PlayerStatus>();
            aliveComp._isAlive = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide");
        if (collision.gameObject.tag.Equals("Player"))
        {
            var aliveComp = collision.gameObject.GetComponent<PlayerStatus>();
            aliveComp._isAlive = false;
        }
    }
}
