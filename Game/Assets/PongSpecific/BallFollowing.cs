using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFollowing : MonoBehaviour {
    public float FollowingSpeed;
    private GameObject FollowingGO;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        // I'm sorry. Im lazy
        if (FollowingGO == null) {
            FollowingGO = GameObject.FindGameObjectWithTag("Ball");
            if (FollowingGO == null) {
                return;
            }
        }
        
        Vector3 forwardVec = FollowingGO.transform.position - gameObject.transform.position;
        forwardVec.Normalize();
        // Prevent light shaking
        if (Vector3.Distance(forwardVec, gameObject.transform.forward) < 0.1f) {
            return;
        }
        var targetPos = Vector3.Lerp(gameObject.transform.position, FollowingGO.transform.position, FollowingSpeed * Time.deltaTime);
        targetPos.y = gameObject.transform.position.y;
        gameObject.transform.position = targetPos;
        
	}
}
