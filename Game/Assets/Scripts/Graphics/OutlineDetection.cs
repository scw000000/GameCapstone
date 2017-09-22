﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineDetection : MonoBehaviour {
    public Camera _camera { get; set; }
    private GameObject _currentOutlineGO;
    private float _rayCastFreq = 0.1f;
    private float _currTime = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _currTime += Time.deltaTime;
        if (_currTime < _rayCastFreq) {
            return;
        }
        _currTime = 0f;

        if (_camera.renderingPath != RenderingPath.DeferredShading) {
            TryToDisableOutline();
            return;
        }

        int visibleLayer = _camera.cullingMask;
        RaycastHit hitResult;
        if (!Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hitResult, 20f, visibleLayer))
        {
            TryToDisableOutline();
            return;
        }
        if (_currentOutlineGO != null && hitResult.transform.gameObject.GetInstanceID() != _currentOutlineGO.GetInstanceID()) {
            TryToDisableOutline();
        }
        var otulineControlComp = hitResult.transform.gameObject.GetComponent<OutlineControl>();
        if (otulineControlComp != null) {
            otulineControlComp.SetEnableOutline(true);
            _currentOutlineGO = hitResult.transform.gameObject;
        }
         
    }

    private void TryToDisableOutline() {
        if (_currentOutlineGO == null) {
            return;
        }
        _currentOutlineGO.GetComponent<OutlineControl>().SetEnableOutline(false);
        _currentOutlineGO = null;
    }
}
