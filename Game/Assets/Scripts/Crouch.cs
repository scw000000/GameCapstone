using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour {

	private bool isCrouch;
	private CharacterController cc;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController> ();
		isCrouch = false;

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.C)) {
			if (isCrouch == false) {
				cc.height = 0.8f;
				isCrouch = true;
			} else {
				cc.height = 2.0f;
				isCrouch = false;
			}
		}
	}
}
