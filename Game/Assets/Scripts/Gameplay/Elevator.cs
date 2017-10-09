using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    private float _direction;
    private float _moveSpeed;
    // Use this for initialization
    void Start()
    {
        _direction = 0.0f;
        _moveSpeed = 3.0f;
    }
    void GoingDown()
    {
        _direction = -1.0f;
    }
    public void TriggerExecute()
    {
        GoingDown();
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10.0f)
        {
            _direction = 0.0f;
        }

        transform.Translate(Vector3.up * Time.deltaTime * _moveSpeed * _direction, Space.World);
    }
}
