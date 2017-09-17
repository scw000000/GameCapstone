using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongPlayerControl : MonoBehaviour {
    public float MovingSpeed = 15.0f;
    public const float RotatingSpeed = 500.0f;
    // private GameObject CameraRoot;
    private float MinX;
    private float MaxX;
    // Use this for initialization
    void Start()
    {
        var leftMostPos = GameObject.Find("P1LeftMostPos");
        MinX = leftMostPos.transform.position.x + 0.5f + gameObject.transform.localScale.x * 0.5f;
        var rightMostPos = GameObject.Find("P1RightMostPos");
        MaxX = rightMostPos.transform.position.x - 0.5f - gameObject.transform.localScale.x * 0.5f;
        // CameraRoot = gameObject.transform.Find("CameraRoot").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(new Vector3(-1.0f * Time.deltaTime * MovingSpeed, 0.0f, 0.0f));
            
        }

        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(new Vector3(Time.deltaTime * MovingSpeed, 0.0f, 0.0f));
        }

        // Limit the position so that it won't clip through wall
        float fixedX = Mathf.Clamp(gameObject.transform.position.x, MinX, MaxX);
        gameObject.transform.position = new Vector3(fixedX
            , gameObject.transform.position.y
            , gameObject.transform.position.z);
        //if (Input.GetKey(KeyCode.W))
        //{
        //    gameObject.transform.Translate(new Vector3(0.0f, 0.0f, Time.deltaTime * MovingSpeed));
        //}

        //if (Input.GetKey(KeyCode.S))
        //{
        //    gameObject.transform.Translate(new Vector3(0.0f, 0.0f, -1.0f * Time.deltaTime * MovingSpeed));
        //}

        //if (Input.GetKey(KeyCode.Mouse1))
        //{
        //    gameObject.transform.RotateAround(gameObject.transform.position, new Vector3(0.0f, 1.0f, 0.0f), Input.GetAxis("Mouse X") * Time.deltaTime * RotatingSpeed);


        //    CameraRoot.transform.RotateAround(CameraRoot.transform.position, gameObject.transform.right, -1.0f * Input.GetAxis("Mouse Y") * Time.deltaTime * RotatingSpeed);
        //}

    }
}
