using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorAutoTrigger : MonoBehaviour {
    public GameObject _doorObj;
    public bool _triggerOpen;
    private int _laserOpenCheck;

	// Use this for initialization
	void Start () {
        _laserOpenCheck = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Player")) {
            _doorObj.GetComponent<DoorControl>()._isOpened = _triggerOpen;
			//SceneManager.LoadScene ("SlingshotPuzzle", LoadSceneMode.Additive);
        }
    }
		
    void TriggerEvent()
    {
        _laserOpenCheck += 1;
        if (_laserOpenCheck == 1)
        {
            _doorObj.GetComponent<DoorControl>()._isOpened = _triggerOpen;
            if (gameObject.GetComponent<CutSceneControl>() != null)
            {
                gameObject.GetComponent<CutSceneControl>().StartTimeLine();
            }
        }

    }
}
