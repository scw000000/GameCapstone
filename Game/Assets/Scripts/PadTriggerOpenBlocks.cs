using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadTriggerOpenBlocks : MonoBehaviour {
	public Color start = Color.green;
	public Color used = Color.red;
	private Renderer rend;

	public GameObject Door1;
	public GameObject Door2;
	public GameObject Platform1;
	public GameObject Platform2;
	public GameObject Left1;
	public GameObject Left2;
	public GameObject Right1;
	public GameObject Right2;

	public AudioSource source;
	public AudioClip clip;
	public AudioSource source2;
	public AudioClip clip2;


	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
		rend.material.color = start;
		source.clip = clip;
		source2.clip = clip2;

		Door1.SetActive(false);
		source.clip = clip;
		Door2.SetActive(true);
		Door1.SetActive(false);
		Platform2.SetActive(true);
		Platform1.SetActive(false);
		Left2.SetActive(true);
		Left1.SetActive(false);
		Right2.SetActive(true);
		Right1.SetActive(false);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			source.PlayDelayed(1);
			source2.Play();
			Door2.SetActive(false);
			Door1.SetActive(true);
			Platform2.SetActive(false);
			Platform1.SetActive(true);
			Left2.SetActive(false);
			Left1.SetActive(true);
			Right2.SetActive(false);
			Right1.SetActive(true);
			rend.material.color = used;
		}
	}

//	void OnTriggerExit(Collider other){
//	}
}
