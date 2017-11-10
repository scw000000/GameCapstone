using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEvent : MonoBehaviour {
    public GameObject _spawnGOPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        var spawnObj = GameObject.Instantiate(_spawnGOPrefab, transform.position, Quaternion.identity);
        spawnObj.layer = gameObject.layer;
        gameObject.SetActive(false);
    }
}
