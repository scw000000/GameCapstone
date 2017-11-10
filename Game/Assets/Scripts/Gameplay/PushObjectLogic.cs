using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObjectLogic : MonoBehaviour {
    public float _pushScale = 4f;
    private bool _isInTrigger = false;
    private GameObject _playerGO;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Interaction") && _isInTrigger)
        {
            var pushDir = transform.position - _playerGO.transform.position;
            pushDir.Normalize();
            pushDir *= _pushScale;
            gameObject.GetComponent<Rigidbody>().AddForce(pushDir, ForceMode.Impulse);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Player"))
        {
            return;
        }
        _playerGO = other.gameObject;
        _isInTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.tag.Equals("Player"))
        {
            return;
        }
        _isInTrigger = false;
    }
}
