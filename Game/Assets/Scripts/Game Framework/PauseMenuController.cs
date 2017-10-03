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
			Pause();
		}
	}

    public void Pause(){
        if (canvas.gameObject.activeInHierarchy == false) {
            _gameManagerComp.SetGameRuntate(false);
            canvas.gameObject.SetActive (true);
		} else {
            _gameManagerComp.SetGameRuntate(true);
            canvas.gameObject.SetActive (false);
        }
	}
}
