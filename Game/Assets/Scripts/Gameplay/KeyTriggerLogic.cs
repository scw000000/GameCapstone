using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTriggerLogic : MonoBehaviour {
    public GameObject _doorGO;
    private bool _triggerable;
    // Use this for initialization
    void Start() {
        _triggerable = true;
    }
        // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (_doorGO == null) {
            return;
        }
        if (!other.tag.Equals("Player")) {
            return;
        }
        if (!_triggerable) {
            return;
        }
        _triggerable = false;
        _doorGO.GetComponent<DoorControl>()._isOpened = true;
        if (gameObject.GetComponent<AudioSource>()!= null) {
            gameObject.GetComponent<AudioSource>().Play();
            StartCoroutine("ActivateUntilAudioPlayed");
        }
    }

    private IEnumerator ActivateUntilAudioPlayed() {
        var audioSource = gameObject.GetComponent<AudioSource>();
        while (audioSource.isPlaying) {
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
