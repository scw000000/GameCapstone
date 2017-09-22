using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalRingAnimation : MonoBehaviour {
    public float _animSpeed = 1f;
    public float _pulseLightPeriod = 3f;
    private float _currYPos = 0f;
    private float _currentTime = 0f;
    private float _pulseLightAlpha = 0f;
    
    private Material _mat;
	// Use this for initialization
	void Start () {
        _mat = gameObject.GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        _currYPos += Time.deltaTime * _animSpeed;
        _currYPos %= 1;
        _mat.SetFloat("_ScanLineBandYCenter", _currYPos - 0.5f);

        _currentTime += Time.deltaTime;
        _pulseLightAlpha = Mathf.Repeat(_currentTime / _pulseLightPeriod, 1f);
        _mat.SetFloat("_PulseFactor", Mathf.Abs(Mathf.Sin(_pulseLightAlpha * Mathf.PI)) * 0.7f + 0.3f);

    }
}
