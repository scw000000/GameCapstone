﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject _playerPrefab;
    private GameObject _playerInstance;
    public GameObject _cameraSetPrefab;
    private GameObject _cameraSetInstance;
    public GameObject _spawnLocation;

    public GameObject _gameOverScreenPrefab;
    private GameObject _gameOverScreenInstance;

    public string _endGameCreditSceneName;
	// Use this for initialization
	void Start () {
        if( SpawnPlayer()) {
            StartCoroutine(GameLoop());
        }
    }
	
	// Update is called once per frame
	void Update () {
       
	}

    private bool SpawnPlayer() {
        if (_playerPrefab == null || _spawnLocation == null) {
            Debug.LogError("Need to Specify player or spawn location");
            return false;
        }
        _playerInstance = Instantiate(_playerPrefab, _spawnLocation.transform.position, _spawnLocation.transform.rotation) as GameObject;
        // We don't want to attach the camera set directly because it will make the camera not smooth
        _cameraSetInstance = Instantiate(_cameraSetPrefab, _playerInstance.transform.Find("CameraRoot").position, _playerInstance.transform.Find("CameraRoot").rotation);
        _playerInstance.SendMessage("SetUpCamera", _cameraSetInstance);
        return true;
    }

    public void PauseGame() {
    }

    public void ResumeGame() {
    }

    private IEnumerator GameLoop() {
        yield return GameStart();

        yield return GameRunning();

        yield return GameEnding();
    }

    private IEnumerator GameStart() {
        Debug.Log("Game Start!");
        // Disable portal effect at start

        SetupPortalSetting();
        yield return new WaitForSeconds(1f);
        Debug.Log("Game Start End!");
    }

    private void SetupPortalSetting() {
        Shader.SetGlobalFloat("_SphereRadius", 0f);
        var goArray = FindObjectsOfType<GameObject>();
        foreach (var go in goArray) {
                if (go.GetComponent<MeshRenderer>() != null && go.GetComponent<RenderTextureControl>() == null) {
                    go.AddComponent<RenderTextureControl>();
                }
        }
    }

    private IEnumerator GameRunning() {
        Debug.Log("Game Running!");
        var statusComponent = _playerInstance.GetComponent<PlayerStatus>();
        
        //Keep Looping while the player is alive
        while (statusComponent != null && statusComponent.IsAlive()) {
            yield return null;
        }
        Debug.Log("Game Running End!");
    }

    // Bring up game over menu
    private IEnumerator GameEnding() {
        Debug.Log("Game Ending!");
        _gameOverScreenInstance = Instantiate(_gameOverScreenPrefab);
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_endGameCreditSceneName);
        Destroy(_gameOverScreenInstance);
        Debug.Log("Game Ending End!");
    }
}
