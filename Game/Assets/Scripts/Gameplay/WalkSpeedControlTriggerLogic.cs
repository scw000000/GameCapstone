using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSpeedControlTriggerLogic : MonoBehaviour {
    public float _triggeredWalkSpeed;
    public float _triggeredRunSpeed;
    private float _origWalkSpeed;
    private float _origRunSpeed;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController _fpsComp;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (!other.tag.Equals("Player"))
        {
            return;
        }
        if (_fpsComp == null)
        {
            _fpsComp = other.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
            _origWalkSpeed = _fpsComp.m_WalkSpeed;
            _origRunSpeed = _fpsComp.m_RunSpeed;
        }

        bool insidePortal = _fpsComp.GetComponent<WorldSwitch>()._insidePortal;
        if ( (_fpsComp.gameObject.layer == gameObject.layer && !insidePortal ) 
            || ( (_fpsComp.gameObject.layer == LayerMask.NameToLayer("WorldA") && gameObject.layer == LayerMask.NameToLayer("WorldAInPortal") )
                || (_fpsComp.gameObject.layer == LayerMask.NameToLayer("WorldB") && gameObject.layer == LayerMask.NameToLayer("WorldBInPortal"))
                && insidePortal)
            || ( ( _fpsComp.gameObject.layer == LayerMask.NameToLayer("WorldA") && gameObject.layer == LayerMask.NameToLayer("WorldAInPortal")
                || (_fpsComp.gameObject.layer == LayerMask.NameToLayer("WorldB") && gameObject.layer == LayerMask.NameToLayer("WorldBInPortal")) )
                && !insidePortal))
        {
            _fpsComp.m_WalkSpeed = _triggeredWalkSpeed;
            _fpsComp.m_RunSpeed = _triggeredRunSpeed;
        }
        else
        {
            _fpsComp.m_WalkSpeed = _origWalkSpeed;
            _fpsComp.m_RunSpeed = _origRunSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.tag.Equals("Player"))
        {
            return;
        }

        _fpsComp.m_WalkSpeed = _origWalkSpeed;
        _fpsComp.m_RunSpeed = _origRunSpeed;
    }
}
