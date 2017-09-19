using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRotation : MonoBehaviour {

    private float rotateSpeed;
    private bool isDuringRotation;
    
    private float deltaDegree;
	// Use this for initialization
	void Start () {
        isDuringRotation = false;
        rotateSpeed = 30.0f;
        deltaDegree = 0.0f;

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.P))
        {
            Rotate90();
            //isDuringRotation = true;
            //Quaternion destRotation = Quaternion.Euler(0, 90, 0);
        }

        if (isDuringRotation) {
            //transform.rotation = Quaternion.Lerp(transform.rotation, destRotation, Time.time * rotateSpeed);
            transform.RotateAround(transform.position, transform.up,  Time.deltaTime*rotateSpeed);
            deltaDegree += Time.deltaTime * rotateSpeed;
        }




        if (deltaDegree > 90.0f) {
            isDuringRotation = false;
            deltaDegree = 0.0f;
            Debug.Log("Rotating false");
        }

    }

    public void Rotate90()
    {
        if (!isDuringRotation)
        {
            isDuringRotation = true;
            //destRotation.RotateAround(transform.position, transform.up, 90.0f);
            //Debug.Log("Rotate90 true");
            //Debug.Log(destRotation.ToString());
        }
        /*else
        {
            Debug.Log("Rotate90 true");
        }*/
    }
}
