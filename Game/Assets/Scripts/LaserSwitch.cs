using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSwitch : MonoBehaviour
{
    GameObject laserObject;
    private bool triggered;
    Vector3 initPos;
    
    void SwitchOn()
    {
        //Debug.Log("Switched on");
        laserObject = GameObject.FindGameObjectWithTag("Laser");
         laserObject.SendMessage("LaserOn");
    }

}