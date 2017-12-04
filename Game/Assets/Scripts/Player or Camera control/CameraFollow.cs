using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public float _followingSpeed;
    public GameObject _followingRoot;
    public AnimationCurve _xAxisShakeCurve;
    public AnimationCurve _yAxisShakeCurve;
    public float _maxShakeXAngle = 30f;
    public float _maxShakeYAngle = 15f;
    public float _shakeXScalar = 0.5f;
    public float _shakeYScalar = 0.1f;

    private float _currAhpla = 0f;
    private float _shakePeriod = 0.6f;
    private float _shakeXRange = 10f;
    private float _shakeYRange = 5f;

    private float _previousMaxY;

    private bool _isShaking = false;
    private bool _previouslyGrounded = false;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController _fpsComp;
    // Use this for initialization
    void Start () {
        _fpsComp = GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();

    }

    private void Update()
    {
        if (_fpsComp.GetIsGrounded())
        {
            if (!_previouslyGrounded) // Player just hit the ground, do hit ground animation 
            {
                float fallDistance = _previousMaxY - _fpsComp.transform.position.y;
                _shakeXRange = Mathf.Min(Mathf.Abs(fallDistance) * _shakeXScalar, _maxShakeXAngle);
                _shakeYRange = Mathf.Min(Mathf.Abs(fallDistance) * _shakeYScalar, _maxShakeYAngle);
                // Debug.Log("X: " + _shakeXRange + " Y: " + _shakeYRange);
                _isShaking = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            _isShaking = true;
        }
        if (!_isShaking)
        {
            gameObject.transform.position = Vector3.Lerp(
            gameObject.transform.position, _followingRoot.transform.position, _followingSpeed * Time.deltaTime);

            gameObject.transform.rotation = Quaternion.Lerp(
                gameObject.transform.rotation, _followingRoot.transform.rotation, _followingSpeed * Time.deltaTime);
        }
        else
        {
            gameObject.transform.position = _followingRoot.transform.position;
            gameObject.transform.rotation = _followingRoot.transform.rotation;

            // start applying shaking
            _currAhpla += Time.deltaTime / _shakePeriod;
            float rotXAngle = _xAxisShakeCurve.Evaluate(_currAhpla) * _shakeXRange;
            float rotYAngle = _yAxisShakeCurve.Evaluate(_currAhpla) * _shakeYRange;
            gameObject.transform.rotation *= Quaternion.Euler(-rotXAngle, rotYAngle, 0f);

            if (_currAhpla > 1f)
            {
                _currAhpla = 0f;
                _isShaking = false;
            }
        }

        if (_fpsComp.GetIsGrounded())
        {
            _previousMaxY = _fpsComp.transform.position.y;
        }
        else
        {
            _previousMaxY = Mathf.Max(_previousMaxY, _fpsComp.transform.position.y);
        }
        _previouslyGrounded = _fpsComp.GetIsGrounded();
        
    }

    public void PerformDamageShake()
    {
        _shakeXRange = 10f;
        _shakeYRange = 5f;
        _isShaking = true;
    }

    // Update is called once per frame
    void FixedUpdate () {
        //if (!_isShaking)
        //{
        //    gameObject.transform.position = Vector3.Lerp(
        //    gameObject.transform.position, _followingRoot.transform.position, _followingSpeed * Time.deltaTime);

        //    gameObject.transform.rotation = Quaternion.Lerp(
        //        gameObject.transform.rotation, _followingRoot.transform.rotation, _followingSpeed * Time.deltaTime);
        //}
        
    }

    private void LateUpdate()
    {
    }

    public void SetupRoot(GameObject followingRoot) {
        _followingRoot = followingRoot;
    }
}
