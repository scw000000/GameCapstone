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

        if (gameObject.layer == LayerMask.NameToLayer("Defult")
            || gameObject.layer == other.gameObject.layer
            || ( gameObject.layer == LayerMask.NameToLayer("WorldAInPortal") && other.gameObject.layer == LayerMask.NameToLayer("WorldB") )
            || (gameObject.layer == LayerMask.NameToLayer("WorldBInPortal") && other.gameObject.layer == LayerMask.NameToLayer("WorldA"))
                )
        {

            gameObject.GetComponent<Collider>().enabled = false;
            _triggerable = false;
            _doorGO.GetComponent<DoorControl>()._isOpened = true;
            if (gameObject.GetComponent<AudioSource>() != null)
            {
                gameObject.GetComponent<AudioSource>().Play();
                StartCoroutine("ActivateUntilAudioPlayed");
            }
        }
    }

    private IEnumerator ActivateUntilAudioPlayed() {
        var audioSource = gameObject.GetComponent<AudioSource>();
        while (audioSource.isPlaying) {
            yield return null;
        }
        for (int i = 0; i < gameObject.transform.childCount; ++i) {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        // gameObject.SetActive(false);
    }
}
