using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserUnlock : MonoBehaviour {
    GameObject eventObject;
    private string eventTag;
    private bool triggered;
    private float fallSpeed;
    Vector3 initPos;
    // Use this for initialization
    void Start () {
        eventTag = "Event";
        eventObject = GameObject.FindGameObjectWithTag("Event");
        triggered = false;
        initPos = eventObject.transform.position;
        fallSpeed = .1f;
    }

    void FixedUpdate()
    {
        if(triggered == true)
        {
            if (eventObject.transform.position.y <= -50f)
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
