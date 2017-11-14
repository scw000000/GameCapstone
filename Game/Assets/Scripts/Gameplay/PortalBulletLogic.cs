﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBulletLogic : MonoBehaviour {
    public float _maxFlyTime;
    public GameObject _portalPreFab;
    private bool _enablePortalGen = true;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 30, ForceMode.Impulse);
        // Distroy itself to prevent flying too long
        // Destroy(gameObject, _maxFlyTime);
        Invoke("DestroySelf", _maxFlyTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collider){
        // if it's trigger then don't trigger portal
        if (collider.isTrigger) {
            return;
        }
        if (collider.tag.Equals("Transportable")
            && (GameObject.FindGameObjectWithTag("Player").layer != collider.gameObject.layer && 
                    (collider.gameObject.layer == LayerMask.NameToLayer("WorldA") 
                || collider.gameObject.layer == LayerMask.NameToLayer("WorldB") ) )
                )
        {
            _enablePortalGen = false;
            collider.gameObject.layer = GameObject.FindGameObjectWithTag("Player").layer;
            GetComponent<AudioSource>().Play();
            Invoke("DestroySelf", GetComponent<AudioSource>().clip.length);
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            GeneratePortal();
        }
    }

    public void GeneratePortal() {
        if (!_enablePortalGen)
        {
            return;
        }
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        gameObject.GetComponent<Collider>().enabled = false;
        if (playerGO != null)
        {
            var statusComp = playerGO.GetComponent<PlayerStatus>();
            if (statusComp._currentPortal != null)
            {
                statusComp._currentPortal.GetComponent<PortalLogic>()._active = false;
            }
            var portalInstance = Instantiate(_portalPreFab, gameObject.transform.position, Quaternion.identity);
            portalInstance.GetComponent<PortalLogic>()._active = true;
            statusComp._currentPortal = portalInstance;
        }
        DestroySelf();
    }

    void DestroySelf() {
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            var statusComp = playerGO.GetComponent<PlayerStatus>();

            if (statusComp._currentPortalBullet != null &&
                statusComp._currentPortalBullet.GetInstanceID() == gameObject.GetInstanceID())
            {
                statusComp._currentPortalBullet = null;
            }
        }
        Destroy(gameObject);
    }
}
