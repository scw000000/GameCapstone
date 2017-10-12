using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingControl : MonoBehaviour {
    public Vector3 _floatPeriod;
    private Vector3 _currTime;
    public Vector3 _maxShift;
    public Vector3 _minShift;
    public AnimationCurve _xAnimCurve;
    public AnimationCurve _yAnimCurve;
    public AnimationCurve _zAnimCurve;
    public bool _useLocalPos = true;
    private Vector3 _defualtPos;
    // Use this for initialization
    void Start() {
        _currTime = Vector3.zero;
        if (_useLocalPos)
        {
            _defualtPos = gameObject.transform.localPosition;
        }
        else {
            _defualtPos = gameObject.transform.position;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (_floatPeriod.x > 0) {
            _currTime.x = Mathf.Repeat(_currTime.x + Time.deltaTime / _floatPeriod.x, 1f);
        }
        if (_floatPeriod.y > 0) {
            _currTime.y = Mathf.Repeat(_currTime.y + Time.deltaTime / _floatPeriod.y, 1f);
        }
        if (_floatPeriod.z > 0) {
            _currTime.z = Mathf.Repeat(_currTime.z + Time.deltaTime / _floatPeriod.z, 1f);
        }

        float curAlphaX = _xAnimCurve.Evaluate(_currTime.x);
        float curAlphaY = _yAnimCurve.Evaluate(_currTime.y);
        float curAlphaZ = _zAnimCurve.Evaluate(_currTime.z);

        float newLocalX = 0f;
        float newLocalY = 0f;
        float newLocalZ = 0f;
        if (curAlphaX > 0) {
            newLocalX = curAlphaX * _maxShift.x;
        }
        else {
            newLocalX = curAlphaX * -_minShift.x;
        }

        if (curAlphaY > 0)
        {
            newLocalY = curAlphaY * _maxShift.y;
        }
        else
        {
            newLocalY = curAlphaY * -_minShift.y;
        }


        if (curAlphaZ > 0)
        {
            newLocalZ = curAlphaZ * _maxShift.z;
        }
        else
        {
            newLocalZ = curAlphaZ * -_minShift.z;
        }


        
        if (_useLocalPos)
        {
            gameObject.transform.localPosition = _defualtPos + gameObject.transform.right * newLocalX + gameObject.transform.up * newLocalY + gameObject.transform.forward * newLocalZ;
        }
        else
        {
            gameObject.transform.position = _defualtPos + gameObject.transform.right * newLocalX + gameObject.transform.up * newLocalY + gameObject.transform.forward * newLocalZ;
        }
        //   new Vector3(newLocalX, newLocalY, newLocalZ);
    }
}
