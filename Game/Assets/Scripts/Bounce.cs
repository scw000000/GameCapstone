using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour {

	public float speed;
	public float rotSpeed;
	public float jumpHeight;
	public bool isGrounded;

	Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		isGrounded = true;
	}
	
	// Update is called once per frame
	void Update () {
		var y = Input.GetAxis ("Horizontal") * rotSpeed;
		var z = Input.GetAxis ("Vertical") * speed;

		transform.Translate (0, 0, z);
		transform.Rotate (0, y, 0);
		if (Input.GetKey (KeyCode.Space) && isGrounded == true) {
			rb.AddForce (0, jumpHeight, 0);
			isGrounded = false;
		}
	}

	void OnCollisionEntrer(Collision col){
		isGrounded = true;
		if (col.gameObject.CompareTag ("Bounce")) {
			rb.AddForce (0, jumpHeight *2, 0);
		}
	}
}
