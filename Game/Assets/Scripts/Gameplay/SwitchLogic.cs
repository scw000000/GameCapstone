using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLogic : MonoBehaviour {
    public bool _isFlipFlop = false;
    public bool _isActive = false;
    private bool _isInTrigger = false;
    private bool _isOpertable = true;
    private MovingPlatformLogic _movePlatformComp;
    public GameObject[] _activeEventObjects;
    public GameObject[] _inactiveEventObjects;
    // Use this for initialization
    void Start () {
        _movePlatformComp = gameObject.GetComponent<MovingPlatformLogic>();
        _movePlatformComp._canReverse = false;
        
        _movePlatformComp._moveDirection = 0;
        _movePlatformComp._attach = false;
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Interaction"))
        {
            if (_isInTrigger == false || _isOpertable == false)
            {
                return;
            }
            _isActive = !_isActive;
            _movePlatformComp._moveDirection = _isActive ? 1: -1;
            StartCoroutine("MonitorMovement");
            Debug.Log("Triggereed");
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Player"))
        {
            return;
        }
        _isInTrigger = true;
    }

    private IEnumerator MonitorMovement()
    {
        _isOpertable = false;
        int prevDir = _movePlatformComp._moveDirection;
        Debug.Log("start dir: " + _movePlatformComp._moveDirection);
        while (_movePlatformComp._moveDirection != 0)
        {
            yield return null;
        }
        FireEvent();
        if (!_isFlipFlop)
        {
            _movePlatformComp._moveDirection = prevDir * - 1;
            Debug.Log("dir: " + _movePlatformComp._moveDirection );
            while (_movePlatformComp._moveDirection != 0)
            {

                yield return null;
            }
            _isActive = !_isActive;
            FireEvent();
        }
        _isOpertable = true;
        yield return null;
    }

    private void FireEvent()
    {
        if (_isActive)
        {
            Debug.Log("Active event");
        }
        else
        {
            Debug.Log("InActive event");
        }
        if (_isActive && _activeEventObjects.Length != 0)
        {
            foreach (var eventObj in _activeEventObjects)
            {
                if (eventObj.layer != LayerMask.NameToLayer("Default"))
                {
                    if (gameObject.layer == LayerMask.NameToLayer("WorldA")
                        || gameObject.layer == LayerMask.NameToLayer("WorldBInPortal"))
                    {
                        eventObj.layer = LayerMask.NameToLayer("WorldA");
                    }
                    else
                    {
                        eventObj.layer = LayerMask.NameToLayer("WorldB");
                    }
                }

                eventObj.SetActive(true);
            }
            
            
        }
        else if ( !_isActive && _inactiveEventObjects.Length != 0) 
        {
            foreach (var eventObj in _inactiveEventObjects)
            {
                eventObj.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.tag.Equals("Player"))
        {
            return;
        }
        _isInTrigger = false;
    }

}
