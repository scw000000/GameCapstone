using UnityEngine;
using System.Collections;

public class AvatarControl : MonoBehaviour
   {
   public float _movingSpeed = 15.0f;
   public const float _rotatingSpeed = 500.0f;
   public GameObject _portalBulletPrefab;
   private Rigidbody _rigidBody = null;
   private GameObject _cameraRoot;
    
    // Use this for initialization
    void Start()
      {
      _rigidBody = gameObject.GetComponent<Rigidbody>();
      if( _rigidBody == null )
         {
         Debug.LogError( "Cannot find rigid body of the avatar" );
         }

        _cameraRoot = gameObject.transform.Find("CameraRoot").gameObject;
    }

   // Update is called once per frame
   void Update() {
      if( Input.GetKey( KeyCode.A ) ) {
            // gameObject.transform.Translate( new Vector3( -1.0f * Time.deltaTime * _movingSpeed, 0.0f, 0.0f ) );
            _rigidBody.MovePosition(transform.position + transform.right * _movingSpeed * -Time.deltaTime);
            
         }

      if( Input.GetKey( KeyCode.D ) ) {
         //gameObject.transform.Translate( new Vector3( Time.deltaTime * _movingSpeed, 0.0f, 0.0f ) );
            _rigidBody.MovePosition(transform.position + transform.right * _movingSpeed * Time.deltaTime);
        }

      if( Input.GetKey( KeyCode.W ) ) {
            _rigidBody.MovePosition(transform.position + transform.forward * _movingSpeed * Time.deltaTime);
            //gameObject.transform.Translate( new Vector3( 0.0f, 0.0f, Time.deltaTime * _movingSpeed ) );
         }

      if( Input.GetKey( KeyCode.S ) ) {
            _rigidBody.MovePosition(transform.position + transform.forward * _movingSpeed * -Time.deltaTime);
            //gameObject.transform.Translate( new Vector3( 0.0f, 0.0f, -1.0f * Time.deltaTime * _movingSpeed ) );
         }
      
      if( Input.GetKey( KeyCode.Mouse1 ) )
         {
            gameObject.transform.RotateAround( gameObject.transform.position, new Vector3( 0.0f, 1.0f, 0.0f ), Input.GetAxis("Mouse X") * Time.deltaTime * _rotatingSpeed );
            _cameraRoot.transform.RotateAround(_cameraRoot.transform.position, gameObject.transform.right, -1.0f * Input.GetAxis("Mouse Y") * Time.deltaTime * _rotatingSpeed );
         }

      if (Input.GetKeyDown(KeyCode.Mouse0)) {
            var forwardVec = _cameraRoot.transform.forward;
            Instantiate(_portalBulletPrefab, _cameraRoot.transform.position + forwardVec * 2, _cameraRoot.transform.rotation);
        }

      }
   }
