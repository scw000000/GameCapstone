using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPortalSwitcher : MonoBehaviour {
    public GameObject _laserObject;
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

    void Update()
    {
        if (gameObject.layer == _worldALayer || gameObject.layer == _worldBLayer)
        {
            _laserObject.layer = gameObject.layer;
        }
        else if (gameObject.layer == _worldAInPortalLayer || gameObject.layer == _worldBInPortalLayer)
        {
            _laserObject.layer = gameObject.layer == _worldAInPortalLayer ? _worldBLayer : _worldALayer;
        }
    }
}
