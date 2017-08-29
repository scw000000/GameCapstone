using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShift : MonoBehaviour {
    public GameObject m_Player;
    public float m_ViewRotationSpeed;
    private Quaternion m_InitialRotation;
    private Vector3 prevMousePos;
    private Vector3 currMousePos;

    // Use this for initialization
    void Start()
    {
        m_ViewRotationSpeed = 25.0f;
        m_InitialRotation = gameObject.transform.rotation;
        prevMousePos = Input.mousePosition;
    }


    // Update is called once per frame
    void Update()
    {
        currMousePos = Input.mousePosition;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 deltaMousePos = currMousePos - prevMousePos;
            gameObject.transform.RotateAround(m_Player.transform.position, gameObject.transform.right, -1.0f * deltaMousePos.y * Time.deltaTime * m_ViewRotationSpeed);
            gameObject.transform.RotateAround(m_Player.transform.position, new Vector3(0.0f, 1.0f, 0.0f), deltaMousePos.x * Time.deltaTime * m_ViewRotationSpeed);
        }

        prevMousePos = currMousePos;
        //  Quaternion.
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameObject.transform.rotation = m_InitialRotation;
        }
    }
}
