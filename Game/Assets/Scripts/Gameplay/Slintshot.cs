using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slintshot : MonoBehaviour {

    private Vector3 grabPos;

    private LineRenderer left;
    private LineRenderer right;
    private Transform ball;
    private GameObject player;
    private bool canGrab;
    private bool _isGrabbing;
    private Vector3 ballOriginalPos;
    private float stringOriginalLength;
   
	// Use this for initialization
	void Start () {
        left = transform.Find("LeftAttachPoint").transform.GetComponent<LineRenderer>();

        right = transform.Find("RightAttachPoint").transform.GetComponent<LineRenderer>();

        ball = transform.Find("Ball");
        ballOriginalPos = ball.position;
        stringOriginalLength = Vector3.Magnitude(left.transform.position - right.transform.position);
        canGrab = false;
        _isGrabbing = false;
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag.Equals("Player"))
        {
            player = obj.gameObject;
            canGrab = true;

        }
        Physics.IgnoreCollision(obj.transform.GetComponent<Collider>(), transform.GetComponent<Collider>(), true);
    }

    void OnTriggerStay(Collider obj)
    {
        if (obj.gameObject.tag.Equals("Player"))
        {
            canGrab = true;

        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.gameObject.tag.Equals("Player"))
        {
            canGrab = false;
        }
    }

    private void ResetPhysics()
    {
        canGrab = false;
        ball.position = ballOriginalPos;
        ball.GetComponent<Rigidbody>().useGravity = false;
        ball.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f,0.0f,0.0f), ForceMode.Impulse);

        left.SetPosition(0, new Vector3(0.0f, 0.0f, 2.0f));
        right.SetPosition(0, new Vector3(0.0f, 0.0f, -2.0f));
    }

    // Update is called once per frame
    void Update () {
        /*ball = transform.Find("Ball");
        if (ball == null)
        {
            Debug.Log("ball not found");
        }
        left.SetPosition(0, new Vector3(ball.localPosition.x, ball.localPosition.y+0.2f, ball.localPosition.z+2.0f));
        right.SetPosition(0, new Vector3(ball.localPosition.x, ball.localPosition.y+0.2f, ball.localPosition.z-2.0f));
        */
        if (canGrab)
        {
            if (Input.GetButton("Interaction"))
            {
                _isGrabbing = true;
                //grabPos = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
                Vector3 shift = new Vector3(-3.0f,0.0f,0.0f);
                grabPos = player.transform.position + shift; 
                ball.position = grabPos;
                left.SetPosition(0, new Vector3(ball.localPosition.x, ball.localPosition.y + 0.2f, ball.localPosition.z + 2.0f));
                right.SetPosition(0, new Vector3(ball.localPosition.x, ball.localPosition.y + 0.2f, ball.localPosition.z - 2.0f));
            }
        }
        if (_isGrabbing && (!canGrab || !Input.GetButton("Interaction")))
        {
            _isGrabbing = false;
                Vector3 shift = new Vector3(-3.0f, 0.0f, 0.0f);
                grabPos = player.transform.position + shift;

                ball.position = grabPos;

                left.SetPosition(0, new Vector3(ball.localPosition.x, ball.localPosition.y + 0.2f, ball.localPosition.z + 2.0f));
                right.SetPosition(0, new Vector3(ball.localPosition.x, ball.localPosition.y + 0.2f, ball.localPosition.z - 2.0f));

                Vector3 Vec3L = new Vector3(left.transform.position.x - grabPos.x, left.transform.position.y - grabPos.y, left.transform.position.z - grabPos.z);
                Vector3 Vec3R = new Vector3(right.transform.position.x - grabPos.x, right.transform.position.y - grabPos.y, right.transform.position.z - grabPos.z);
                //Vector3 Vec3L = new Vector3(-2.0f - grabPos.x, 2.0f - grabPos.y, -grabPos.z);
                //Vector3 Vec3R = new Vector3(2.0f - grabPos.x, 2.0f - grabPos.y, -grabPos.z);
                float deltaX = Vec3L.magnitude + Vec3R.magnitude - stringOriginalLength;
                Vector3 Dir = (Vec3L + Vec3R).normalized;

                ball.GetComponent<Rigidbody>().useGravity = true;
                ball.GetComponent<Rigidbody>().AddForce(Dir * deltaX * 12.0f, ForceMode.Impulse);

                left.SetPosition(0, new Vector3(0.0f, 0.0f, 2.0f));
                right.SetPosition(0, new Vector3(0.0f, 0.0f, -2.0f));

            //canGrab = false;
        }
    }
}
