using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	private GameObject player;
	private bool isAttached;
	private Vector3 velocity;
	private Vector3 locShift;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		isAttached = true;
		velocity = new Vector3 (0.0f,0.0f,0.0f);
		locShift = new Vector3 (0.0f,0.0f,1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePosition ();

	}

	void UpdatePosition(){

		if (isAttached) {
			if (player) {
				transform.position = player.transform.position + locShift;

				if (Input.GetKeyDown (KeyCode.Space)) {
					Launch ();
				}
			}
		} else {
			transform.Translate (velocity*Time.deltaTime);
			if(Input.GetKeyDown (KeyCode.R)){
				Respawn();
			}
		}
	}

	void Launch(){
		if(isAttached){
			if (Input.GetKey(KeyCode.D)) {
				velocity = new Vector3 (9.0f, 0.0f, 12.0f);
			} else if (Input.GetKey(KeyCode.A)) {
				velocity = new Vector3 (-9.0f, 0.0f, 12.0f);
			} else {
				velocity = new Vector3 (0.0f, 0.0f, 12.0f);
			}
		}
		isAttached = false;
	}
	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "Wall"){
			ChangeXVelocity ();
		}
		if(col.gameObject.tag == "PlayerTag"){
			ChangeZVelocity ();
		}
	}
	void Respawn(){
		player = GameObject.Find ("Player");
		isAttached = true;
		velocity = new Vector3 (0.0f,0.0f,0.0f);
		locShift = new Vector3 (0.0f,0.0f,1.0f);
		transform.position = player.transform.position + locShift;
	}

	public void ChangeXVelocity(){
		velocity = new Vector3 (-velocity.x,velocity.y,velocity.z);
	}

	public void ChangeZVelocity(){
		velocity = new Vector3 (velocity.x,velocity.y,-velocity.z);
	}
}
