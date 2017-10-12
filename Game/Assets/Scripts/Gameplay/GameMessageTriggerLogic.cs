using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMessageTriggerLogic : MonoBehaviour {
    public bool _isNotification = true;
    public bool _triggerOnce = true;
    public float _triggerCoolDown = 0f;
    public string _message;
    public float _time;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Player")) {
            return;
        }
        if (_isNotification) {
            GameObject.Find("GameManager").GetComponent<GameManager>().DisplayNotifyMessage(_message, _time);
        }
        else {
            GameObject.Find("GameManager").GetComponent<GameManager>().DisplayHintMessage(_message, _time);
        }
        
        if (_triggerOnce) {
            gameObject.SetActive(false);
        }
        if (!_triggerOnce && _triggerCoolDown > 0f) {
            gameObject.GetComponent<Collider>().enabled = false;
            StartCoroutine("ActiveAfterSeconds", _triggerCoolDown);
        }
    }

    private IEnumerator ActiveAfterSeconds(float time) {
        yield return new WaitForSeconds(time);
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
