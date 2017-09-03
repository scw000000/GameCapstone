using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringDetection : MonoBehaviour {
    public bool IsPlayerScore;
    public GameObject GameManager;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Ball")) {
            GameManager.SendMessage("Score", IsPlayerScore);
            Debug.Log("Score!");
        }
    }
}
