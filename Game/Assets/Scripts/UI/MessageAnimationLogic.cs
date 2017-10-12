using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageAnimationLogic : MonoBehaviour {
    public Vector2 _panelHideShift;
    public AnimationCurve _panelShiftAnimCurve;
    public AnimationCurve _panelImageAlphaAnimCurve;
    public AnimationCurve _textAlphaAnimCurve;
    public float _panelDisplayAnimTime = 1f;
    public float _panelHideAnimTime = 2f;

    private float _panelCurrLerp = 0f;
    private Vector2 _panelHideAnchorPos;
    private Vector2 _panelDisplayAnchorPos;
    private Text _messageText;
    private Image _messageIcon;

    private Color _panelImageDisplayColor;
    private Color _panelImageHideColor;

    private Color _textDisplayColor;
    private Color _textHideColor;

    private Color _textIconDisplayColor;
    private Color _textIconHideColor;

    private class MessageRequest {
        public string _message;
        public float _time;
        public string _iconName;
        public MessageRequest(string msg, float time, string icon = "") {
            _message = msg;
            _time = time;
            _iconName = icon;
        }
    }

    private bool _isRunning;
    private bool _keepDisplaying;
    private Queue<MessageRequest> _displayRequests;
    // Use this for initialization
    void Start () {
        _panelDisplayAnchorPos = gameObject.GetComponent<RectTransform>().anchoredPosition;
        _panelHideAnchorPos = _panelDisplayAnchorPos + _panelHideShift;
        gameObject.GetComponent<RectTransform>().anchoredPosition = _panelHideAnchorPos;
        _messageText = gameObject.transform.Find("Text").GetComponent<Text>();
        _messageIcon = gameObject.transform.Find("Icon").GetComponent<Image>();

        _panelImageDisplayColor = gameObject.GetComponent<Image>().color;
        _panelImageHideColor = new Color(
            _panelImageDisplayColor.r,
            _panelImageDisplayColor.g,
            _panelImageDisplayColor.b,
            0f);

        _textDisplayColor = _messageText.color;
        _textHideColor = new Color(
            _textDisplayColor.r,
            _textDisplayColor.g,
            _textDisplayColor.b,
            0f);

        _textIconDisplayColor = _messageIcon.color;
        _textIconHideColor = new Color(
            _textIconDisplayColor.r,
            _textIconDisplayColor.g,
            _textIconDisplayColor.b,
            0f);

        _displayRequests = new Queue<MessageRequest>();

        _isRunning = false;
        _keepDisplaying = true;
        SetupDisplay(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            TerminateDisplay();
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
          //  DisplayMessage("Fuck you", 3f);
        }
        if(Input.GetKeyDown(KeyCode.F7))
        {
            // DisplayMessage("Fuck you", 0f);
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition +=
                new Vector2(10, 0);
        }

        if (!_isRunning && _displayRequests.Count > 0) {
            var request = _displayRequests.Peek();
            _messageText.text = request._message;
            var sprite = Resources.Load<Sprite>("Icons/" + request._iconName);
            if (sprite != null) {
                _messageIcon.sprite = sprite;
            }
            StartCoroutine("DisplayGameMessageCycle", request._time);
            _displayRequests.Dequeue();
        }
    }


    public void DisplayMessage(string message, float time, string iconName = "")
    {
        _displayRequests.Enqueue( new MessageRequest(message, time, iconName) );
    }

    private IEnumerator DisplayGameMessageCycle(float displayTime)
    {
        _isRunning = true;
        _keepDisplaying = true;
        yield return MoveGameMessageToFront();
        if (displayTime > 0)
        {
            float currTime = 0f;
            while (_keepDisplaying && currTime < displayTime) {
                currTime += Time.deltaTime;
                yield return null;
            }
            //yield return new WaitForSeconds(displayTime);
        }
        else
        {
            _keepDisplaying = true;
            while (_keepDisplaying) {
                yield return null;
            }
        }
        yield return MoveGameMessageToBack();
        yield return null;
        _isRunning = false;
    }

    private IEnumerator MoveGameMessageToFront()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = _panelHideAnchorPos;
        SetupDisplay(true);
        _panelCurrLerp = 0f;
        while (_panelCurrLerp < 1f)
        {
            _panelCurrLerp += Time.deltaTime / _panelDisplayAnimTime;
            gameObject.GetComponent<RectTransform>().anchoredPosition =
            Vector2.Lerp(_panelHideAnchorPos, _panelDisplayAnchorPos, _panelShiftAnimCurve.Evaluate(_panelCurrLerp));

            gameObject.GetComponent<Image>().color = Color.Lerp(
                _panelImageHideColor,
                _panelImageDisplayColor,
                _panelImageAlphaAnimCurve.Evaluate(_panelCurrLerp)
                );

            _messageText.color = Color.Lerp(
                _textHideColor,
                _textDisplayColor,
                _textAlphaAnimCurve.Evaluate(_panelCurrLerp)
                );

            _messageIcon.color = Color.Lerp(
                _textIconHideColor,
                _textIconDisplayColor,
                _panelImageAlphaAnimCurve.Evaluate(_panelCurrLerp)
                );
            yield return null;
        }
        gameObject.GetComponent<RectTransform>().anchoredPosition = _panelDisplayAnchorPos;
        yield return null;
    }

    private IEnumerator MoveGameMessageToBack()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = _panelDisplayAnchorPos;
        _panelCurrLerp = 1f;
        while (_panelCurrLerp > 0f)
        {
            _panelCurrLerp -= Time.deltaTime / _panelDisplayAnimTime;
            gameObject.GetComponent<RectTransform>().anchoredPosition =
            Vector2.Lerp(_panelHideAnchorPos, _panelDisplayAnchorPos, _panelShiftAnimCurve.Evaluate(_panelCurrLerp));

            gameObject.GetComponent<Image>().color = Color.Lerp(
                _panelImageHideColor,
                _panelImageDisplayColor,
                _panelImageAlphaAnimCurve.Evaluate(_panelCurrLerp)
                );

            _messageText.color = Color.Lerp(
                _textHideColor,
                _textDisplayColor,
                _textAlphaAnimCurve.Evaluate(_panelCurrLerp)
                );

            _messageIcon.color = Color.Lerp(
                _textIconHideColor,
                _textIconDisplayColor,
                _panelImageAlphaAnimCurve.Evaluate(_panelCurrLerp)
                );
                yield return null;
        }
        SetupDisplay(false);
        gameObject.GetComponent<RectTransform>().anchoredPosition = _panelHideAnchorPos;
        yield return null;
    }

    private void SetupDisplay(bool isEnabled) {
        gameObject.GetComponent<Image>().enabled = isEnabled;
        _messageText.enabled = isEnabled;
        _messageIcon.enabled = isEnabled;
    }

    public void TerminateDisplay() {
        _keepDisplaying = false;
    }
}
