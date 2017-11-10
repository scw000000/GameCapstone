using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLogic : MonoBehaviour {
    public bool _enableSlingShot = false;
    public bool _enableLaser = false;
    public GameObject _triggerEventObject;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.name.Equals("Ball") || !_enableSlingShot)
        {
            return;
        }
        Debug.Log("Triggering!");
        FireEvent();
    }

    public void TriggerEvent()
    {
        if (!_enableLaser)
        {
            return;
        }
        FireEvent();
    }

    private void FireEvent()
    {
        if (_triggerEventObject == null )
        {
            return;
        }

        _triggerEventObject.SetActive(true);
    }
}
