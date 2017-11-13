using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipuleEventListener : MonoBehaviour {
    public GameObject _eventGO;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        var parentGO = transform.parent.gameObject;
        int childCount = parentGO.transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            if (!parentGO.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                return;
            }
        }

        _eventGO.SetActive(true);
    }
}
