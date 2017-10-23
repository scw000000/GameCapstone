using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject _playerInstance;
    public GameObject _cameraSetInstance;
    public GameObject[] _spawnLocations;
    public GameObject _hudInstance;
    
    public GameObject _gameOverScreenInstance;

    public string _endGameCreditSceneName;

    private GameObject _gameMessagePanelGO;
    private GameObject _systemMessagePanelGO;

    private Text _gameMessageText;
    private Text _systemMessageText;
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
        if (_spawnLocations == null) {
            Debug.LogError("Need to Specify player or spawn location");
            return false;
        }
        GameObject spawnLoc = null;
        int progress = 0;
        // Detect if we should load game data
        int loadSlot = PlayerPrefs.GetInt(GameCapstone.SaveData._loadPrefName);
        int saveSlotValid = PlayerPrefs.GetInt(GameCapstone.SaveData._saveSlotValidPrefName + loadSlot);
        if (loadSlot >= 0 && saveSlotValid > 0)
        {
            var saveData = BayatGames.SaveGameFree.SaveGame.Load<GameCapstone.SaveData>(
                GameCapstone.SaveData._saveSlotPrefName + loadSlot,
                new GameCapstone.SaveData()
                );
            Debug.Log("Progress is: " + saveData._currentProgress);
            if (saveData._currentProgress < _spawnLocations.Length)
            {
                progress = saveData._currentProgress;
                spawnLoc = _spawnLocations[saveData._currentProgress];
            }
            else // prevent array idx out of bound
            {
                spawnLoc = _spawnLocations[0];
                progress = 0;
            }
        }
        else
        {
            Debug.Log("Skip loading game");
            spawnLoc = _spawnLocations[0];
            progress = 0;
        }

        _gameOverScreenInstance.SetActive(false);

        // _playerInstance = Instantiate(_playerPrefab, spawnLoc.transform.position, spawnLoc.transform.rotation) as GameObject;
        _playerInstance.transform.position = spawnLoc.transform.position;
        _playerInstance.transform.rotation = spawnLoc.transform.rotation;
        _playerInstance.GetComponent<PlayerStatus>()._currentProgress = progress;
        // We don't want to attach the camera set directly because it will make the camera not smooth
        // _cameraSetInstance = Instantiate(_cameraSetPrefab, _playerInstance.transform.Find("CameraRoot").position, _playerInstance.transform.Find("CameraRoot").rotation);
        var camRootGO = _playerInstance.transform.Find("CameraRoot").gameObject;
        _cameraSetInstance.transform.position = camRootGO.transform.position;
        _cameraSetInstance.transform.rotation = camRootGO.transform.rotation;
        _cameraSetInstance.SetActive(true);
        _playerInstance.SendMessage("SetUpCamera", _cameraSetInstance);

        // Setup HUD objects
        var healthBarLogicComp = _hudInstance.transform.Find("HealthUI").transform.Find("HealthBar").GetComponent<HealthBarLogic>();
        healthBarLogicComp._playerStatusComp = _playerInstance.GetComponent<PlayerStatus>();

        _gameMessagePanelGO = _hudInstance.transform.Find("GameMessagePanel").gameObject;
        _systemMessagePanelGO = _hudInstance.transform.Find("SystemMessagePanel").gameObject;
        
        return true;
    }

    public void SetGameRunState(bool isRunning) {
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
        // int aLyaer = LayerMask.NameToLayer("WorldA");
        // int bLyaer = LayerMask.NameToLayer("WorldB");
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
        SetGameRunState(false);
        _gameOverScreenInstance.SetActive(true);
        // Prevent pause is being hit when gameover
        gameObject.GetComponent<PauseMenuController>().enabled = false;
        while (_gameOverScreenInstance.activeInHierarchy) {
            yield return null;
        }
        // _gameOverScreenInstance.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_endGameCreditSceneName);
        Debug.Log("Game Ending End!");
        yield return null;
    }

    public void SetPlayerInput(bool isEnabled)
    {
        var playerControl = _playerInstance.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        playerControl.enabled = isEnabled;
        _playerInstance.GetComponent<ShootingLogic>().enabled = isEnabled;
        _playerInstance.GetComponent<WorldSwitch>().enabled = isEnabled;
        // var playerMenuControl = gameObject.GetComponent<PauseMenuController>();
        // playerMenuControl.enabled = isEnabled;
    }

    public void DisplayGameMessage(string msg, float time) {
        _gameMessagePanelGO.GetComponent<MessageAnimationLogic>().DisplayMessage(msg, time);
    }

    public void DisplayHintMessage(string msg, float time) {
        var msgAnimLogic = _gameMessagePanelGO.GetComponent<MessageAnimationLogic>();
        msgAnimLogic.DisplayMessage(msg, time, "QuestionMark");
    }

    public void DisplayNotifyMessage(string msg, float time)
    {
        var msgAnimLogic = _gameMessagePanelGO.GetComponent<MessageAnimationLogic>();
        msgAnimLogic.DisplayMessage(msg, time, "ExclamationMark");
    }

    public void DisplaySystemMessage(string msg, float time)
    {
        _systemMessagePanelGO.GetComponent<MessageAnimationLogic>().DisplayMessage(msg, time);
    }

    public void TerminateGameMessage(bool isClearAll)
    {
        if (isClearAll)
        {
            _gameMessagePanelGO.GetComponent<MessageAnimationLogic>().ClearAllDisplay();
        }
        else
        {
            _gameMessagePanelGO.GetComponent<MessageAnimationLogic>().TerminateDisplay();
        }
    }

    public void TerminateSystemMessage(bool isClearAll)
    {

        if (isClearAll)
        {
            _systemMessagePanelGO.GetComponent<MessageAnimationLogic>().ClearAllDisplay();
        }
        else
        {
            _systemMessagePanelGO.GetComponent<MessageAnimationLogic>().TerminateDisplay();
        }
    }
}
