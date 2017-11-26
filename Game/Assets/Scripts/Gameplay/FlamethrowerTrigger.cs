using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerTrigger : MonoBehaviour {
    private GameObject _playerGO;
    public bool _playerInFire = false;
	// Use this for initialization
	void Start () {
        _playerGO = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _playerInFire = true;
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _playerInFire = false;
        }
    }
}
