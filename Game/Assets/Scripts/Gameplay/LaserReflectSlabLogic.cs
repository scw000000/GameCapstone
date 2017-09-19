using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReflectSlabLogic : MonoBehaviour {
    private GameObject _playerGO;
    private GameObject _slabGO;
    private Color _originalColor;
    private bool _isPlayerInside = false;
	// Use this for initialization
	void Start () {
        _slabGO = gameObject.transform.parent.gameObject;
        _originalColor = _slabGO.GetComponent<MeshRenderer>().material.GetColor("_Color");
    }
	
	// Update is called once per frame
	void Update () {
        if (_isPlayerInside && Input.GetButton("Interaction"))
        {
            if (Input.GetButtonDown("Interaction"))
            {
                _slabGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0.5f));
            }
            _slabGO.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, _playerGO.transform.rotation, 0.7f * Time.deltaTime);
        }
        else
        {
            _slabGO.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(_originalColor.r, _originalColor.g, _originalColor.b, _originalColor.a));
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _playerGO = other.gameObject;
            _isPlayerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
        _isPlayerInside = false;
        }
    }
}
