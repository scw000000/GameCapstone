using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRiverInstantKilling : MonoBehaviour {
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            var command = collision.gameObject.GetComponent<PlayerStatus>();
            command.InstantKill();
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
