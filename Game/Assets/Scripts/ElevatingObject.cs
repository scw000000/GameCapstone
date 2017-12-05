using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatingObject : MonoBehaviour {
    public AudioSource _audio;
    private bool ifPlayed;
    public float _goalHeight;
    private bool _ifElevating;
    private float _direction;
    private float _moveSpeed;
    public GameObject _myObject;
    public GameObject[] _eventObjects;
    // Use this for initialization
    void Start () {
        //_goalHeight = -0.5f;
        _ifElevating = false;
        _direction = 1.0f;
        _moveSpeed = 3.0f;
        ifPlayed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Equals("Ball"))
        {
            _ifElevating = true;
            foreach (var eventGO in _eventObjects)
            {
                if (eventGO != null)
                {
                    eventGO.SetActive(true);
                }
            }
        }
    }
    // Update is called once per frame
    void Update () {
        if (_ifElevating && _myObject.transform.position.y < -_goalHeight)
        {
            if (!ifPlayed)
            {
                ifPlayed = true;
                _audio.Play();
            }
            _myObject.transform.Translate(Vector3.up * Time.deltaTime * _moveSpeed * _direction, Space.World);
        }
	}
}
