using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour {
	//For future use to disable player
	//public Transform Player;
	public Transform canvas;
    private GameManager _gameManagerComp;
	// Use this for initialization
	void Start () {
        canvas.gameObject.SetActive(false);
        _gameManagerComp = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
            
			this.Pause();
		}
	}

    public void Pause(){
		if (canvas.gameObject.activeInHierarchy == false) {
            _gameManagerComp.SetPlayerInput(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            // _gameManagerComp.AttachToMainCamera(canvas.gameObject);
            canvas.gameObject.SetActive (true);
            Time.timeScale = 0;
			//When the player controller is ready, this line of code will pause user control
			//Player.GetComponent<PonePlayerController> ().enable = false;
		} else {
            _gameManagerComp.SetPlayerInput(true);
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            canvas.gameObject.SetActive (false);
            
            
            //When the player controller is ready, this line of code will resume user control
            //Player.GetComponent<PonePlayerController> ().enable = true;
        }
	}
}
