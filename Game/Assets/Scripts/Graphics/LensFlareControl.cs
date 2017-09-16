using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensFlareControl : MonoBehaviour {
    public string _existInLayerName = "WorldB";
    private int _existLayer;
    private GameObject _player;
    private WorldSwitch _playerSwitchComp;
    private LensFlare _lensFlare;
    private WorldSwitchSphere _cameraASwitchComp;
    private WorldSwitchSphere _cameraBSwitchComp;
    // Use this for initialization
    void Start () {
        Invoke("Init", 4);    

	}

    void Init() {
        do
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }
        while (_player == null);
        _existLayer = LayerMask.NameToLayer(_existInLayerName);
        _playerSwitchComp = _player.GetComponent<WorldSwitch>();
        _lensFlare = gameObject.GetComponent<LensFlare>();
        var camSet = GameObject.FindGameObjectWithTag("MainCamera");
        _cameraASwitchComp = camSet.transform.Find("CameraA").gameObject.GetComponent<WorldSwitchSphere>();
        _cameraBSwitchComp = camSet.transform.Find("CameraB").gameObject.GetComponent<WorldSwitchSphere>();
    }
	
	// Update is called once per frame
	void Update () {
        if (_player != null) {
            if (_player.layer == _existLayer && !_playerSwitchComp._insidePortal)
            {
                // only when the light is inside the switch sphere do we need to open lens flare effect
                float distance = Vector3.Distance(gameObject.transform.position, _player.transform.position);
                if ((!_cameraASwitchComp.enabled && !_cameraBSwitchComp.enabled)
                    || distance < (_cameraASwitchComp.enabled ? _cameraASwitchComp._currSphereRadius : _cameraBSwitchComp._currSphereRadius))
                {
                    _lensFlare.enabled = true;
                }
                else {
                    _lensFlare.enabled = false;
                }
            }
            else
            {
                _lensFlare.enabled = false;
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            Debug.Log("OUUUUUT");
          //  _lensFlare.enabled = false;
            // Disable itself as well to prevent it turn on lensflare again
          //  enabled = false;
        }
    }
}
