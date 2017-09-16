using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour {
    private Animator m_ArmAnimator;
    private float speed;

	// Use this for initialization
	void Start () {
        m_ArmAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        speed = Input.GetAxis("Vertical")* Input.GetAxis("Vertical") + Input.GetAxis("Horizontal")* Input.GetAxis("Horizontal");
        m_ArmAnimator.SetFloat("Speed",speed);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_ArmAnimator.SetBool("IsRunning",true);
        }
        else {
            m_ArmAnimator.SetBool("IsRunning", false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            float random = Random.Range(0.0f, 1f);
            if (random > 0.5f)
            {
                m_ArmAnimator.SetTrigger("PunchLeft");
            }
            else
            {
                m_ArmAnimator.SetTrigger("PunchRight");
            }
        }
        else {
            m_ArmAnimator.ResetTrigger("PunchLeft");
            m_ArmAnimator.ResetTrigger("PunchRight");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_ArmAnimator.SetTrigger("Jump");
        }
        else {
            m_ArmAnimator.ResetTrigger("Jump");
        }
	}
}
