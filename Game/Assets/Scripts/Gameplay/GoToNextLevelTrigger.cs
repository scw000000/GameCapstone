using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextLevelTrigger : MonoBehaviour {
    private bool _hasEntered = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Player"))
        {
            return;
        }
        if (!_hasEntered)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().GoToNextLevel();
            _hasEntered = true;
        }
    }
}
