using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKeyCube : MonoBehaviour {

	public GameObject _spawnObjects;
	private int _laserSpawnCheck;

	// Use this for initialization
	void Start () {
		_spawnObjects.SetActive (false);
		_laserSpawnCheck = 0;
	}
	
	// Update is called once per frame
	void TriggerEvent(){
		_laserSpawnCheck += 1;
		if (_laserSpawnCheck == 1) {
			_spawnObjects.SetActive (true);
		}
	}
}
