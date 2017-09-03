using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoCapture : MonoBehaviour {
    private bool IsCaptureing = false;
    public GameObject TempCameraPrefab;
    // This camera is for recording
    public GameObject MainCamera;
    // This camera will be spawned once started recording because
    // video capture plugin is stupid, it cannot display image atm
    private GameObject TempCamera;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!IsCaptureing)
            {
                TempCamera = Instantiate(TempCameraPrefab, MainCamera.transform);
                TempCamera.GetComponent<AudioListener>().enabled = false;
                TempCamera.GetComponent<FadeInEffect>().enabled = false;
                TempCamera.GetComponent<CameraFollow>().enabled = false;
                RockVR.Video.VideoCaptureCtrl.instance.StartCapture();
            }
            else
            {
                RockVR.Video.VideoCaptureCtrl.instance.StopCapture();
                MainCamera.GetComponent<Camera>().targetTexture = null;
                Destroy(TempCamera);
            }
            IsCaptureing = !IsCaptureing;
        }
    }
}
