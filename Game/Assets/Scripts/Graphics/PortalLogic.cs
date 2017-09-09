using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalLogic : MonoBehaviour {
    public float _portalMaxRadius = 8f;
    public AnimationCurve _portalRadiusCurve;
    public float _portalRadiusAnimTime;
    public float _portalLifeTime = 3f;
    private float _portalCurrentAnimTime;
    private float _portalCurrentRadius = 0f;
    private int _worldALayer;
    private int _worldBLayer;
    private Camera _cameraA;
    private Camera _cameraB;
    private GameObject _player;
    // Use this for initialization
    void Start () {
        _worldALayer = LayerMask.NameToLayer("WorldA");
        _worldBLayer = LayerMask.NameToLayer("WorldB");
        StartCoroutine("PortalLifeCircle");
        _cameraA = GameObject.Find("CameraA").GetComponent<Camera>();
        _cameraB = GameObject.Find("CameraB").GetComponent<Camera>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    IEnumerator PortalLifeCircle() {
        yield return Expand();
        yield return Idle();
        yield return Shrink();
        yield return null;
    }
    // Init and expanding animation
    IEnumerator Expand() {
        _portalCurrentAnimTime = 0f;
        while (_portalCurrentAnimTime < 1f) {
            _portalCurrentAnimTime += Time.deltaTime / _portalRadiusAnimTime;
            _portalCurrentRadius = _portalRadiusCurve.Evaluate(_portalCurrentAnimTime) * _portalMaxRadius;
            Shader.SetGlobalFloat("_SphereRadius", _portalCurrentRadius);
            transform.localScale = new Vector3(_portalCurrentRadius, _portalCurrentRadius, _portalCurrentRadius);
            yield return null;
        }
    }

    IEnumerator Idle(){
        yield return new WaitForSeconds(_portalLifeTime);
    }

    IEnumerator Shrink(){
        _portalCurrentAnimTime = 0f;
        while (_portalCurrentAnimTime < 1f)
        {
            _portalCurrentAnimTime += Time.deltaTime / _portalRadiusAnimTime;
            _portalCurrentRadius = _portalRadiusCurve.Evaluate(1f - _portalCurrentAnimTime) * _portalMaxRadius;
            Shader.SetGlobalFloat("_SphereRadius", _portalCurrentRadius);
            transform.localScale = new Vector3(_portalCurrentRadius, _portalCurrentRadius, _portalCurrentRadius);
            yield return null;
        }
        ClosePortal();
    }

    void ClosePortal() {
        Shader.SetGlobalFloat("_SphereRadius", 0f);
        Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        Shader.SetGlobalVector("_SphereCenter", gameObject.transform.position);
    }

    void SwapLayer(GameObject go) {
        if (go.layer == _worldALayer){
            go.layer = _worldBLayer;
        }
        else if (go.layer == _worldBLayer){
            go.layer = _worldALayer;
        }
    }

    void OnTriggerEnter(Collider other) {
        // Debug.Log("Enter");
        if (_player.GetInstanceID() == other.gameObject.GetInstanceID())
        {
            other.gameObject.GetComponent<WorldSwitch>().SetPortalStatus(true);
        }
        else if (other.gameObject.GetComponent<Rigidbody>() != null && ( other.gameObject.layer == _worldALayer || other.gameObject.layer == _worldBLayer ))
        {
            if (_player.GetComponent<WorldSwitch>()._insidePortal){
                other.gameObject.layer = _player.layer;
            }
            else {
                other.gameObject.layer = _player.layer == _worldALayer ?_worldBLayer: _worldALayer;
            }
        }

    }

    private void OnTriggerStay(Collider other){
        if (_player.GetInstanceID() != other.gameObject.GetInstanceID() && other.gameObject.GetComponent<Rigidbody>() != null && (other.gameObject.layer == _worldALayer || other.gameObject.layer == _worldBLayer))
        {
            if (_player.GetComponent<WorldSwitch>()._insidePortal)
            {
                other.gameObject.layer = _player.layer;
            }
            else
            {
                other.gameObject.layer = _player.layer == _worldALayer ? _worldBLayer : _worldALayer;
            }
        }
    }

    private void OnTriggerExit(Collider other){
        // Debug.Log("Leave");
        if (_player.GetInstanceID() == other.gameObject.GetInstanceID())
        {
            other.gameObject.GetComponent<WorldSwitch>().SetPortalStatus(false);
        }
        else if (other.gameObject.GetComponent<Rigidbody>() != null && (other.gameObject.layer == _worldALayer || other.gameObject.layer == _worldBLayer))
        {
            if (_player.GetComponent<WorldSwitch>()._insidePortal){
                other.gameObject.layer = _player.layer == _worldALayer ? _worldBLayer : _worldALayer;
            }
            else{
                other.gameObject.layer = _player.layer;
            }
        }
    }


}
