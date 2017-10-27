using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalLogic : MonoBehaviour {
    public float _portalMaxRadius = 8f;
    public AnimationCurve _portalRadiusCurve;
    public float _portalRadiusAnimTime;
    public float _portalLifeTime = 3f;
    private float _portalCurrentAnimTime;
    public float _portalCurrentRadius = 0f;
    public bool _active { set; get; }
    private int _worldALayer;
    private int _worldBLayer;
    private int _worldAPortalLayer;
    private int _worldBPortalLayer;
    private Vector3 _targetLocalScale;
    // private Camera _cameraA;
    // private Camera _cameraB;
    private GameObject _player;
    private AudioSource _audioSrc;
    private AudioSource _enterAudioSrc;
    private AudioSource _leaveAudioSrc;
    public AudioClip _startSound;
    public AudioClip _idleSound;
    public AudioClip _passThroughSound;
    private int _prevEnterColliderID;
    private int _prevLeaveColliderID;
    // Use this for initialization
    void Start () {
        _worldALayer = LayerMask.NameToLayer("WorldA");
        _worldBLayer = LayerMask.NameToLayer("WorldB");
        _worldAPortalLayer = LayerMask.NameToLayer("WorldAInPortal");
        _worldBPortalLayer = LayerMask.NameToLayer("WorldBInPortal");
        _player = GameObject.FindGameObjectWithTag("Player");

        _targetLocalScale = transform.localScale;
        Shader.SetGlobalVector("_SphereCenter", gameObject.transform.position);
        var audioResources = gameObject.GetComponents<AudioSource>();
        _audioSrc = audioResources[0];
        _enterAudioSrc = audioResources[1];
        _leaveAudioSrc = audioResources[2];
        _enterAudioSrc.clip = _passThroughSound;
        _leaveAudioSrc.clip = _passThroughSound;
        StartCoroutine("PortalLifeCircle");
        // _cameraA = GameObject.Find("CameraA").GetComponent<Camera>();
        // _cameraB = GameObject.Find("CameraB").GetComponent<Camera>();
        
    }

    IEnumerator PortalLifeCircle() {
        yield return Expand();
        yield return Idle();
        yield return Shrink();
    }
    // Init and expanding animation
    IEnumerator Expand() {
        _audioSrc.clip = _startSound;
        _audioSrc.loop = true;
        _audioSrc.pitch = 1f;
        _audioSrc.volume = 1f;
        _audioSrc.Play();
        _portalCurrentAnimTime = 0f;
        while (_portalCurrentAnimTime < 1f) {
            _portalCurrentAnimTime += Time.deltaTime / _portalRadiusAnimTime;
            _portalCurrentRadius = _portalRadiusCurve.Evaluate(_portalCurrentAnimTime) * _portalMaxRadius;
            Shader.SetGlobalFloat("_SphereRadius", _portalCurrentRadius);
            _targetLocalScale = new Vector3(_portalCurrentRadius, _portalCurrentRadius, _portalCurrentRadius);
            yield return null;
        }
    }

    IEnumerator Idle(){
        _audioSrc.clip = _idleSound;
        _audioSrc.loop = true;
        _audioSrc.pitch = 1f;
        _audioSrc.volume = 1f;
        _audioSrc.Play();
        while (_active) {
            yield return null;
        }
        _audioSrc.Stop();
        // yield return new WaitForSeconds(_portalLifeTime);
    }

    IEnumerator Shrink(){
        _audioSrc.clip = _startSound;
        _audioSrc.pitch = 0.5f;
        _audioSrc.loop = false;
        _audioSrc.volume = 1f;
        _audioSrc.Play();
        _portalCurrentAnimTime = 0f;
        while (_portalCurrentAnimTime < 1f)
        {
            _portalCurrentAnimTime += Time.deltaTime / _portalRadiusAnimTime;
            _portalCurrentRadius = _portalRadiusCurve.Evaluate(1f - _portalCurrentAnimTime) * _portalMaxRadius;
            Shader.SetGlobalFloat("_SphereRadius", _portalCurrentRadius);
            _targetLocalScale = new Vector3(_portalCurrentRadius, _portalCurrentRadius, _portalCurrentRadius);
            yield return null;
        }
        ClosePortal();
    }

    void ClosePortal() {
        Shader.SetGlobalFloat("_SphereRadius", 0f);
        // var collider = gameObject.GetComponent<SphereCollider>();
        var overlappers = Physics.OverlapSphere(gameObject.transform.position, 0.1f * _portalCurrentRadius);
        // Only alow switch when the player is not in overlapp with object in another world
        foreach (var overlap in overlappers)
        {
           // Debug.Log("reset " + overlap.gameObject.name);
            UpdateNonPlayerGOLeavePortal(overlap.gameObject);
        }
        var playerGo = GameObject.FindGameObjectWithTag("Player");
        if (playerGo != null 
            && playerGo.GetComponent<PlayerStatus>()._currentPortal != null
            && playerGo.GetComponent<PlayerStatus>()._currentPortal.GetInstanceID() == gameObject.GetInstanceID()) {
            playerGo.GetComponent<PlayerStatus>()._currentPortal = null;
        }
        Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    private void FixedUpdate()
    {
        transform.localScale = _targetLocalScale;

    }

    void SwapLayer(GameObject go) {
        if (go.layer == _worldALayer){
            go.layer = _worldBLayer;
        }
        else if (go.layer == _worldBLayer){
            go.layer = _worldALayer;
        }
    }

    void OnTriggerEnter(Collider other) {
        //    Debug.Log("enter portal");
        Debug.Log(other.gameObject.name);
        if (_prevEnterColliderID != other.gameObject.GetInstanceID()) {
            _prevEnterColliderID = other.gameObject.GetInstanceID();
            if (_prevLeaveColliderID == _prevEnterColliderID)
            {
                _prevLeaveColliderID = 0;
            }
            _enterAudioSrc.pitch = 1.58f;
            //   _enterAudioSrc.loop = false;
            _enterAudioSrc.volume = 1f;
            _enterAudioSrc.Play();
        }
        

        if (_player.GetInstanceID() == other.gameObject.GetInstanceID())
        {
            other.gameObject.GetComponent<WorldSwitch>().SetPortalStatus(true);
        }
        else
        {
            UpdateNonPlayerGOInPortal(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other){
       // Debug.Log(other.gameObject.name + " stay test");
        if (_player.GetInstanceID() == other.gameObject.GetInstanceID())
        {
            other.gameObject.GetComponent<WorldSwitch>().SetPortalStatus(true);
        }
        else
        {
            UpdateNonPlayerGOInPortal(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other){
        Debug.Log("Leave");
        if (_prevLeaveColliderID != other.gameObject.GetInstanceID()) {
            _prevLeaveColliderID = other.gameObject.GetInstanceID();
            if (_prevLeaveColliderID == _prevEnterColliderID) {
                _prevEnterColliderID = 0;
            }
            _leaveAudioSrc.pitch = 0.8f;
            _leaveAudioSrc.loop = false;
            _leaveAudioSrc.volume = 1f;
            _leaveAudioSrc.Play();
        }
        if (_player.GetInstanceID() == other.gameObject.GetInstanceID())
        {
            other.gameObject.GetComponent<WorldSwitch>().SetPortalStatus(false);
        }
        else
        {
            UpdateNonPlayerGOLeavePortal(other.gameObject);
        }
    }

    void UpdateNonPlayerGOInPortal(GameObject go) {
       // Debug.Log(go.name + "testing in portal");
        if (_player.GetInstanceID() != go.gameObject.GetInstanceID())
        {
            if (go.layer == _worldALayer || go.layer == _worldBLayer)
            {
                if (go.tag.Equals("Transportable"))
                {
                    // Both go and player inside portal, make them in same world
                    if (_player.GetComponent<WorldSwitch>()._insidePortal)
                    {
                        go.layer = _player.layer == _worldALayer ? _worldAPortalLayer : _worldBPortalLayer;
                        // other.gameObject.layer = _player.layer == _worldALayer ? _worldBLayer : _worldALayer;
                    }// player outside and go inside, make them in different world
                    else
                    {
                        go.layer = _player.layer == _worldALayer ? _worldBPortalLayer : _worldAPortalLayer;
                    }
                }
                else
                {
                    //if (_player.GetComponent<WorldSwitch>()._insidePortal && go.layer == _player.layer)
                    //{
                    //    go.layer = _player.layer == _worldALayer ? _worldAPortalLayer : _worldBPortalLayer;
                    //}// player outside and go inside, make them in different world
                    //else if (!_player.GetComponent<WorldSwitch>()._insidePortal && go.layer != _player.layer)
                    //{
                    //    go.layer = _player.layer == _worldALayer ? _worldBPortalLayer : _worldAPortalLayer;
                    //}

                    if (_player.GetComponent<WorldSwitch>()._insidePortal)
                    {
                        go.layer = go.layer == _worldALayer ? _worldAPortalLayer : _worldBPortalLayer;
                    }// player outside and go inside, make them in different world
                    else if (!_player.GetComponent<WorldSwitch>()._insidePortal)
                    {
                        go.layer = go.layer == _worldALayer ? _worldAPortalLayer : _worldBPortalLayer;
                    }
                }
                   
            }
            else if (go.layer == _worldAPortalLayer || go.layer == _worldBPortalLayer)
            {
                if (go.tag.Equals("Transportable"))
                {
                    if (_player.GetComponent<WorldSwitch>()._insidePortal) {
                        go.layer = _player.layer == _worldALayer ? _worldAPortalLayer : _worldBPortalLayer;
                    }
                    else {
                        go.layer = _player.layer == _worldALayer ? _worldBPortalLayer : _worldAPortalLayer;
                    }
                }
                else {
                    // If the rigid body is shared by two meshes, things get quite tricky
                    // because you cannot dicide it's world based on player's world anymore
                    // or you'll move the child mesh around the world
                    //if (_player.GetComponent<WorldSwitch>()._insidePortal){
                    //    if ((_player.layer == _worldALayer && go.layer == _worldBPortalLayer)
                    //        || (_player.layer == _worldBLayer && go.layer == _worldAPortalLayer)){
                    //        go.layer = go.layer == _worldAPortalLayer ? _worldALayer : _worldBLayer;
                    //    }
                    //}
                    //else {
                    //    if ( (_player.layer == _worldALayer && go.layer == _worldAPortalLayer)
                    //        || (_player.layer == _worldBLayer && go.layer == _worldBPortalLayer) ) {
                    //        go.layer = go.layer == _worldAPortalLayer ? _worldALayer : _worldBLayer;
                    //    }
                            
                    //}
                }
                
            }
            //else {
                for (int i = 0; i < go.transform.childCount; ++i)
                {
                    // Debug.Log(go.transform.GetChild(i).gameObject.name);
                    UpdateNonPlayerGOInPortal(go.transform.GetChild(i).gameObject);
                }
           // }
            
        }
    }

    void UpdateNonPlayerGOLeavePortal(GameObject go){
      //  Debug.Log(go.name + "out portal");
        if (_player.GetInstanceID() != go.GetInstanceID() && (go.layer == _worldAPortalLayer || go.layer == _worldBPortalLayer))
        {
            if (go.tag.Equals("Transportable"))
            {
                // player insdie portal, then set it to outside environment
                if (_player.GetComponent<WorldSwitch>()._insidePortal) {
                    go.layer = _player.layer == _worldALayer ? _worldBLayer : _worldALayer;
                }
                else if (!_player.GetComponent<WorldSwitch>()._insidePortal) {
                    go.layer = _player.layer;
                }
            }
            else {
                // Return it to it's original world, don't be influenced by player world
                go.layer = go.layer == _worldAPortalLayer ? _worldALayer : _worldBLayer;
            }
            
        }
        for (int i = 0; i < go.transform.childCount; ++i)
        {
               // Debug.Log(go.transform.GetChild(i).gameObject.name);
                UpdateNonPlayerGOLeavePortal(go.transform.GetChild(i).gameObject);
        }
    }
}
