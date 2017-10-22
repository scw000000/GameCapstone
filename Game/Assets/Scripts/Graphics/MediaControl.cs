using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaControl : MonoBehaviour {
    private int _existLayer;
    private GameObject _player;
   // private WorldSwitch _playerSwitchComp;
    private LensFlare _lensFlare;
    private Light _light;
    public bool _avaliable = true;
    public bool _useDisableTrigger = false;
    private WorldSwitchSphere _cameraASwitchComp;
    private WorldSwitchSphere _cameraBSwitchComp;
    // Use this for initialization
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _existLayer = gameObject.layer;
       // _playerSwitchComp = _player.GetComponent<WorldSwitch>();
        _lensFlare = gameObject.GetComponent<LensFlare>();
        _light = gameObject.GetComponent<Light>();
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
                _cameraASwitchComp = GameObject.Find("CameraA").gameObject.GetComponent<WorldSwitchSphere>();
                _cameraBSwitchComp = GameObject.Find("CameraB").gameObject.GetComponent<WorldSwitchSphere>();
            }
            // only when the light is inside the switch sphere do we need to open lens flare effect
            
            float distance = Vector3.Distance(gameObject.transform.position, _player.transform.position);
            if ((!_cameraASwitchComp.enabled && !_cameraBSwitchComp.enabled)
                || distance < (_cameraASwitchComp.enabled ? _cameraASwitchComp._currSphereRadius : _cameraBSwitchComp._currSphereRadius))
            {
                if (_lensFlare != null)
                {
                    _lensFlare.enabled = true;
                }
                if (_light != null) {
                    _light.enabled = true;
                }
                
            }
            else
            {
                if (_lensFlare != null)
                {
                    _lensFlare.enabled = false;
                }
                if (_light != null)
                {
                    _light.enabled = false;
                }
            }
            // _lensFlare.enabled = true;
        }
        else {
            if (_lensFlare != null)
            {
                _lensFlare.enabled = false;
            }
            if (_light != null)
            {
                _light.enabled = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!_useDisableTrigger) {
            return;
        }
        if (other.gameObject.tag.Equals("Player"))
        {
            _avaliable = false;
            if (_lensFlare != null)
            {
                _lensFlare.enabled = false;
            }
            if (_light != null)
            {
                _light.enabled = false;
            }
            // Disable itself as well to prevent it turn on lensflare again
            //  enabled = false;
        }
    }
}
