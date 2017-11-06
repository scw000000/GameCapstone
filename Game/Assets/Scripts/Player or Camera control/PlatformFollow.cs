using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFollow : MonoBehaviour {

    //When landing on a Platform, add its .transform to this.
    public Transform Platform
    {
        set
        {
            platform = value;
            //When we add a new Platform, get its starting position so our calculations correct
            if (platform != null)
            {
                prePos = platform.position;
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
    private Vector3 prePos = Vector3.zero;

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
            Debug.Log(Platform.position - prePos);
            //We are calculating how much the platform moved by subtracting last frame's position, then ADDING it to our player's position.
            gameObject.GetComponent<CharacterController>().Move(Platform.position - prePos);
            //gameObject.transform.position += Platform.position - prePos;
            prePos = Platform.position; //Set prePos for use next frame
        }
    }

}
