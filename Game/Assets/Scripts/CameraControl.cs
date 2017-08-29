using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
   {
   public GameObject m_TargetObject;
   // Use this for initialization
   void Start()
      {
      }

   
   // Update is called once per frame
   void Update()
      {
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, m_TargetObject.transform.rotation, 10.0f * Time.deltaTime);
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, m_TargetObject.transform.position, 10.0f * Time.deltaTime);

      }
   }
