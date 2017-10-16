using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class LaserReflectSlabLogic : MonoBehaviour
{
    private GameObject _playerGO;
    private GameObject _slabGO;
    private Color _originalColor;
    private bool _isPlayerInside = false;
    private Quaternion _slabRotate;
    // Use this for initialization
    void Start()
    {
        _slabGO = gameObject.transform.parent.gameObject;
        _originalColor = _slabGO.GetComponent<MeshRenderer>().material.GetColor("_Color");
    }

    // Update is called once per frame
    void Update()
    {

        if (_isPlayerInside && Input.GetButton("Interaction"))
        {
            updateRotation();
            
            if (Input.GetButtonDown("Interaction"))
            {
                _slabGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0.5f));
                _playerGO.SendMessage("InBounceBox", true);
            }
            _slabGO.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, _slabRotate, 0.7f * Time.deltaTime);
        }
        else
        {
            if (_isPlayerInside && Input.GetButtonUp("Interaction"))
            {
                _playerGO.SendMessage("InBounceBox", false);
            }
            if(!_isPlayerInside && !Input.GetButton("Interaction"))
            {
                if (_playerGO != null)
                {
                    _playerGO.SendMessage("InBounceBox", false);
                }
            }
            _slabGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(_originalColor.r, _originalColor.g, _originalColor.b, _originalColor.a));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _playerGO = other.gameObject;
            _isPlayerInside = true;
            if (!Input.GetButton("Interaction"))
            {
                _slabRotate = _slabGO.transform.rotation;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _isPlayerInside = false;
        }
    }

    void updateRotation()
    {
        float yRot = Input.GetAxis("Mouse X");
        _slabRotate *= Quaternion.Euler(0f, yRot, 0f);
    }

}
