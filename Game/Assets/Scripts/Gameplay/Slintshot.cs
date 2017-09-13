using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slintshot : MonoBehaviour {

    private Vector3 grabPos;

    private LineRenderer left;
    private LineRenderer right;
    private GameObject ball;
    private GameObject player;
    private bool canGrab;
	// Use this for initialization
	void Start () {
        left = transform.Find("LeftAttachPoint").transform.GetComponent<LineRenderer>();
        right = transform.Find("RightAttachPoint").transform.GetComponent<LineRenderer>();
        ball = 
        player = GameObject.FindGameObjectWithTag("Player");
        canGrab = false;
    }

    private void OnTriggerEnter(Collision obj)
    {
        Physics.IgnoreCollision(obj.transform.GetComponent<Collider>(), transform.GetComponent<Collider>(), true);
    }

    void OnTriggerStay(Collision obj)
    {
        if (obj.gameObject.tag.Equals("Player"))
        {
            canGrab = true;

        }
    }

    // Update is called once per frame
    void Update () {
        if (canGrab)
        {
            if (Input.GetKey(KeyCode.E))
            {
                grabPos = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
                ball.transform.position = grabPos;
                left.SetPosition(0, new Vector3(grabPos.x, grabPos.y, grabPos.z));
                right.SetPosition(0, new Vector3(grabPos.x, grabPos.y, grabPos.z));
            }

            if (Input.GetKey(KeyCode.E))
            {


                ball.transform.position = grabPos;

                left.SetPosition(0, new Vector3(grabPos.x, grabPos.y, grabPos.z ));
                right.SetPosition(0, new Vector3(grabPos.x, grabPos.y, grabPos.z));

                Vector3 Vec3L = new Vector3(-2.0f - grabPos.x, 2.0f - grabPos.y, -grabPos.z);
                Vector3 Vec3R = new Vector3(2.0f - grabPos.x, 2.0f - grabPos.y, -grabPos.z);
                Vector3 Dir = (Vec3L + Vec3R).normalized;

                ball.transform.GetComponent<Rigidbody>().useGravity = true;
                ball.transform.GetComponent<Rigidbody>().AddForce(Dir * 9.0f, ForceMode.Impulse);

                left.SetPosition(0, new Vector3(0F, 1.8F, 0F));
                right.SetPosition(0, new Vector3(0F, 1.8F, 0F));
            }

            canGrab = false;
        }
	}
}
