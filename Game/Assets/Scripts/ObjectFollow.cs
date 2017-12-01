using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow : MonoBehaviour {
    public bool _followRotation = false;
    public bool _followLocation = true;
    public float _rotationFollowSpeed = 0.3f;
    public float _locationFollowSpeed = 0.5f;
    public GameObject _followTarget;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_followTarget == null)
        {
            return;
        }
        if (_followLocation)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, _followTarget.transform.position, _locationFollowSpeed);
        }
        if (_followRotation)
        {
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, _followTarget.transform.rotation, _rotationFollowSpeed);
        }
	}
}
