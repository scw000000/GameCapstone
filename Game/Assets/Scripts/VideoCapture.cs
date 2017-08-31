using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoCapture : MonoBehaviour {
    private bool IsCaptureing = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!IsCaptureing)
            {
                RockVR.Video.VideoCaptureCtrl.instance.StartCapture();
            }
            else
            {
                RockVR.Video.VideoCaptureCtrl.instance.StopCapture();
            }
            IsCaptureing = !IsCaptureing;
        }
    }
}
