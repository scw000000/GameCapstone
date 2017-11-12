using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOpenDoor : MonoBehaviour {

	public GameObject Door1;
	public GameObject Door2;

	// Use this for initialization
	void Start () {
		Door2.SetActive(true);
		Door1.SetActive(false);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other){
		Door2.SetActive(false);
		Door1.SetActive(true);
	}

	void OnTriggerExit(Collider other){
		Door2.SetActive(true);
		Door1.SetActive(false);
	}
}
