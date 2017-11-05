using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensFlareReset : MonoBehaviour {
    public GameObject _lensFlareObject;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            _lensFlareObject.GetComponent<MediaControl>()._avaliable = true;
            // Disable itself as well to prevent it turn on lensflare again
            //  enabled = false;
        }
    }
}
