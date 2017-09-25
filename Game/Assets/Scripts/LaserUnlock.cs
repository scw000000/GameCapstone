using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserUnlock : MonoBehaviour {
    public GameObject eventObject;
    private bool triggered;
    private float fallSpeed;
    Vector3 initPos;
    // Use this for initialization
    void Start () {
        triggered = false;
        fallSpeed = .1f;
        initPos = eventObject.transform.position;
    }

    void FixedUpdate()
    {
        if(triggered == true)
        {
            if (eventObject.transform.position.y <=-50)
            {
                eventObject.SetActive(false);
            }
            else
            {
                initPos.y -= fallSpeed;
                eventObject.transform.position = initPos;
            }
        }
    }

    void TriggerEvent()
    {
        triggered = true;

    }
}
