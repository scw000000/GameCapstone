using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAutoTrigger : MonoBehaviour {
    public GameObject _doorObj;
    public bool _triggerOpen;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Player")) {
            _doorObj.GetComponent<DoorControl>()._isOpened = _triggerOpen;
        }
    }
}
