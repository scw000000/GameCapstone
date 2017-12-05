using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionListener : MonoBehaviour
{
    public string[] _destructableGONames;
    public GameObject[] _eventObjects;
    private AudioSource _triggerSoundSrc;
    // Use this for initialization
    void Start()
    {
        _triggerSoundSrc = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (var name in _destructableGONames)
        {
            if (name.Equals(collision.gameObject.name))
            {
                collision.transform.parent.gameObject.SendMessage("ResetPhysics");
                foreach (var go in _eventObjects)
                {
                    if (go != null)
                    {
                        go.SetActive(true);
                        gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
        
    }
}
