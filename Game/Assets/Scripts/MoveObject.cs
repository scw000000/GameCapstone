﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {

	public GameObject item;
	public GameObject _tempParent;
	public Transform _guide;
    private bool _isPlayerInTrigger = false;
    private bool _isBeingHold = false;
	// Use this for initialization
	void Start () {
		item.GetComponent<Rigidbody> ().useGravity = true;
        if (_tempParent == null)
        {
            _tempParent = GameObject.Find("HoldRoot");
            _guide = _tempParent.transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E) )
        {
            if (_isPlayerInTrigger && !_isBeingHold)
            {
                _isBeingHold = true;
                item.GetComponent<Rigidbody>().useGravity = false;
                item.GetComponent<Rigidbody>().isKinematic = true;
                item.transform.position = _guide.transform.position;
                item.transform.rotation = _guide.transform.rotation;
                item.transform.parent = _tempParent.transform;
            }
            else if (_isBeingHold)
            {
                _isBeingHold = false;
                item.GetComponent<Rigidbody>().useGravity = true;
                item.GetComponent<Rigidbody>().isKinematic = false;
                item.transform.parent = null;
                item.transform.position = _guide.transform.position;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Player"))
        {
            return;
        }

        _isPlayerInTrigger = true;
    }

 //   void OnTriggerStay(Collider col){
	//	if (col.gameObject.tag == "Player"){
	//		Debug.Log ("coll");
	//		if (Input.GetKeyDown (KeyCode.E)) {
	//			Debug.Log ("key");
	//			item.GetComponent<Rigidbody> ().useGravity = false;
	//			item.GetComponent<Rigidbody> ().isKinematic = true;
	//			item.transform.position = guide.transform.position;
	//			item.transform.rotation = guide.transform.rotation;
	//			item.transform.parent = tempParent.transform;
	//		}
	//	}
	//}

	void OnTriggerExit(Collider col){
        //if (col.gameObject.tag == "Player") {
        //	Debug.Log ("coll");
        //	if (Input.GetKeyUp (KeyCode.E)) {
        //		Debug.Log ("Key Up");
        //		item.GetComponent<Rigidbody> ().useGravity = true;
        //		item.GetComponent<Rigidbody> ().isKinematic = false;
        //		item.transform.parent = null;
        //		item.transform.position = guide.transform.position;
        //	}
        //}

        if (!col.tag.Equals("Player"))
        {
            return;
        }

        _isPlayerInTrigger = false;
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
