using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeUILogic : MonoBehaviour {
    public GameObject _listeningGO;
    public Color _inactiveColor;
    public float _checkFrequency = 0.1f;
    public float _inactiveAnimPeriod = 0.8f;
    public float _activeAnimPeriod = 0.3f;
    public AnimationCurve _activeAnimCurve;
    public AnimationCurve _inactiveAnimCurve;


    private AudioListener _listeningAudioListener;
    private Color _origColor;
    private float _currAlpha;
    private AnimationCurve _currAnimCurve;
    private float _currAnimPeriod;
    private Color _fromColor;
    private Color _toColor;

    private bool _previousActive = true;
	// Use this for initialization
	void Start () {
        _listeningAudioListener = _listeningGO.GetComponent<AudioListener>();
        _origColor = gameObject.GetComponent<UnityEngine.UI.Image>().color;
        StartCoroutine("UpdateCurve");
    }
	
	// Update is called once per frame
	void Update () {
        if (_currAnimCurve == null)
        {
            return;
        }

        _currAlpha += Time.deltaTime / _currAnimPeriod;
        gameObject.GetComponent<UnityEngine.UI.Image>().color =
            Color.Lerp(_fromColor, _toColor, _currAnimCurve.Evaluate(_currAlpha));

        if (_currAlpha >= 1f)
        {
            _currAnimCurve = null;
            gameObject.GetComponent<UnityEngine.UI.Image>().color = _toColor;
        }
        //if (_listeningAudioListener.enabled)
        //{
        //    gameObject.GetComponent<UnityEngine.UI.Image>().color = _origColor;
        //}
        //else
        //{
        //    gameObject.GetComponent<UnityEngine.UI.Image>().color = _inactiveColor;
        //}
    }

    IEnumerator UpdateCurve()
    {
        while (true)
        {
            if (_listeningAudioListener.enabled != _previousActive)
            {
                _currAlpha = 0f;
                // from inactive to active
                if (_listeningAudioListener.enabled)
                {
                    _fromColor = _inactiveColor;
                    _toColor = _origColor;
                    _currAnimPeriod = _activeAnimPeriod;
                    _currAnimCurve = _activeAnimCurve;
                }
                else // opposite direction
                {
                    _toColor = _inactiveColor;
                    _fromColor = _origColor;
                    _currAnimPeriod = _inactiveAnimPeriod;
                    _currAnimCurve = _inactiveAnimCurve;
                }
            }
            _previousActive = _listeningAudioListener.enabled;
            yield return new WaitForSeconds(_checkFrequency);
        }
    }
}
