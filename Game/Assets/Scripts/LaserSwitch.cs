using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSwitch : MonoBehaviour
{
    GameObject laserObject;
    private bool triggered;
    Vector3 initPos;
    // Use this for initialization
    void Start()
    {
        laserObject = GameObject.FindGameObjectWithTag("Laser");
    }

    void SwitchOn()
    {
        laserObject.SendMessage("LaserOn");
    }
}