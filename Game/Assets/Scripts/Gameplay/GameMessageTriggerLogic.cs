using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMessageTriggerLogic : MonoBehaviour
{
    public bool _isNotification = true;
    public bool _isTrigger = true;
    public bool _triggerOnce = true;
    public float _triggerCoolDown = 0f;
    public string _message;
    private bool _isActivatable = true;
    public float _time;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Player"))
        {
            return;
        }
        if (!_isTrigger)
        {
            return;
        }
        ActivateGameMessage();
    }

    private IEnumerator ActiveAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        _isActivatable = true;
    }

    public void ActivateGameMessage()
    {
        if (!_isActivatable) {
            return;
        }
        if (_isNotification)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().DisplayNotifyMessage(_message, _time);
        }
        else
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().DisplayHintMessage(_message, _time);
        }

        _isActivatable = false;

        if (!_triggerOnce)
        {
            if (_triggerCoolDown <= 0f)
            {
                _isActivatable = true;
            }
            else {
                StartCoroutine("ActiveAfterSeconds", _triggerCoolDown);
            }
        }
    }

    public void TerminateGameMessage()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().TerminateGameMessage();
    }

}

