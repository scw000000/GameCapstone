using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {
    public GameObject _laserPuzzle;
    private int _laserSpawnCheck;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void TriggerEvent()
    {
        _laserSpawnCheck += 1;
        if (_laserSpawnCheck == 1)
        {
            _laserPuzzle.SetActive(true);
        }

    }
}
