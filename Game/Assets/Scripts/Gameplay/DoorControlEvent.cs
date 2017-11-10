using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControlEvent : MonoBehaviour {
    public bool _setOpen = true;
    public bool _isFlipFlop = false;
    public GameObject _doorGO;
    private DoorControl _doorComp;
	// Use this for initialization
	void Start () {
    }

    // This function may be called before start
    private void OnEnable()
    {
        _doorComp = _doorGO.GetComponent<DoorControl>();
        Debug.Log("Door event");
        if (_doorComp != null)
        {
            _doorComp._isOpened = _setOpen;
            if (_isFlipFlop)
            {
                _setOpen = !_setOpen;
            }
        }

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
