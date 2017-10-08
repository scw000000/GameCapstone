using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingLogic : MonoBehaviour {
    private GameObject _cameraRootGO;
    public GameObject _portalBulletPrefab;
    public float _coolDownTime;
    private float _currentCoolDown;
    // Use this for initialization
    void Start () {
        _cameraRootGO = gameObject.transform.Find("CameraRoot").gameObject;
        _currentCoolDown = _coolDownTime;
    }
	
	// Update is called once per frame
	void Update () {
        _currentCoolDown = Mathf.Min(_currentCoolDown + Time.deltaTime, _coolDownTime);
        if (Input.GetButtonDown("Fire1") && _currentCoolDown >= _coolDownTime )
        {
            _currentCoolDown = 0f;
            var forwardVec = _cameraRootGO.transform.forward;
            var statusComp = gameObject.GetComponent<PlayerStatus>();
            if (statusComp._currentPortal != null) {
                statusComp._currentPortal.GetComponent<PortalLogic>()._active = false;
            }
            if (statusComp._currentPortalBullet != null) {
                Destroy(statusComp._currentPortalBullet);
                statusComp._currentPortalBullet = null;
            }
            statusComp._currentPortalBullet = Instantiate(_portalBulletPrefab, _cameraRootGO.transform.position + forwardVec * 2, _cameraRootGO.transform.rotation);
        }

        if (Input.GetButtonDown("Fire2")) {
            var statusComp = gameObject.GetComponent<PlayerStatus>();
            if (statusComp._currentPortalBullet != null)
            {
                statusComp._currentPortalBullet.GetComponent<PortalBulletLogic>().GeneratePortal();
                statusComp._currentPortalBullet = null;
            }
            else if (statusComp._currentPortal != null)
            {
                statusComp._currentPortal.GetComponent<PortalLogic>()._active = false;
            }
        }
    }
}
