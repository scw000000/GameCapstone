using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformHoldPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			Debug.Log ("YES!!!");
			other.transform.parent = this.transform;
		}
	}

//	void OnTriggerExit(Collider other){
//		if (other.tag == "Player") {
//			other.transform.parent = null;
//		}
//	}
}
