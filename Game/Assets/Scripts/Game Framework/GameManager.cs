using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject _playerPrefab;
    private GameObject _playerInstance;
    public GameObject _cameraSetPrefab;
    private GameObject _cameraSetInstance;
    public GameObject _spawnLocation;
    public GameObject _hudPrefab;
    private GameObject _hudInstance;

    public GameObject _gameOverScreenPrefab;
    public GameObject _gameOverScreenInstance;

    public string _endGameCreditSceneName;
	// Use this for initialization
	void Start () {
        if( Init()) {
            StartCoroutine(GameLoop());
        }
    }
	
	// Update is called once per frame
	void Update () {
       
	}

    private bool Init() {
        if (_playerPrefab == null || _spawnLocation == null) {
            Debug.LogError("Need to Specify player or spawn location");
            return false;
        }
        _gameOverScreenInstance.SetActive(false);

        _playerInstance = Instantiate(_playerPrefab, _spawnLocation.transform.position, _spawnLocation.transform.rotation) as GameObject;
        // We don't want to attach the camera set directly because it will make the camera not smooth
        _cameraSetInstance = Instantiate(_cameraSetPrefab, _playerInstance.transform.Find("CameraRoot").position, _playerInstance.transform.Find("CameraRoot").rotation);
        _playerInstance.SendMessage("SetUpCamera", _cameraSetInstance);

        _hudInstance = Instantiate(_hudPrefab);

        var healthBarLogicComp = _hudInstance.transform.Find("HealthUI").transform.Find("HealthBar").GetComponent<HealthBarLogic>();
        healthBarLogicComp._playerStatusComp = _playerInstance.GetComponent<PlayerStatus>();
        return true;
    }

    public void SetGameRuntate(bool isRunning) {
        if (isRunning)
        {
            SetPlayerInput(true);
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            //When the player controller is ready, this line of code will resume user control
            //Player.GetComponent<PonePlayerController> ().enable = true;
        }
        else
        {
            SetPlayerInput(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            // _gameManagerComp.AttachToMainCamera(canvas.gameObject);
            Time.timeScale = 0;
            //When the player controller is ready, this line of code will pause user control
            //Player.GetComponent<PonePlayerController> ().enable = false;

        }
    }

    private IEnumerator GameLoop() {
        yield return GameStart();

        yield return GameRunning();

        yield return GameEnding();
    }

    private IEnumerator GameStart() {
        Debug.Log("Game Start!");
        // Disable portal effect at start
        SetPlayerInput(true);
        SetupPortalSetting();
        yield return new WaitForSeconds(1f);
        Debug.Log("Game Start End!");
    }

    private void SetupPortalSetting() {
        Shader.SetGlobalFloat("_SphereRadius", 0f);
        var goArray = FindObjectsOfType<GameObject>();
        int aLyaer = LayerMask.NameToLayer("WorldA");
        int bLyaer = LayerMask.NameToLayer("WorldB");
        foreach (var go in goArray) {
                if ( ( go.GetComponent<MeshRenderer>() != null || go.GetComponent<SkinnedMeshRenderer>() != null )
                && go.GetComponent<RenderTextureControl>() == null) {
                    go.AddComponent<RenderTextureControl>();
                }
        }
    }

    public void Continue() {
        gameObject.GetComponent<LevelLoading>().StartLoadLevel(SceneManager.GetActiveScene().name);
    }

    private IEnumerator GameRunning() {
        Debug.Log("Game Running!");
        var statusComponent = _playerInstance.GetComponent<PlayerStatus>();
        
        //Keep Looping while the player is alive
        while (statusComponent != null && statusComponent.GetIsAlive()) {
            yield return null;
        }
        Debug.Log("Game Running End!");
    }

    // Bring up game over menu
    private IEnumerator GameEnding() {
        Debug.Log("Game Ending!");
        SetGameRuntate(false);
        _gameOverScreenInstance.SetActive(true);
        while (_gameOverScreenInstance.activeInHierarchy) {
            yield return null;
        }
        // _gameOverScreenInstance = Instantiate(_gameOverScreenPrefab);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_endGameCreditSceneName);
        // Destroy(_gameOverScreenInstance);
        Debug.Log("Game Ending End!");
        yield return null;
    }

    public void SetPlayerInput(bool isEnabled)
    {
        var playerControl = _playerInstance.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        playerControl.enabled = isEnabled;
        // var playerMenuControl = gameObject.GetComponent<PauseMenuController>();
        // playerMenuControl.enabled = isEnabled;
    }
}
