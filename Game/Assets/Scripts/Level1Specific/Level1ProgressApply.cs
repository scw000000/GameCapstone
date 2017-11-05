using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1ProgressApply : MonoBehaviour {
    public GameObject _fakeLensObj;
	// Use this for initialization
	void Start () {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>()._currentProgress >= 2) 
            {
            _fakeLensObj.GetComponent<MoveLensToAltar>()._hasFinished = true;
            }
        Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
