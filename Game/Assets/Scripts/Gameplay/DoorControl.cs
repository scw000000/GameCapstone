using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour {
    public GameObject[] _doorComponents;
    public GameObject[] _doorOpenRoots;
    public GameObject[] _doorCloseRoots;
    public AnimationCurve[] _doorSpeedCurves;
    public bool _isOpened = false;
    public float _openTime = 2f;
    public float _closeTime = 2f;
    private float _doorOpenLerp;
    // Use this for initialization
    void Start()
    {
        ResetDoors();
    }
	// Update is called once per frame
	void Update () {
        // don't waste time to update the doors
        if ((_isOpened && _doorOpenLerp >= 1f) || (!_isOpened && _doorOpenLerp <= 0f)) {
            return;
        }

        if (_isOpened)
        {
            _doorOpenLerp += Time.deltaTime / _openTime;
        }
        else
        {
            _doorOpenLerp -= Time.deltaTime / _closeTime;
        }
        _doorOpenLerp = Mathf.Clamp01(_doorOpenLerp);

        for (int i = 0; i < _doorComponents.Length; ++i)
        {
            
            GameObject openRoot;
            if (i < _doorOpenRoots.Length)
            {
                openRoot = _doorOpenRoots[i];
            }
            else
            {
                openRoot = _doorOpenRoots[_doorOpenRoots.Length - 1];
            }
            GameObject closeRoot;
            if (i < _doorCloseRoots.Length)
            {
                closeRoot = _doorCloseRoots[i];
            }
            else
            {
                closeRoot = _doorCloseRoots[_doorCloseRoots.Length - 1];
            }
            AnimationCurve animcurve;
            if (i < _doorSpeedCurves.Length)
            {
                animcurve = _doorSpeedCurves[i];
            }
            else
            {
                animcurve = _doorSpeedCurves[_doorSpeedCurves.Length - 1];
            }
            _doorComponents[i].transform.position =
               Vector3.Lerp(closeRoot.transform.position, openRoot.transform.position, animcurve.Evaluate(_doorOpenLerp));
            _doorComponents[i].transform.rotation =
               Quaternion.Slerp(closeRoot.transform.rotation, openRoot.transform.rotation, animcurve.Evaluate(_doorOpenLerp));
        }

    }

    private void ResetDoors()
    {
        if (_isOpened)
        {
            _doorOpenLerp = 1f;
        }
        else {
            _doorOpenLerp = 0f;
        }
        for (int i = 0; i < _doorComponents.Length; ++i)
        {
            GameObject targetRoot;
            if (_isOpened)
            {
                if (i < _doorOpenRoots.Length)
                {
                    targetRoot = _doorOpenRoots[i];
                }
                else
                {
                    targetRoot = _doorOpenRoots[_doorOpenRoots.Length - 1];
                }
            }
            else
            {
                if (i < _doorCloseRoots.Length)
                {
                    targetRoot = _doorCloseRoots[i];
                }
                else
                {
                    targetRoot = _doorCloseRoots[_doorOpenRoots.Length - 1];
                }
            }
            _doorComponents[i].transform.position = targetRoot.transform.position;
            _doorComponents[i].transform.rotation = targetRoot.transform.rotation;
        }
    }
}
