using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadLogic : MonoBehaviour {
    public float _chargeTime = 3f;
    public float _ejectMagtitude = 30f;
    private GameObject _player;
    private float _currentChargeTime = 0f;
    private bool _playerInsideTrigger = false;
	// Use this for initialization
	void Start () {
        _currentChargeTime = 0f;
       //StartCoroutine("JumpPadLifeCycle");
	}
	
	// Update is called once per frame
	void Update () {
        if (_playerInsideTrigger) {
            if (_player == null)
            {
                return;
            }
            if (_player.layer == gameObject.layer
                || (gameObject.layer == LayerMask.NameToLayer("WorldAInPortal") && _player.layer == LayerMask.NameToLayer("WorldA"))
                || (gameObject.layer == LayerMask.NameToLayer("WorldBInPortal") && _player.layer == LayerMask.NameToLayer("WorldB")))
            {
                _currentChargeTime += Time.deltaTime;
            }
            else
            {
                _currentChargeTime = 0f;
            }
            // Debug.Log(_currentChargeTime);
            if (_currentChargeTime >= _chargeTime)
            {
                var fpsController = _player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
                fpsController.PerformEject(_ejectMagtitude);
                Debug.Log("Eject!");
                //rb.AddForce( 0f, _ejectMagtitude, 0f, ForceMode.Impulse);
                _currentChargeTime = 0f;
            }
        }
	}

    // Start counting time
    void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Player")) {
            _player = other.gameObject;
            _playerInsideTrigger = true;
        }
    }
    
    void OnTriggerStay(Collider other) {
    }

    void OnTriggerExit(Collider other) {
        _currentChargeTime = 0f;
        _playerInsideTrigger = false;
    }

    void Eject(GameObject go) {

    }
}
