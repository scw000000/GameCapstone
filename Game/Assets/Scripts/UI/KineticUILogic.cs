using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticUILogic : MonoBehaviour {
    public AnimationCurve _jumpAnimCurve;
    public AnimationCurve _hitGroundAnimCurve;
    private AnimationCurve _currAnimCurve;

    private Vector2 _originalPos;

    public float _jumpAnimMaxShift = 10f;
    public float _hitGroundAnimMaxShift = 15f;

    public float _jumpAnimPeriod = 1f;
    public float _hitGoundAnimPeriod = 0.5f;

    private float _currAlpha;
    private float _currAnimPeriod;
    private float _currMaxYShift;

    public float _jumpAnimScalar = 1f;
    public float _hitGroundAnimScalar = 1f;
    private float _prevVelocityY = 0f;
    private float _previousMaxY;

    private bool _previouslyGrounded = false;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController _fpsComp;
    // Use this for initialization
    void Start () {
        _fpsComp = GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        _originalPos = gameObject.GetComponent<RectTransform>().anchoredPosition;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(_fpsComp.m_PreviouslyGrounded);
       // Debug.Log(_fpsComp.GetIsGrounded());

        if (_fpsComp.GetIsGrounded())
        {
            if (!_previouslyGrounded) // Player just hit the ground, do hit ground animation 
            {
                _currAlpha = 0f;
                _currAnimPeriod = _hitGoundAnimPeriod;
                _currAnimCurve = _hitGroundAnimCurve;
                _currMaxYShift = Mathf.Min(Mathf.Abs(_previousMaxY - _fpsComp.transform.position.y) * _hitGroundAnimScalar, _hitGroundAnimMaxShift);

              // Debug.Log("hit Shift = " + _currMaxYShift + " _prevY " + _previousMaxY + " posY " + _fpsComp.transform.position.y + " shift " + (_previousMaxY - _fpsComp.transform.position.y));
              //   Debug.Log("hit !");
            }
        }

        // player starts jumping
        if (_prevVelocityY <= 0f && _fpsComp.m_MoveDir.y > 0f)
        {
            _currAlpha = 0f;
            _currAnimPeriod = _jumpAnimPeriod;
            _currAnimCurve = _jumpAnimCurve;
            _currMaxYShift = Mathf.Min(Mathf.Abs( (_fpsComp.m_MoveDir.y - _prevVelocityY) ) * _jumpAnimScalar, _jumpAnimMaxShift );
            // Debug.Log("jump Shift = " + _currMaxYShift);
            // Debug.Log("jump!");
        }

        _prevVelocityY = _fpsComp.m_MoveDir.y;
        if ( _fpsComp.GetIsGrounded())
        {
            _previousMaxY = _fpsComp.transform.position.y;
        }
        else
        {
            _previousMaxY = Mathf.Max(_previousMaxY, _fpsComp.transform.position.y);
        }
        _previouslyGrounded = _fpsComp.GetIsGrounded();

        // Debug.Log(_fpsComp.m_MoveDir.y);
        if (_currAnimCurve == null)
        {
            return;
        }

        _currAlpha += Time.deltaTime / _currAnimPeriod;
        float lerpVal = _currAnimCurve.Evaluate(_currAlpha);
        gameObject.GetComponent<RectTransform>().anchoredPosition = _originalPos + 
            new Vector2(0, Mathf.Lerp( 0f, Mathf.Sign(lerpVal) * _currMaxYShift, Mathf.Abs(lerpVal) ) );

        if (_currAlpha > 1f)
        {
            _currAlpha = 0f;
            gameObject.GetComponent<RectTransform>().anchoredPosition = _originalPos;
            _currAnimCurve = null;
        }
    }
}
