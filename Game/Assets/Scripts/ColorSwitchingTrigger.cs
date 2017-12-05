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

    public GameObject[] _redEventObjects;
    public GameObject[] _greenEventObjects;
    public GameObject[] _BlueEventObjects;
    // Use this for initialization
    void Start () {
        _r = false;
        _g = false;
        _b = false;
        _ifElevating = false;
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Equals("Ball_Red") && !_r)
        {
            _r = true;
            RedEvent();
            
        }
        else if (other.transform.name.Equals("Ball_Green") && _r && !_b)
        {
            GreenEvent();
            _g = true;
        }
        else if (other.transform.name.Equals("Ball_Blue") && _r && _g && !_b)
        {
            BlueEvent();
        }
    }

    private void RedEvent()
    {
        _ifElevating = true;
        Vector3 temp = _redRing.transform.position;
        GameObject.Destroy(_redRing);
        _greenRing.transform.position = temp;
        foreach (var go in _redEventObjects)
        {
            if (go != null)
            {
                go.SetActive(true);
            }
        }

    }
    private void GreenEvent()
    {
        Vector3 temp = _greenRing.transform.position;
        GameObject.Destroy(_greenRing);
        _blueRing.transform.position = temp;
        foreach (var go in _greenEventObjects)
        {
            if (go != null)
            {
                go.SetActive(true);
            }
        }
    }
    private void BlueEvent()
    {
        Vector3 temp = _lockedBox.transform.position;
        GameObject.Destroy(_lockedBox);
        _unlockedBox.transform.position = temp;

        foreach (var go in _BlueEventObjects)
        {
            if (go != null)
            {
                go.SetActive(true);
            }
        }
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
