using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlockLogic : MonoBehaviour {
    public float _meltDelay;
    public float _particleDelay;
    public GameObject _steamObject;
    public GameObject _idleObject;
    private AudioSource _audioSrcComp;
    private bool _isMelting = false;
	// Use this for initialization
	void Start () {
        _audioSrcComp = gameObject.GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.M)) {
        //    Melt();                                                                
        //}
	}

    public void Melt() {
        if (_isMelting)
        {
            return;
        }
        _isMelting = true;
        StartCoroutine("MeltProcess");
    }

    IEnumerator MeltProcess()
    {
        _steamObject.SetActive(true);
        _audioSrcComp.Play();
        yield return new WaitForSeconds(_meltDelay);
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        float particleTime = 0f;
        while (particleTime < _particleDelay)
        {
            _audioSrcComp.volume = 1f - (particleTime / _particleDelay);
            particleTime += Time.deltaTime;
            yield return null;
        }
        //yield return new WaitForSeconds(_particleDelay);
        _steamObject.SetActive(false);
        _idleObject.GetComponent<ParticleSystem>().Stop();

        yield return null;
    }
}
