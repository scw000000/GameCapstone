using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointCheater : MonoBehaviour {
    KeyCode [] _numberKeyCodes;
    // Use this for initialization
    void Start () {
        _numberKeyCodes = new KeyCode[10];
        for (int i = 0; i < _numberKeyCodes.Length; ++i)
        {
            _numberKeyCodes[i] = KeyCode.Alpha0 + i;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!Input.GetKey(KeyCode.C))
        {
            return;
        }

        for (int i = 0; i < _numberKeyCodes.Length; ++i)
        {
            if (Input.GetKeyDown(_numberKeyCodes[i]))
            {
                gameObject.GetComponent<GameManager>().ApplyProgress(i - 1, false);
                break;
            }   
        }
    }
}
