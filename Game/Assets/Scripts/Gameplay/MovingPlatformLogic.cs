using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformLogic : MonoBehaviour {
    public GameObject[] _anchorPoints;
    public float[] _moveTime;
    public AnimationCurve[] _moveCurves;
    private float[] _accuMoveTime;
    
    // 1: forward, -1: backward, 0: not moving
    public int _moveDirection = 1;
    public int _currAhchorIndex = 0;
    private int _targetAhchorIndex = 0;
    public bool _canReverse = true;
    private float _currTime = 0f;
    public bool _attach = true;
	// Use this for initialization
	void Start () {
        _accuMoveTime = new float[ _moveTime.Length + 1 ];
        _accuMoveTime[0] = 0f;
        for (int i = 1; i < _accuMoveTime.Length; ++i)
        {
            _accuMoveTime[i] = _accuMoveTime[i - 1] + _moveTime[i-1];
        }
        gameObject.transform.position = _anchorPoints[ _currAhchorIndex ].transform.position;
        gameObject.transform.rotation = _anchorPoints[_currAhchorIndex].transform.rotation;

        _currTime = _accuMoveTime[_currAhchorIndex];

        UpdateTargetAnchorPoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_moveDirection == 0)
        {
            return;
        }
        // Update curr and target index
        _currTime += Time.deltaTime * _moveDirection;
        
        if (_moveDirection == 1)
        {
            if (_currTime >= _accuMoveTime[_targetAhchorIndex])
            {
                _currAhchorIndex = _targetAhchorIndex;
                UpdateTargetAnchorPoint();
                if (_moveDirection == -1) // has reversed
                {
                    _currTime = _accuMoveTime[_accuMoveTime.Length - 1];
                }
            }
        }
        else
        {
            if (_currTime <= _accuMoveTime[_targetAhchorIndex])
            {
                _currAhchorIndex = _targetAhchorIndex;
                UpdateTargetAnchorPoint();
                if (_moveDirection == 1) // has reversed
                {
                    _currTime = 0f;
                }
            }
        }
        float lerpFactor = 0f;
        // This should be a rest position, no need to update transform
        if (_anchorPoints[_currAhchorIndex].GetInstanceID() == _anchorPoints[_targetAhchorIndex].GetInstanceID())
        {
            return;
        }
        // Debug.Log("curr " + _currAhchorIndex + " target " + _targetAhchorIndex);
        // Debug.Log(_currTime);
        if (_moveDirection == 1)
        {
            lerpFactor = (_currTime - _accuMoveTime[_currAhchorIndex]) / (_accuMoveTime[_targetAhchorIndex] - _accuMoveTime[_currAhchorIndex]);
            lerpFactor = _moveCurves[_currAhchorIndex].Evaluate(lerpFactor);
            gameObject.transform.position = Vector3.Lerp(
                _anchorPoints[_currAhchorIndex].transform.position,
                _anchorPoints[_targetAhchorIndex].transform.position,
                lerpFactor
                );

            gameObject.transform.rotation = Quaternion.Lerp(
                _anchorPoints[_currAhchorIndex].transform.rotation,
                _anchorPoints[_targetAhchorIndex].transform.rotation,
                lerpFactor);
        }
        else
        {
            lerpFactor = (_currTime - _accuMoveTime[_targetAhchorIndex]) / (_accuMoveTime[_currAhchorIndex] - _accuMoveTime[_targetAhchorIndex]);
            lerpFactor = _moveCurves[_targetAhchorIndex].Evaluate(lerpFactor);

            gameObject.transform.position = Vector3.Lerp(
                _anchorPoints[_targetAhchorIndex].transform.position,
                _anchorPoints[_currAhchorIndex].transform.position,
                lerpFactor
                );

            gameObject.transform.rotation = Quaternion.Lerp(
                _anchorPoints[_targetAhchorIndex].transform.rotation,
                _anchorPoints[_currAhchorIndex].transform.rotation,
                lerpFactor);
        }

        
    }

    void UpdateTargetAnchorPoint()
    {
        if (_moveDirection == 1)
        {
            if (_currAhchorIndex < _anchorPoints.Length - 1)
            {
                _targetAhchorIndex = _currAhchorIndex + 1;
            }
            else if (_canReverse)
            {
                _targetAhchorIndex = _currAhchorIndex - 1;
                _moveDirection = -1;
            }
            else
            {
                _moveDirection = 0;
            }
        }
        else if (_moveDirection == -1)
        {
            if (_currAhchorIndex > 0)
            {
                _targetAhchorIndex = _currAhchorIndex - 1;
                
            }
            else if (_canReverse) // it's in zero index
            {
                _targetAhchorIndex = 1;
                _moveDirection = 1;
            }
            else
            {
                _moveDirection = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlatformFollow>() == null || !_attach)
        {
            return;
        }
        //Debug.Log("Enger!");
        // other.transform.parent = gameObject.transform;
        other.gameObject.GetComponent<PlatformFollow>()._platform = gameObject.transform;
        // other.transform.parent = gameObject.transform.parent;
        //if (other.gameObject.GetComponent<CharacterController>().isGrounded)
        //{
        //    // other.gameObject.GetComponent<CharacterController>().
        //    //Debug.Log("Attach!");
        //    //other.transform.parent = gameObject.transform.parent;
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlatformFollow>() == null || !_attach)
        {
            return;
        }

       // Debug.Log("Leave!");
        other.gameObject.GetComponent<PlatformFollow>()._platform = null;
        //other.transform.parent = GameObject.Find("FrameworkRoot").transform;
    }
    
}
