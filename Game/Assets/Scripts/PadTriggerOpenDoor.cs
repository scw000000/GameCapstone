using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadTriggerOpenDoor : MonoBehaviour {

	public GameObject Door1;
	public GameObject Door2;
	public AudioSource source;
	public AudioClip clip;

	// Use this for initialization
	void Start () {
		Door2.SetActive(true);
		Door1.SetActive(false);
		source.clip = clip;
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			Door2.SetActive (false);
			Door1.SetActive (true);
			source.Play ();
		}
	}
}
