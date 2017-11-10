using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFollow : MonoBehaviour {

    //When landing on a Platform, add its .transform to this.
    public Transform _platform
    {
        set
        {
            platform = value;
            //When we add a new Platform, get its starting position so our calculations correct
            if (platform != null)
            {
                Debug.Log("Attach Platform " + platform.gameObject.name);
                _prevPos = platform.position;
          //      gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_IsOnPlatform = true;
            }
            else
            {
                //gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_IsOnPlatform = false;

            }
        }
        get { return platform; }
    }

    private Transform platform;
    private Vector3 _prevPos = Vector3.zero;

    void LateUpdate()
    {
        //if (platform != null)
        //{
        //    Debug.Log(Platform.position - prePos);
        //    //We are calculating how much the platform moved by subtracting last frame's position, then ADDING it to our player's position.
        //    gameObject.GetComponent<CharacterController>().Move(Platform.position - prePos);
        //    //gameObject.transform.position += Platform.position - prePos;
        //    prePos = Platform.position; //Set prePos for use next frame
        //}
    }

    private void FixedUpdate()
    {
        if (platform != null)
        {
            // Debug.Log(Platform.position - prePos);
            //We are calculating how much the platform moved by subtracting last frame's position, then ADDING it to our player's position.
            // gameObject.GetComponent<CharacterController>().Move(Platform.position - prePos);
            // Debug.Log(gameObject.GetComponent<CharacterController>().isGrounded);
            // gameObject.GetComponent<Rigidbody>().MovePosition(transform.position + Platform.position - prePos);
            gameObject.transform.position += _platform.position - _prevPos;
            _prevPos = _platform.position; //Set prePos for use next frame
        }
    }

}
