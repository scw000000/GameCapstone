using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {
    public float _maxHitPoint = 100f;
    public int _currentProgress;
    public GameObject _currentPortal;
    public GameObject _currentPortalBullet;
    private float _hitPoint;
    
	// Use this for initialization
	void Start () {
        _hitPoint = 1f;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.H))
        {
            AddHitPoints(-10f);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            InstantKill();
        }
    }

    public float GetHitPoint() {
        return _hitPoint;
    }

    public bool GetIsAlive() {
        return _hitPoint > 0f;
    }

    public void AddHitPoints(float amount) {
        _hitPoint += amount / _maxHitPoint;
        _hitPoint = Mathf.Clamp(_hitPoint, 0, 1);
    }

    public void InstantKill() {
        _hitPoint = 0f;
    }

    
}
