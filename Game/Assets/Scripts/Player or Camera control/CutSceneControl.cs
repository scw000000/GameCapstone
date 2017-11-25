﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneControl : MonoBehaviour {
    public bool _useSelfGO = false;
    public GameObject _referencedTimelineGO;
    public GameObject _cutSceneCameraGO;
    public bool _enableTrigger;
    private Camera _cutSceneCamera;
    public AudioListener _cutsceneAudioListener;
    private GameObject _DisabledCameraAGO;
    private GameObject _DisabledCameraBGO;
    private bool _isActivated = true;
    public GameObject _endCutScenetEventGO;
    // Use this for initialization
    void Start () {
        if (_useSelfGO)
        {
            _referencedTimelineGO = gameObject;
        }
        _cutSceneCamera = _cutSceneCameraGO.GetComponent<Camera>();
        _cutsceneAudioListener = _cutSceneCameraGO.GetComponent<AudioListener>();

    }
	
	// Update is called once per frame
	void Update () {
	}

    private void PlayTimeLine()
    {
        _referencedTimelineGO.GetComponent<PlayableDirector>().Play();
        
    }

    public void StartTimeLine() {
        // _referencedTimelineGO.GetComponent<Collider>().enabled = false;
        var managerComp = GameObject.Find("GameManager").GetComponent<GameManager>();
        managerComp.SetPlayerInput(false);
        _cutSceneCameraGO.GetComponent<AudioListener>().enabled = true;
        _cutSceneCameraGO.GetComponent<Camera>().enabled = true;
        if (_DisabledCameraAGO == null)
        {
            _DisabledCameraAGO = GameObject.Find("CameraA");
            _DisabledCameraBGO = GameObject.Find("CameraB");
        }
        _DisabledCameraAGO.SetActive(false);
        _DisabledCameraBGO.SetActive(false);
        PlayTimeLine();
        //Invoke("SwitchBackCameras", (float)gameObject.GetComponent<PlayableDirector>().duration);
        
        StartCoroutine("WaitUntilFinished");
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_enableTrigger || !other.tag.Equals("Player")) {
            return;
        }
        if (!_isActivated) {
            return;
        }
        _isActivated = false;
        StartTimeLine();
    }

    private void SwitchBackCameras() {
        _cutSceneCameraGO.GetComponent<AudioListener>().enabled = false;
        _DisabledCameraAGO.SetActive(true);
        _DisabledCameraBGO.SetActive(true);
        _cutSceneCameraGO.GetComponent<Camera>().enabled = false;
    }

    private IEnumerator WaitUntilFinished()
    {
        while (System.Math.Abs(_referencedTimelineGO.GetComponent<PlayableDirector>().time -
            _referencedTimelineGO.GetComponent<PlayableDirector>().duration ) > 0.001)
        {
            yield return null;
        }
        SwitchBackCameras();
        if (_endCutScenetEventGO != null)
        {
            _endCutScenetEventGO.SetActive(true);
        }
        var managerComp = GameObject.Find("GameManager").GetComponent<GameManager>();
        managerComp.SetPlayerInput(true);
    }

    //private IEnumerator ReturnToCameraPosition() {
    //    float accumulatedTime = 0f;

    //    while (accumulatedTime < (float)gameObject.GetComponent<PlayableDirector>().duration) {
    //        accumulatedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //    while (Vector3.Distance(_DisabledCameraAGO.transform.position, _cutSceneCameraGO.transform.position) > 0.01) {
    //        _cutSceneCameraGO.transform.position = Vector3.Lerp(_cutSceneCameraGO.transform.position, _DisabledCameraAGO.transform.position, _returnToCameraSpeed);
    //        _cutSceneCameraGO.transform.rotation = Quaternion.Lerp(_cutSceneCameraGO.transform.rotation, _DisabledCameraAGO.transform.rotation, _returnToCameraSpeed);
    //        yield return null;
    //    }
    //    _cutSceneCameraGO.transform.position = _DisabledCameraAGO.transform.position;
    //    _cutSceneCameraGO.transform.rotation = _DisabledCameraAGO.transform.rotation;
    //    SwitchBackCameras();
    //}
}
