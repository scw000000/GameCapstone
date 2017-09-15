using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadLogic : MonoBehaviour {
    private bool _isPlayerInside = false;
    public float _chargeTime = 3f;
    public float _ejectMagtitude = 30f;
    private GameObject _player;
    private float _currentChargeTime = 0f;
	// Use this for initialization
	void Start () {
        _isPlayerInside = false;
        _currentChargeTime = 0f;
       //StartCoroutine("JumpPadLifeCycle");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Start counting time
    void OnTriggerEnter(Collider other) {
        
        if (other.tag.Equals("Player")) {
            _player = other.gameObject;
        }
    }
    
    void OnTriggerStay(Collider other) {
        _currentChargeTime += Time.deltaTime;
        // Debug.Log(_currentChargeTime);
        if (_currentChargeTime >= _chargeTime) {
            var fpsController = _player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
            fpsController.PerformEject(_ejectMagtitude);
            Debug.Log("Eject!");
            //rb.AddForce( 0f, _ejectMagtitude, 0f, ForceMode.Impulse);
            _currentChargeTime = 0f;
        }
        
    }

    void OnTriggerExit(Collider other) {
        _currentChargeTime = 0f;
    }

    //IEnumerator JumpPadLifeCycle() {
    //    while (true) {
    //        yield return Idle();
    //        yield return Charge();
    //        yield return Eject();
    //    }
    //}

    //IEnumerator Idle() {
    //    _currentChargeTime = 0f;
    //    while (!_isPlayerInside) {
    //        yield return null;
    //    }
    //    yield return null;
    //}

    //IEnumerator Charge() {
    //    while (_currentChargeTime < _chargeTime) {
    //        _currentChargeTime += Time.deltaTime;
    //    }
    //    yield return null;
    //}

    //IEnumerator Eject(){
    //    yield return null;
    //}

    void Eject(GameObject go) {

    }
}
