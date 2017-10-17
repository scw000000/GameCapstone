using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {
    public GameObject _spawnObjects;
    private int _laserSpawnCheck;
    // Use this for initialization
    void Start () {
        _laserSpawnCheck = 0;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void TriggerEvent()
    {
        _laserSpawnCheck += 1;
        if (_laserSpawnCheck == 1)
        {
            _spawnObjects.SetActive(true);
            if (gameObject.GetComponent<CutSceneControl>() != null) {
                gameObject.GetComponent<CutSceneControl>().StartTimeLine();
            }
        }

    }
}
