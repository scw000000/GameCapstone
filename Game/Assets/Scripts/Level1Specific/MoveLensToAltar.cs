using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLensToAltar : MonoBehaviour {
    public GameObject _lensIdlePoint;
    private GameObject _lens;
    public GameObject _lensCopy;
    private bool _insideTrigger = false;
    private GameObject _player;
    private GameObject _leftEyeIcon;
    private GameObject _rightEyeIcon;

    public bool _hasFinished = false;
	// Use this for initialization
	void Start () {
        _lens = GameObject.Find("Holder");
        _lens.GetComponent<MeshRenderer>().enabled = false;
        _lens.GetComponent<OutlineControl>()._alwaysActive = false;
        _lens.GetComponent<OutlineControl>().SetEnableOutline(false);
        _player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(_player.GetComponent<WorldSwitch>().SetupLensMaterial(_lensCopy) );
        _leftEyeIcon = GameObject.Find("LeftEyeIcon");
        _leftEyeIcon.GetComponent<UnityEngine.UI.Image>().enabled = false; //.SetActive(false);
        _rightEyeIcon = GameObject.Find("RightEyeIcon");
        _rightEyeIcon.GetComponent<UnityEngine.UI.Image>().enabled = false; //.SetActive(false);
        Debug.Log("Disabled");
    }
	
	// Update is called once per frame
	void Update () {
        _player.GetComponent<ShootingLogic>().enabled = false;
        _player.GetComponent<WorldSwitch>().enabled = false;

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
        Debug.Log("Acquired");
        _player.GetComponent<ShootingLogic>().enabled = true;
        _player.GetComponent<WorldSwitch>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<GameMessageTriggerLogic>().TerminateGameMessage(true);

        _lens.GetComponent<MeshRenderer>().enabled = true;
        // StartCoroutine(_player.GetComponent<WorldSwitch>().SetupLensMaterial(_lens));

        _lens.GetComponent<OutlineControl>()._alwaysActive = true;
        _lens.GetComponent<OutlineControl>().SetEnableOutline(true);
        _lensCopy.SetActive(false);

        _leftEyeIcon.GetComponent<UnityEngine.UI.Image>().enabled = true;// .SetActive(true);
        _rightEyeIcon.GetComponent<UnityEngine.UI.Image>().enabled = true;// .SetActive(true);
        // gameObject.transform.Find("MagicRing").gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
