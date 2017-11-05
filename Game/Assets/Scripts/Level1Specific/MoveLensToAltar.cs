using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLensToAltar : MonoBehaviour {
    public GameObject _lensIdlePoint;
    private GameObject _lens;
    public GameObject _lensCopy;
    private bool _insideTrigger = false;
    private GameObject _player;
    public bool _hasFinished = false;
	// Use this for initialization
	void Start () {
        _lens = GameObject.Find("Holder");
        _lens.GetComponent<MeshRenderer>().enabled = false;
        _lens.GetComponent<OutlineControl>()._alwaysActive = false;
        _lens.GetComponent<OutlineControl>().SetEnableOutline(false);
        _player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(_player.GetComponent<WorldSwitch>().SetupLensMaterial(_lensCopy) );
        
    }
	
	// Update is called once per frame
	void Update () {
        if (( _insideTrigger && Input.GetButtonDown("Interaction") )) {
            Vector3 playerToLensCopy = _lensCopy.transform.position - _player.transform.position;
            playerToLensCopy.Normalize();
            if (Vector3.Dot(playerToLensCopy, _player.transform.forward) >= 0.7f) {
                AcquireLens();
                var gameManagerComp = GameObject.Find("GameManager").GetComponent<GameManager>();
                gameManagerComp.DisplayHintMessage("Look through the lens to investigate anomaly", 9);
                gameManagerComp.DisplayNotifyMessage("Press F to switch world", 4);
                gameManagerComp.DisplayHintMessage("Find a way out of the cave", 4);
            }
        }
        if (_hasFinished)
        {
            AcquireLens();
            Destroy(gameObject);
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

        gameObject.GetComponent<GameMessageTriggerLogic>().TerminateGameMessage(true);


        _insideTrigger = false;
    }

    public void AcquireLens()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<GameMessageTriggerLogic>().TerminateGameMessage(true);

        _lens.GetComponent<MeshRenderer>().enabled = true;
        // StartCoroutine(_player.GetComponent<WorldSwitch>().SetupLensMaterial(_lens));

        _lens.GetComponent<OutlineControl>()._alwaysActive = true;
        _lens.GetComponent<OutlineControl>().SetEnableOutline(true);
        _lensCopy.SetActive(false);
        gameObject.transform.Find("MagicRing").gameObject.SetActive(false);
    }
}
