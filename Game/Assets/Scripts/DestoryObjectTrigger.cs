using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryObjectTrigger : MonoBehaviour {
    public GameObject _destory;

    private bool isTriggered;
	// Use this for initialization
	void Start () {
        isTriggered = false;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            isTriggered = true;
        }
    }
    // Update is called once per frame
    void Update () {
        if (isTriggered)
        {
            GameObject.Destroy(_destory);
        }
	}
}
