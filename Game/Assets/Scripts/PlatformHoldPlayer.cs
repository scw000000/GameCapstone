using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformHoldPlayer : MonoBehaviour {

	//public GameObject player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "MovingPlatform") {
			transform.position = col.transform.position;
		}
	}

//	void OnTriggerExit(Collider col){
//		if (col.gameObject.tag == "MovingPlatform") {
//			transform.parent = null;
//		}
//	}

//	void OnCollisionEnter(Collision col){
//		if (col.gameObject.tag == "Player") {
//			Debug.Log ("YES");
//
//			//transform.position = col.transform.position;
//			player.transform.position = new Vector3(0,2,0);
//		}
//	}

//	void OnTriggerEnter(Collider other){
//
//			Debug.Log ("YES!!!");
//			other.transform.parent = transform;
//
//	}

//	void OnTriggerExit(Collider other){
//		other.transform.parent = null;
//	}

}
