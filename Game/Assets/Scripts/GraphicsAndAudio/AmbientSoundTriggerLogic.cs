using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundTriggerLogic : MonoBehaviour {
    public AudioClip _worldAAmbientSound;
    public AudioClip _worldBAmbientSound;
    private GameObject _ambientSoundGO;
    public float _mixSoundPeriod = 4f;
    private bool _isSwitchingSound = false;
    // Use this for initialization
    void Start () {
        _ambientSoundGO = GameObject.Find("AmbientSoundControl");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Player"))
        {
            return;
        }

        var soundControl = _ambientSoundGO.GetComponent<AmbientSoundControl>();
        // Both source should be null
        if (soundControl._worldAAudio.clip == null )
        {
            SwithWorldSound(soundControl._worldAAudio, _worldAAmbientSound);
            SwithWorldSound(soundControl._worldBAudio, _worldBAmbientSound);
            return;
        }

        if (!_isSwitchingSound && soundControl._worldAAudio.clip.GetInstanceID() !=
           _worldAAmbientSound.GetInstanceID())
        {
            StartCoroutine("MixAmbientSound");
        }
        else if (!_isSwitchingSound && soundControl._worldBAudio.clip.GetInstanceID() !=
           _worldBAmbientSound.GetInstanceID())
        {
            StartCoroutine("MixAmbientSound");
        }
    }

    private void SwithWorldSound( AudioSource source, AudioClip to )
    {
        source.clip = to;
        source.Play();
    }

    private IEnumerator MixAmbientSound()
    {
        _isSwitchingSound = true;
        float currTime = 0f;
        var soundControl = _ambientSoundGO.GetComponent<AmbientSoundControl>();
        float prevWorldAVolume = soundControl._worldAAudio.volume;
        float prevWorldBVolume = soundControl._worldBAudio.volume;

        bool needSwitchWorldASound = soundControl._worldAAudio.clip.GetInstanceID() != _worldAAmbientSound.GetInstanceID();
        bool needSwitchWorldBSound = soundControl._worldBAudio.clip.GetInstanceID() != _worldBAmbientSound.GetInstanceID();

        while (currTime < _mixSoundPeriod)
        {
            currTime += Time.deltaTime;
            if (needSwitchWorldASound)
            {
                soundControl._worldAAudio.volume = Mathf.Lerp(prevWorldAVolume, 0, currTime / _mixSoundPeriod );
            }
            if (needSwitchWorldBSound)
            {
                soundControl._worldBAudio.volume = Mathf.Lerp(prevWorldBVolume, 0, currTime / _mixSoundPeriod);
            }
            yield return null;
        }

        if (needSwitchWorldASound)
        {
            SwithWorldSound(soundControl._worldAAudio, _worldAAmbientSound);
            soundControl._worldAAudio.volume = prevWorldAVolume;
        }
        if (needSwitchWorldBSound)
        {
            SwithWorldSound(soundControl._worldBAudio, _worldBAmbientSound);
            soundControl._worldBAudio.volume = prevWorldBVolume;
        }
        _isSwitchingSound = false;
        yield return null;
        
    }
}
