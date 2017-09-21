using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSwitch : MonoBehaviour
{
    GameObject laserObject;
    private bool triggered;
    Vector3 initPos;
    private int _worldALayer;
    private int _worldBLayer;
    private int _worldAInPortalLayer;
    private int _worldBInPortalLayer;
    void Start()
    {
        _worldALayer = LayerMask.NameToLayer("WorldA");
        _worldBLayer = LayerMask.NameToLayer("WorldB");
        _worldAInPortalLayer = LayerMask.NameToLayer("WorldAInPortal");
        _worldBInPortalLayer = LayerMask.NameToLayer("WorldBInPortal");
    }

    void SwitchOn()
    {
        //Debug.Log("Switched on");
        laserObject = GameObject.FindGameObjectWithTag("Laser");
         laserObject.SendMessage("LaserOn");
    }

    void Update()
    {
        laserObject = GameObject.FindGameObjectWithTag("Laser");
        if (gameObject.layer == _worldALayer || gameObject.layer == _worldBLayer) {
            laserObject.layer = gameObject.layer;
        }
        else if (gameObject.layer == _worldAInPortalLayer || gameObject.layer == _worldBInPortalLayer) {
            laserObject.layer = gameObject.layer == _worldAInPortalLayer ? _worldBLayer: _worldALayer;
        }
    }
}