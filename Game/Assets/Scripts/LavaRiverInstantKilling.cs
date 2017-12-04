using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRiverInstantKilling : MonoBehaviour {
    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            var statusComp = other.GetComponent<PlayerStatus>();
            statusComp.InstantKill();
            if (statusComp == null)
            {
                Debug.Log("Null Player!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            var statusComp = collision.gameObject.GetComponent<PlayerStatus>();
            statusComp.InstantKill();
            if (statusComp == null)
            {
                Debug.Log("Null Player!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
