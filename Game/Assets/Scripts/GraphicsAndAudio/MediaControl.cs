using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaControl : MonoBehaviour {
 //   private int _existLayer;
    private GameObject _player;
   // private WorldSwitch _playerSwitchComp;
    private LensFlare _lensFlare;
    private Light _light;
    private AudioSource _audio;
    public bool _avaliable = true;
    public bool _useDisableTrigger = false;
    private WorldSwitchSphere _cameraASwitchComp;
    private WorldSwitchSphere _cameraBSwitchComp;
    // Use this for initialization
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
     //   _existLayer = gameObject.layer;
       // _playerSwitchComp = _player.GetComponent<WorldSwitch>();
        _lensFlare = gameObject.GetComponent<LensFlare>();
        _light = gameObject.GetComponent<Light>();
        _audio = gameObject.GetComponent<AudioSource>();
    }

    //// Update is called once per frame
    void Update()
    {        //if (_avaliable && ( ( _player.layer == _existLayer && !_player.GetComponent<WorldSwitch>()._insidePortal ) ||
        //    (_player.layer != _existLayer && _player.GetComponent<WorldSwitch>()._insidePortal) )
        ////_player.layer == (_existInLayerName == "WorldA" ? LayerMask.NameToLayer("WorldAInPortal") : LayerMask.NameToLayer("WorldBInPortal"))
        //        )
        bool insidePortal = _player.GetComponent<WorldSwitch>()._insidePortal;
        if ((_player.layer == gameObject.layer && !insidePortal)
           || ((_player.layer == LayerMask.NameToLayer("WorldA") && gameObject.layer == LayerMask.NameToLayer("WorldAInPortal"))
               || (_player.layer == LayerMask.NameToLayer("WorldB") && gameObject.layer == LayerMask.NameToLayer("WorldBInPortal"))
               && insidePortal)
           || ((_player.layer == LayerMask.NameToLayer("WorldA") && gameObject.layer == LayerMask.NameToLayer("WorldBInPortal")
               || (_player.layer == LayerMask.NameToLayer("WorldB") && gameObject.layer == LayerMask.NameToLayer("WorldAInPortal")))
               && !insidePortal))
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
                if (_audio != null)
                {
                    _audio.mute = false;
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
                if (_audio != null)
                {
                    _audio.mute = false;
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
            if (_audio != null)
            {
                _audio.mute = true;
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
            if (_audio != null)
            {
                _audio.mute = true;
            }
            // Disable itself as well to prevent it turn on lensflare again
            //  enabled = false;
        }
    }
}
