using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonTrigger : MonoBehaviour {

	public GameObject elevator;

	// Use this for initialization
	void Start () {
		elevator.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		elevator.SetActive (true);
	}

	void OnTriggerExit(Collider other){
		elevator.SetActive (false);
	}
}
