using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBulletLogic : MonoBehaviour {
    public float _maxFlyTime;
    public GameObject _portalPreFab;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 30, ForceMode.Impulse);
        // Distroy itself to prevent flying too long
        Destroy(gameObject, _maxFlyTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collider){
        var portalInstance = Instantiate(_portalPreFab, gameObject.transform.position, Quaternion.identity);
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            playerGO.GetComponent<PlayerStatus>()._currentPortal = portalInstance;
        }
        Destroy(gameObject);
    }
}
