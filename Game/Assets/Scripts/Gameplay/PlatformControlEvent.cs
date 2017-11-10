using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControlEvent : MonoBehaviour {
    public GameObject _controlledPlatform;
    private int _prevDir;
    public bool _isPauseEvent;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        Debug.Log("platfomr control");
        if (_controlledPlatform == null || _controlledPlatform.GetComponent<MovingPlatformLogic>() == null)
        {
            return;
        }
        var movingPlatformComp = _controlledPlatform.GetComponent<MovingPlatformLogic>();
        if (_isPauseEvent)
        {
            if (movingPlatformComp._moveDirection == 0)
            {
                movingPlatformComp._moveDirection = _prevDir;
            }
            else
            {
                _prevDir = movingPlatformComp._moveDirection;
                movingPlatformComp._moveDirection = 0;
            }
        }
        else
        {
            movingPlatformComp._moveDirection *= -1;
        }

        gameObject.SetActive(false);
    }
}
