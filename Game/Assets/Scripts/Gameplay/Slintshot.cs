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
    private Vector3 ballOriginalPos;
   
	// Use this for initialization
	void Start () {
        left = transform.Find("LeftAttachPoint").transform.GetComponent<LineRenderer>();

        right = transform.Find("RightAttachPoint").transform.GetComponent<LineRenderer>();

        ball = transform.Find("Ball");
        ballOriginalPos = ball.position;

        player = GameObject.FindGameObjectWithTag("Player");
        canGrab = false;
    }

    private void OnTriggerEnter(Collider obj)
    {
        Physics.IgnoreCollision(obj.transform.GetComponent<Collider>(), transform.GetComponent<Collider>(), true);
        canGrab = true;
    }

    void OnTriggerStay(Collider obj)
    {
        if (obj.gameObject.tag.Equals("Player"))
        {
            canGrab = true;

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
            if (Input.GetKey(KeyCode.E))
            {
                //grabPos = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
                Vector3 shift = new Vector3(-3.0f,0.0f,0.0f);
                grabPos = player.transform.position + shift; 
                ball.position = grabPos;
                left.SetPosition(0, new Vector3(ball.localPosition.x, ball.localPosition.y + 0.2f, ball.localPosition.z + 2.0f));
                right.SetPosition(0, new Vector3(ball.localPosition.x, ball.localPosition.y + 0.2f, ball.localPosition.z - 2.0f));
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                Vector3 shift = new Vector3(-3.0f, 0.0f, 0.0f);
                grabPos = player.transform.position + shift;

                ball.position = grabPos;

                left.SetPosition(0, new Vector3(ball.localPosition.x, ball.localPosition.y + 0.2f, ball.localPosition.z + 2.0f));
                right.SetPosition(0, new Vector3(ball.localPosition.x, ball.localPosition.y + 0.2f, ball.localPosition.z - 2.0f));
                
                Vector3 Vec3L = new Vector3(left.transform.position.x - grabPos.x, left.transform.position.y - grabPos.y, left.transform.position.z- grabPos.z);
                Vector3 Vec3R = new Vector3(right.transform.position.x- grabPos.x, right.transform.position.y - grabPos.y, right.transform.position.z- grabPos.z);
                //Vector3 Vec3L = new Vector3(-2.0f - grabPos.x, 2.0f - grabPos.y, -grabPos.z);
                //Vector3 Vec3R = new Vector3(2.0f - grabPos.x, 2.0f - grabPos.y, -grabPos.z);
                Vector3 Dir = (Vec3L + Vec3R).normalized;

                ball.GetComponent<Rigidbody>().useGravity = true;
                ball.GetComponent<Rigidbody>().AddForce(Dir * 20.0f, ForceMode.Impulse);

                left.SetPosition(0, new Vector3(0.0f, 0.0f, 2.0f));
                right.SetPosition(0, new Vector3(0.0f, 0.0f, -2.0f));
            }

            //canGrab = false;
        }
	}
}
