using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public GameObject FollowingRoot;
    public float FollowingSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = Vector3.Lerp(
            gameObject.transform.position, FollowingRoot.transform.position, FollowingSpeed * Time.deltaTime);

        gameObject.transform.rotation = Quaternion.Lerp(
            gameObject.transform.rotation, FollowingRoot.transform.rotation, FollowingSpeed * Time.deltaTime);
    }
}
