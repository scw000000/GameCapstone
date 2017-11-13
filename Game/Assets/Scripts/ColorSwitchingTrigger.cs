using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitchingTrigger : MonoBehaviour {
    private bool _r, _g, _b,_ifElevating;
    public GameObject _stair01;
    public GameObject _stair02;
    public GameObject _redRing;
    public GameObject _greenRing;
    public GameObject _blueRing;
    public GameObject _lockedBox;
    public GameObject _unlockedBox;
	// Use this for initialization
	void Start () {
        _r = false;
        _g = false;
        _b = false;
        _ifElevating = false;
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Equals("Ball_Red"))
        {
            _r = true;
            Debug.Log("Red Ball");
            RedEvent();
            
        }
        else if (other.transform.name.Equals("Ball_Green") && _r)
        {
            Debug.Log("Greed Ball");
            GreenEvent();
            _g = true;
        }
        else if (other.transform.name.Equals("Ball_Blue") && _r && _g)
        {
            Debug.Log("Greed Ball");
            BlueEvent();
        }
    }

    private void RedEvent()
    {
        _ifElevating = true;
        Vector3 temp = _redRing.transform.position;
        GameObject.Destroy(_redRing);
        _greenRing.transform.position = temp;

    }
    private void GreenEvent()
    {
        Vector3 temp = _greenRing.transform.position;
        GameObject.Destroy(_greenRing);
        _blueRing.transform.position = temp;
    }
    private void BlueEvent()
    {
        Vector3 temp = _lockedBox.transform.position;
        GameObject.Destroy(_lockedBox);
        _unlockedBox.transform.position = temp;
    }
    // Update is called once per frame
    void Update () {
        if (_ifElevating && _stair01.transform.position.y < 2.5f)
        {
            _stair01.transform.Translate(Vector3.up * Time.deltaTime * 3.0f, Space.World);
            _stair02.transform.Translate(Vector3.up * Time.deltaTime * 3.0f, Space.World);
        }
        else
        {
            _ifElevating = false;
        }
    }
}
