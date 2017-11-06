using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public float _followingSpeed;
    public GameObject _followingRoot;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void FixedUpdate () {
        gameObject.transform.position = Vector3.Lerp(
            gameObject.transform.position, _followingRoot.transform.position, _followingSpeed * Time.deltaTime);

        gameObject.transform.rotation = Quaternion.Lerp(
            gameObject.transform.rotation, _followingRoot.transform.rotation, _followingSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        //gameObject.transform.position = Vector3.Lerp(
        //    gameObject.transform.position, _followingRoot.transform.position, _followingSpeed * Time.deltaTime);

        //gameObject.transform.rotation = Quaternion.Lerp(
        //    gameObject.transform.rotation, _followingRoot.transform.rotation, _followingSpeed * Time.deltaTime);
    }

    public void SetupRoot(GameObject followingRoot) {
        _followingRoot = followingRoot;
    }
}
