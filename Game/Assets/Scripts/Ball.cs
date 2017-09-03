using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public GameObject AttachRoot;
    public float LaunchForceScalar = 40f;
    public float ReflectSpeedScalar = 30f;
    public float MaxPaddleReflectionAngle = 40f;
    public float ReflectShiftSpeedScalar = 10f;
    private bool isAttached;
	private Vector3 locShift;
	// Use this for initialization
	void Start () {
		isAttached = true;
		// locShift = new Vector3 (0.0f,0.0f,1.0f);
	}
	
	// Update is called once per frame
	void Update () {
        if (isAttached)
        {
            if (AttachRoot)
            {
                transform.position = AttachRoot.transform.position + locShift;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Launch();
                }
            }
        }
        else
        {
            // transform.Translate (velocity*Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.R))
            {
                Respawn();
            }
        }
    }

	void Launch(){
		if(isAttached){
            var ballRigidBody = gameObject.GetComponent<Rigidbody>();
            ballRigidBody.velocity = new Vector3();
            var appliedForce = AttachRoot.transform.forward;
			if (Input.GetKey(KeyCode.D)) {
                appliedForce = Quaternion.AngleAxis(45, Vector3.up) * appliedForce;

            } else if (Input.GetKey(KeyCode.A)) {
                appliedForce = Quaternion.AngleAxis(-45, Vector3.up) * appliedForce;
            }
            appliedForce *= LaunchForceScalar;
            ballRigidBody.AddForce(appliedForce, ForceMode.Impulse);
        }
		isAttached = false;
	}
	void OnCollisionEnter(Collision col){
        if (isAttached) {
            return;
        }
        var ballRigidBody = gameObject.GetComponent<Rigidbody>();
        var newVelocity = ballRigidBody.velocity;
        newVelocity.Normalize();
        // Debug.DrawRay(ballRigidBody.transform.position, newVelocity, Color.green, 3f);
        // newVelocity = Vector3.Reflect(newVelocity, col.gameObject.transform.forward);
        // Debug.DrawRay(ballRigidBody.transform.position, col.gameObject.transform.forward, Color.red, 3f);
        //// Debug.DrawRay(ballRigidBody.transform.position, newVelocity, Color.blue, 3f);
        // Only paddle can chagne the direction of velocity
        if (col.gameObject.tag.Equals("PlayerTag"))
        {
            // Decide rotation degree based on x postion difference
            float xDifference = ballRigidBody.transform.position.x - col.gameObject.transform.position.x;
            // Normallize it to +- 1
            xDifference /= (ballRigidBody.gameObject.transform.localScale.x * 0.5f);
            var newForce = xDifference > 0f ? col.gameObject.transform.right : -col.gameObject.transform.right;
            Debug.DrawRay(ballRigidBody.transform.position, newForce, Color.red, 3f);
            ballRigidBody.AddForce(gameObject.transform.right * xDifference * ReflectShiftSpeedScalar, ForceMode.Impulse);
        }
        // Make the force constant
        newVelocity *= ReflectSpeedScalar;
        ballRigidBody.velocity = newVelocity;
    }
	void Respawn(){
		// player = GameObject.Find ("Player");
		isAttached = true;
		transform.position = AttachRoot.transform.position + locShift;
	}
}
