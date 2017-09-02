using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongPauseMenuController : MonoBehaviour {
	//For future use to disable player
	//public Transform Player;
	public Transform canvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			this.Pause();
		}
	}

	public void Pause(){
		if (canvas.gameObject.activeInHierarchy == false) {
			canvas.gameObject.SetActive (true);
			Time.timeScale = 0;
			//When the player controller is ready, this line of code will pause user control
			//Player.GetComponent<PonePlayerController> ().enable = false;
		} else {
			canvas.gameObject.SetActive (false);
			Time.timeScale = 1;
			//When the player controller is ready, this line of code will resume user control
			//Player.GetComponent<PonePlayerController> ().enable = true;
		}
	}
}
