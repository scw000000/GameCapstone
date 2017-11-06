using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccpetKey: MonoBehaviour {
	public GameObject KeyCube;
	public GameObject AcceptedKeyCube;
	public GameObject Door1;
	public GameObject Door2;

	// Use this for initialization
	void Start () {
		AcceptedKeyCube.SetActive(false);
		Door2.SetActive(true);
		Door1.SetActive(false);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "KeyCube")
		{
			KeyCube.SetActive(false);
			AcceptedKeyCube.SetActive(true);
			Door2.SetActive(false);
			Door1.SetActive(true);
		}
	}
}﻿