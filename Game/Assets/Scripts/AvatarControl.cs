using UnityEngine;
using System.Collections;

public class AvatarControl : MonoBehaviour
   {
   public float m_MovingSpeed = 15.0f;
   public float m_ScalingSpeed = 3.0f;
   public const float m_RotatingSpeed = 500.0f;
   private Rigidbody m_RigidBody = null;
   private Vector3 m_PrevMousePos;
   private Vector3 m_CurrMousePos;

   // Use this for initialization
   void Start()
      {
      Debug.Log( gameObject.name );
      m_RigidBody = gameObject.GetComponent<Rigidbody>();
      if( m_RigidBody == null )
         {
         Debug.LogError( "Cannot find rigid body of the avatar" );
         }

      m_PrevMousePos = Input.mousePosition;
      }

   // Update is called once per frame
   void Update()
      {
      if( Input.GetKey( KeyCode.A ) )
         {
         gameObject.transform.Translate( new Vector3( -1.0f * Time.deltaTime * m_MovingSpeed, 0.0f, 0.0f ) );
         }

      if( Input.GetKey( KeyCode.D ) )
         {
         gameObject.transform.Translate( new Vector3( Time.deltaTime * m_MovingSpeed, 0.0f, 0.0f ) );
         }

      if( Input.GetKey( KeyCode.W ) )
         {
         gameObject.transform.Translate( new Vector3( 0.0f, 0.0f, Time.deltaTime * m_MovingSpeed ) );
         }

      if( Input.GetKey( KeyCode.S ) )
         {
         gameObject.transform.Translate( new Vector3( 0.0f, 0.0f, -1.0f * Time.deltaTime * m_MovingSpeed ) );
         }

      m_CurrMousePos = Input.mousePosition;
      if( Input.GetKey( KeyCode.Mouse1 ) )
         {
            // gameObject.transform.RotateAround( gameObject.transform.position, gameObject.transform.right, -1.0f * deltaMousePos.y * Time.deltaTime * m_RotationSpeed );
            //Debug.Log(Input.GetAxis("Mouse X"));
            
            gameObject.transform.RotateAround( gameObject.transform.position, new Vector3( 0.0f, 1.0f, 0.0f ), Input.GetAxis("Mouse X") * Time.deltaTime * m_RotatingSpeed );
         }
      m_PrevMousePos = m_CurrMousePos;

      }
   }
