using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadTrigger : MonoBehaviour {
	public GameObject platform;
	public Color start = Color.green;
	public Color used = Color.red;
	private Renderer rend;
	public AudioSource source;
	public AudioClip clip;

	// Use this for initialization
	void Start () {
		platform.SetActive(false);
		rend = GetComponent<Renderer> ();
		rend.material.color = start;
		source.clip = clip;
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			source.Play();
			platform.SetActive (true);
			rend.material.color = used;
		}
	}
//	void OnTriggerExit(Collider other){
//	}
}
