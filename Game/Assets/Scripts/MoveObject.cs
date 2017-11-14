using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {

	public GameObject item;
	public GameObject tempParent;
	public Transform guide;

	// Use this for initialization
	void Start () {
		item.GetComponent<Rigidbody> ().useGravity = true;
	}
	
	// Update is called once per frame
	void Update () {
//		if (Input.GetKeyDown(KeyCode.E)) {
//			item.GetComponent<Rigidbody> ().useGravity = false;
//			item.GetComponent<Rigidbody> ().isKinematic = true;
//			item.transform.position = guide.transform.position;
//			item.transform.rotation = guide.transform.rotation;
//			item.transform.parent = tempParent.transform;
//		}
//
//		if (Input.GetKeyUp (KeyCode.E)) {
//			item.GetComponent<Rigidbody> ().useGravity = true;
//			item.GetComponent<Rigidbody> ().isKinematic = false;
//			item.transform.parent = null;
//			item.transform.position = guide.transform.position;
//		}
	}

	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "Player"){
			Debug.Log ("coll");
			if (Input.GetKeyDown (KeyCode.E)) {
				Debug.Log ("key");
				item.GetComponent<Rigidbody> ().useGravity = false;
				item.GetComponent<Rigidbody> ().isKinematic = true;
				item.transform.position = guide.transform.position;
				item.transform.rotation = guide.transform.rotation;
				item.transform.parent = tempParent.transform;
			}
		}
	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Player") {
			Debug.Log ("coll");
			if (Input.GetKeyUp (KeyCode.E)) {
				Debug.Log ("Key Up");
				item.GetComponent<Rigidbody> ().useGravity = true;
				item.GetComponent<Rigidbody> ().isKinematic = false;
				item.transform.parent = null;
				item.transform.position = guide.transform.position;
			}
		}
	}

//	void OnTriggerExit(Collider col){
//		if (col.gameObject.tag == "Player" && Input.GetKeyUp (KeyCode.R)) {
//			item.GetComponent<Rigidbody> ().useGravity = true;
//			item.GetComponent<Rigidbody> ().isKinematic = false;
//			item.transform.parent = null;
//			item.transform.position = guide.transform.position;
//		}
//	}
//	void OnMouseDown(){
//		item.GetComponent<Rigidbody> ().useGravity = false;
//		item.GetComponent<Rigidbody> ().isKinematic = true;
//		item.transform.position = guide.transform.position;
//		item.transform.rotation = guide.transform.rotation;
//		item.transform.parent = tempParent.transform;
//	}
//
//	void OnMouseUp(){
//		item.GetComponent<Rigidbody> ().useGravity = true;
//		item.GetComponent<Rigidbody> ().isKinematic = false;
//		item.transform.parent = null;
//		item.transform.position = guide.transform.position;
//	}
}
