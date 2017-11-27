using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZoneGenerator : MonoBehaviour {

    public GameObject _windZone;
	// Use this for initialization
	void Start () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Vector3 temp = new Vector3 (-286.7048f, -54.92f, -141.9403f);
            _windZone.transform.position = temp;
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
