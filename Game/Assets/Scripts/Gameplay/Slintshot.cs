using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slintshot : MonoBehaviour {

    private Vector3 _grabPos;

    private LineRenderer _left;
    private LineRenderer _right;
    private Transform _ball;
    private GameObject _player;
    private bool _canGrab;
    private bool _isGrabbing;
    private Vector3 _ballOriginalPos;
    private float _stringOriginalLength;
    public float _forceRate;
    private bool _isButtonPressed;
    // Use this for initialization
    void Start () {
        _left = transform.Find("LeftAttachPoint").transform.GetComponent<LineRenderer>();
        _isButtonPressed = false;
        _right = transform.Find("RightAttachPoint").transform.GetComponent<LineRenderer>();
        if (_forceRate <= 0.0f)
        {
            _forceRate = 12.0f;
        }
        _ball = transform.Find("Ball");
        _ballOriginalPos = _ball.position;
        _stringOriginalLength = Vector3.Magnitude(_left.transform.position - _right.transform.position);
        _canGrab = false;
        _isGrabbing = false;
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag.Equals("Player"))
        {
            _player = obj.gameObject;
            _canGrab = true;

        }
        Physics.IgnoreCollision(obj.transform.GetComponent<Collider>(), transform.GetComponent<Collider>(), true);
    }

    void OnTriggerStay(Collider obj)
    {
        if (obj.gameObject.tag.Equals("Player"))
        {
            _canGrab = true;

        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.gameObject.tag.Equals("Player"))
        {
            _canGrab = false;
            _isButtonPressed = false;
        }
    }

    private void ResetPhysics()
    {
        _canGrab = false;
        _ball.position = _ballOriginalPos;
        _ball.GetComponent<Rigidbody>().useGravity = false;
        _ball.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f,0.0f,0.0f), ForceMode.Impulse);

        _left.SetPosition(0, new Vector3(0.0f, 0.0f, 2.0f));
        _right.SetPosition(0, new Vector3(0.0f, 0.0f, -2.0f));
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonUp("Interaction"))
        {
            _isButtonPressed = !_isButtonPressed;
        }

        if (_canGrab)
        {
            if (_isButtonPressed)
            {
                _isGrabbing = true;
                //grabPos = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));

                Vector3 shift = _player.transform.forward * 3.0f;// new Vector3(-3.0f,0.0f,0.0f);
                _grabPos = _player.transform.position + shift;
                _ball.position = _grabPos;
                _left.SetPosition(0, new Vector3(_ball.localPosition.x, _ball.localPosition.y + 0.2f, _ball.localPosition.z + 2.0f));
                _right.SetPosition(0, new Vector3(_ball.localPosition.x, _ball.localPosition.y + 0.2f, _ball.localPosition.z - 2.0f));
            }
        }
        if (_isGrabbing && (!_canGrab || !_isButtonPressed))
        {
                _isGrabbing = false;
                _isButtonPressed = false;
                 Vector3 shift = _player.transform.forward * 3.0f;// new Vector3(-3.0f,0.0f,0.0f);
                _grabPos = _player.transform.position + shift;

                _ball.position = _grabPos;

                _left.SetPosition(0, new Vector3(_ball.localPosition.x, _ball.localPosition.y + 0.2f, _ball.localPosition.z + 2.0f));
                _right.SetPosition(0, new Vector3(_ball.localPosition.x, _ball.localPosition.y + 0.2f, _ball.localPosition.z - 2.0f));

                Vector3 Vec3L = new Vector3(_left.transform.position.x - _grabPos.x, _left.transform.position.y - _grabPos.y, _left.transform.position.z - _grabPos.z);
                Vector3 Vec3R = new Vector3(_right.transform.position.x - _grabPos.x, _right.transform.position.y - _grabPos.y, _right.transform.position.z - _grabPos.z);
                //Vector3 Vec3L = new Vector3(-2.0f - grabPos.x, 2.0f - grabPos.y, -grabPos.z);
                //Vector3 Vec3R = new Vector3(2.0f - grabPos.x, 2.0f - grabPos.y, -grabPos.z);
                float deltaX = Vec3L.magnitude + Vec3R.magnitude - _stringOriginalLength;
                Vector3 Dir = (Vec3L + Vec3R).normalized;

                _ball.GetComponent<Rigidbody>().useGravity = true;
                _ball.GetComponent<Rigidbody>().AddForce(Dir * deltaX * _forceRate, ForceMode.Impulse);

                _left.SetPosition(0, new Vector3(0.0f, 0.0f, 2.0f));
                _right.SetPosition(0, new Vector3(0.0f, 0.0f, -2.0f));

            //canGrab = false;
        }
    }
}
