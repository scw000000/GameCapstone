using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFacingPlayer : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Camera.main == null) {
            return;
        }
        var camPos = Camera.main.transform.position;
        // Remove pitch
        camPos.y = gameObject.transform.position.y;
        var targetRot = Quaternion.LookRotation((camPos - gameObject.transform.position).normalized, Vector3.up);
        gameObject.transform.rotation = targetRot;
	}
}
