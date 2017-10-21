using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensFlareControl : MonoBehaviour {
    public string _existInLayerName = "WorldB";
    private int _existLayer;
    private GameObject _player;
    private WorldSwitch _playerSwitchComp;
    private LensFlare _lensFlare;
    public bool _avaliable = true;
    private WorldSwitchSphere _cameraASwitchComp;
    private WorldSwitchSphere _cameraBSwitchComp;
    // Use this for initialization
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _existLayer = LayerMask.NameToLayer(_existInLayerName);
        _playerSwitchComp = _player.GetComponent<WorldSwitch>();
        _lensFlare = gameObject.GetComponent<LensFlare>();
    }

    //// Update is called once per frame
    void Update()
    {
        if (_avaliable && ( ( _player.layer == _existLayer && !_player.GetComponent<WorldSwitch>()._insidePortal ) ||
            (_player.layer != _existLayer && _player.GetComponent<WorldSwitch>()._insidePortal) )
        //_player.layer == (_existInLayerName == "WorldA" ? LayerMask.NameToLayer("WorldAInPortal") : LayerMask.NameToLayer("WorldBInPortal"))
                )
        {
            if (_cameraASwitchComp == null) {
                var camSet = GameObject.FindGameObjectWithTag("MainCamera");
                _cameraASwitchComp = GameObject.Find("CameraA").gameObject.GetComponent<WorldSwitchSphere>();
                _cameraBSwitchComp = GameObject.Find("CameraB").gameObject.GetComponent<WorldSwitchSphere>();
            }
            // only when the light is inside the switch sphere do we need to open lens flare effect
            
            float distance = Vector3.Distance(gameObject.transform.position, _player.transform.position);
            if ((!_cameraASwitchComp.enabled && !_cameraBSwitchComp.enabled)
                || distance < (_cameraASwitchComp.enabled ? _cameraASwitchComp._currSphereRadius : _cameraBSwitchComp._currSphereRadius))
            {
                _lensFlare.enabled = true;
            }
            else
            {
                _lensFlare.enabled = false;
            }
            // _lensFlare.enabled = true;
        }
        else {
            _lensFlare.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            _avaliable = false;
            _lensFlare.enabled = false;
            // Disable itself as well to prevent it turn on lensflare again
            //  enabled = false;
        }
    }
}
