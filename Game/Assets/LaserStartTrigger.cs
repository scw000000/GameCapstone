using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserStartTrigger : MonoBehaviour {
    private bool _isPlayerInside;
    private GameObject _playerObject;
    public GameObject _laserBeam;
    public int _layerNumber;//Set 8 if you want trigger in world A, 9 in world B
	// Use this for initialization
	void Start () {
        _isPlayerInside = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(_isPlayerInside)
        {
            //Layer 9 is WorldB
            if (Input.GetButton("Interaction") && _playerObject.layer == _layerNumber)
            {
                _laserBeam.SendMessage("LaserOn");
            }
        }
	}
    void OnTriggerEnter(Collider other)
    {
        
        if (other.tag.Equals("Player"))
        {
            _isPlayerInside = true;
            _playerObject = other.gameObject;
        }
    }
}
