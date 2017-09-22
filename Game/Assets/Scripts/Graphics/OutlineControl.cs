using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineControl : MonoBehaviour {
    public Color _outlineColor;
    private Color _outlineMinColor;
    public bool _onlyOutline = true;
    private string _shaderName = "Unlit/OutlineCapture";
    private GameObject _cloneMeshGO;
    private MeshRenderer _rendererComp;
    private bool _isActivated = false;
    private float _pulseLightAlpha = 0f;
    private float _pulseLightMaxShutdownTime = 1f;
    private float _pulseLightPeriod = 3f;
    private float _currentTime = 0f;
    // Use this for initialization
    void Start () {
        var meshFilterComp = gameObject.GetComponent<MeshFilter>();
        _cloneMeshGO = new GameObject("MeshClone");
        _cloneMeshGO.transform.parent = gameObject.transform;
        _cloneMeshGO.transform.localPosition = Vector3.zero;
        _cloneMeshGO.transform.localRotation = Quaternion.identity;
        _cloneMeshGO.transform.localScale = Vector3.one;
        _cloneMeshGO.layer = LayerMask.NameToLayer("Outline");

        var cloneMeshFilterComp = _cloneMeshGO.AddComponent<MeshFilter>();
        cloneMeshFilterComp.mesh = meshFilterComp.mesh;

        _rendererComp = _cloneMeshGO.AddComponent<MeshRenderer>();
        _rendererComp.material = new Material(Shader.Find(_shaderName));
        _rendererComp.enabled = false;
        if (_onlyOutline) {
            _outlineColor.a = 1f;
        }
        _outlineMinColor = _outlineColor * 0.1f;
        _outlineMinColor.a = 1f;
        _rendererComp.material.SetColor("_OutlineColor", _outlineColor);
        _rendererComp.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _rendererComp.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
    }

    // Update is called once per frame
    void Update () {
        if (!_isActivated && !_rendererComp.enabled) {
            return;
        }
        if (_isActivated)
        {
            _rendererComp.enabled = true;
            _currentTime += Time.deltaTime;
            _pulseLightAlpha = Mathf.Repeat(_currentTime / _pulseLightPeriod, 1f);
        }
        else {
            _pulseLightAlpha -= Time.deltaTime / _pulseLightMaxShutdownTime;
            // right now rendercomp is stll enabled, 
            if (_pulseLightAlpha <= 0f)
            {
                _currentTime = 0f;
                _pulseLightAlpha = 0f;
                _rendererComp.enabled = false;
                return;
            }
        }
        _rendererComp.material.SetColor("_OutlineColor", 
            Color.Lerp(_outlineMinColor, _outlineColor, Mathf.Abs(Mathf.Sin( _pulseLightAlpha * Mathf.PI ))));

    }

    public void SetEnableOutline(bool isEnabled) {
        _isActivated = isEnabled;
    }
}
