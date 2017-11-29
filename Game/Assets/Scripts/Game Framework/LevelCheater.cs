using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCheater : MonoBehaviour {
    KeyCode[] _numberKeyCodes;
    // Use this for initialization
    void Start()
    {
        _numberKeyCodes = new KeyCode[10];
        for (int i = 0; i < _numberKeyCodes.Length; ++i)
        {
            _numberKeyCodes[i] = KeyCode.Alpha0 + i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.C))
        {
            return;
        }

        for (int i = 0; i < _numberKeyCodes.Length; ++i)
        {
            if (Input.GetKeyDown(_numberKeyCodes[i]))
            {
                if (gameObject.GetComponent<LevelLoading>().IsLevelExist("Level" + i))
                {
                    GameObject.Find("Canvas").SetActive(false);
                    gameObject.GetComponent<LevelLoading>().StartLoadLevel("Level" + i);
                    break;
                }
            }
        }
    }
}