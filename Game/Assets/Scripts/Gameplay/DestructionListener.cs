using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionListener : MonoBehaviour
{
    public string[] _destructableGONames;
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
                StartCoroutine("DeactiveAfterPlayingSound");
            }
        }
        
    }

    IEnumerator DeactiveAfterPlayingSound()
    {
        if (_triggerSoundSrc == null)
        {
            gameObject.SetActive(false);
            yield break;
        }
        _triggerSoundSrc.Play();
        while (_triggerSoundSrc.isPlaying)
        {
            yield return null;
        }

        gameObject.SetActive(false);
        yield return null;

    }
}
