using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarLogic : MonoBehaviour {
    public PlayerStatus _playerStatusComp;
    private Slider _healthSlider;
    // Use this for initialization
    void Start () {
        _healthSlider = gameObject.GetComponent<Slider>();
    }
	
	// Update is called once per frame
	void Update () {
        if (_playerStatusComp != null) {
            _healthSlider.value = _playerStatusComp.GetHitPoint();
        }
    }
}
