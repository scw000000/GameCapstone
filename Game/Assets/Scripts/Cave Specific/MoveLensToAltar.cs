using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLensToAltar : MonoBehaviour {
    public GameObject _lensIdlePoint;
    private GameObject _lens;
    public GameObject _lensCopy;
    private bool _insideTrigger = false;
    private GameObject _player;
	// Use this for initialization
	void Start () {
        _lens = GameObject.Find("Holder");
        _lens.GetComponent<MeshRenderer>().enabled = false;
        _lens.GetComponent<OutlineControl>()._alwaysActive = false;
        _player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(_player.GetComponent<WorldSwitch>().SetupLensMaterial(_lensCopy) );
        
    }
	
	// Update is called once per frame
	void Update () {
        if (_insideTrigger && Input.GetButtonDown("Interaction")) {
            Vector3 playerToLensCopy = _lensCopy.transform.position - _player.transform.position;
            playerToLensCopy.Normalize();
            if (Vector3.Dot(playerToLensCopy, _player.transform.forward) >= 0.7f) {
                gameObject.GetComponent<Collider>().enabled = false;
                gameObject.GetComponent<GameMessageTriggerLogic>().TerminateGameMessage();
                
                _lens.GetComponent<MeshRenderer>().enabled = true;
                // StartCoroutine(_player.GetComponent<WorldSwitch>().SetupLensMaterial(_lens));

                _lens.GetComponent<OutlineControl>()._alwaysActive = true;
                _lens.GetComponent<OutlineControl>().SetEnableOutline(true);
                _lensCopy.SetActive(false);

                GameObject.Find("GameManager").GetComponent<GameManager>().DisplayHintMessage("Look thorugh the lens to investigate anomaly", 9);
                GameObject.Find("GameManager").GetComponent<GameManager>().DisplayNotifyMessage("Press F to switch world", 4);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Player")) {
            return;
        }
        
        gameObject.GetComponent<GameMessageTriggerLogic>().ActivateGameMessage();

        _insideTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.tag.Equals("Player"))
        {
            return;
        }

        gameObject.GetComponent<GameMessageTriggerLogic>().TerminateGameMessage();


        _insideTrigger = false;
    }
}
