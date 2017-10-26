using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPortalSwitcher : MonoBehaviour {
    public GameObject _laserObject;
    public GameObject _playerObject;
    public bool _changeChildLayer = false;
    private int _worldALayer;
    private int _worldBLayer;
    private int _worldAInPortalLayer;
    private int _worldBInPortalLayer;
    void Start()
    {
        _playerObject = GameObject.FindGameObjectWithTag("Player");
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

        if (_laserObject.GetComponent<AudioSource>() != null)
        {
            if (( (_playerObject.layer == _laserObject.layer && !_playerObject.GetComponent<WorldSwitch>()._insidePortal)
                || (_playerObject.layer != _laserObject.layer && _playerObject.GetComponent<WorldSwitch>()._insidePortal) )
                )
            {
                _laserObject.GetComponent<AudioSource>().mute = false;
            }
            else {
                _laserObject.GetComponent<AudioSource>().mute = true;
            }
        }

        if (_changeChildLayer) {
            ChangeChildrenLayer(_laserObject, _laserObject.layer);
        }
    }

    private static void ChangeChildrenLayer(GameObject go, int layer) {
        for(int i = 0; i < go.transform.childCount; ++i) {
            var child = go.transform.GetChild(i).gameObject;
            child.layer = layer;
            ChangeChildrenLayer(child, layer);
        }
    }
}
