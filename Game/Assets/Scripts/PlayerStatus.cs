using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float _maxHitPoint = 100f;
    public int _currentProgress;
    public GameObject _currentPortal;
    public GameObject _currentPortalBullet;
    private float _hitPoint;
    public float wait;

    // Use this for initialization
    void Start()
    {
        _hitPoint = 1f;
        wait = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            InstantKill();
        }
        if (wait > 0.0f)
        {
            wait -= Time.deltaTime;
        }
    }

    public float GetHitPoint()
    {
        return _hitPoint;
    }

    public bool GetIsAlive()
    {
        return _hitPoint > 0f;
    }

    public void AddHitPoints(float amount)
    {
        _hitPoint += amount / _maxHitPoint;
        _hitPoint = Mathf.Clamp(_hitPoint, 0, 1);
        if (amount < 0f)
        {
            GameObject.Find("CameraSet").GetComponent<CameraFollow>().PerformDamageShake();
        }
    }

    public void InstantKill()
    {
        _hitPoint = 0f;
    }

    public void TakeDamage()
    {
        if (wait < 0.1f)
        {
            AddHitPoints(-0.2f);
            wait = 3.0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "eyes")
        {
            other.transform.parent.GetComponent<Enemy>().checkSight();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Hellephant")
        {
            this.TakeDamage();
        }
    }
}
