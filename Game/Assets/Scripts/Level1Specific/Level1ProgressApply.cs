using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1ProgressApply : MonoBehaviour {
    public GameObject _fakeLensObj;
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>()._currentProgress >= 2)
        {
            _fakeLensObj.GetComponent<MoveLensToAltar>()._hasFinished = true;
        }
        gameObject.SetActive(false);
    }
}
