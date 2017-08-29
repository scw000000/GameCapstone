using UnityEngine;
using System.Collections;

public class AvatarControl : MonoBehaviour
   {
   public float MovingSpeed = 15.0f;
   public float ScalingSpeed = 3.0f;
   public const float RotatingSpeed = 500.0f;
   private Rigidbody RigidBody = null;
   private GameObject CameraRoot;

    // Use this for initialization
    void Start()
      {
      Debug.Log( gameObject.name );
      RigidBody = gameObject.GetComponent<Rigidbody>();
      if( RigidBody == null )
         {
         Debug.LogError( "Cannot find rigid body of the avatar" );
         }

        CameraRoot = gameObject.transform.Find("CameraRoot").gameObject;
    }

   // Update is called once per frame
   void Update()
      {
      if( Input.GetKey( KeyCode.A ) )
         {
         gameObject.transform.Translate( new Vector3( -1.0f * Time.deltaTime * MovingSpeed, 0.0f, 0.0f ) );
         }

      if( Input.GetKey( KeyCode.D ) )
         {
         gameObject.transform.Translate( new Vector3( Time.deltaTime * MovingSpeed, 0.0f, 0.0f ) );
         }

      if( Input.GetKey( KeyCode.W ) )
         {
         gameObject.transform.Translate( new Vector3( 0.0f, 0.0f, Time.deltaTime * MovingSpeed ) );
         }

      if( Input.GetKey( KeyCode.S ) )
         {
         gameObject.transform.Translate( new Vector3( 0.0f, 0.0f, -1.0f * Time.deltaTime * MovingSpeed ) );
         }
      
      if( Input.GetKey( KeyCode.Mouse1 ) )
         {
            gameObject.transform.RotateAround( gameObject.transform.position, new Vector3( 0.0f, 1.0f, 0.0f ), Input.GetAxis("Mouse X") * Time.deltaTime * RotatingSpeed );


            CameraRoot.transform.RotateAround(CameraRoot.transform.position, gameObject.transform.right, -1.0f * Input.GetAxis("Mouse Y") * Time.deltaTime * RotatingSpeed );
         }

      }
   }
