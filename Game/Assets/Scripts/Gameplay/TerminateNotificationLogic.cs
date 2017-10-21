using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminateNotificationLogic : MonoBehaviour {
    public bool _triggerOnce = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void TerminateGameMessage() {
        GameObject.Find("GameManager").GetComponent<GameManager>().TerminateGameMessage();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Player")) {
            return;
        }

        TerminateGameMessage();

        if (_triggerOnce) {
            gameObject.SetActive(false);
        }
    }
}
