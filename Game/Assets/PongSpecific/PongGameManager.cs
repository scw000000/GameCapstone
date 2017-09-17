using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongGameManager : MonoBehaviour {
    public GameObject BallPrefab;
    public GameObject PlayerPrefab;
    public GameObject CameraPrefab;
    public GameObject EnemyPrefab;
    public GameObject HUDPrefab;
    private GameObject CameraInstance;
    private GameObject PlayerInstance;
    private GameObject EnemyInstance;
    private GameObject BallInstance;
    private GameObject HUDInstance;
    private UnityEngine.UI.Text PlayerScoreText;
    private UnityEngine.UI.Text AIScoreText;
    private UnityEngine.UI.Text ScoreMessageText;

    private GameObject PlayerCameraInstance;
    public GameObject PlayerSpawnLocation;
    public GameObject EnemySpawnLocation;
    public GameObject BallSpawnLocation;
    public GameObject CountDownPrefab;
    private GameObject CountDownInstance;

    public GameObject GameOverScreenPrefab;
    private GameObject GameOverScreenInstance;

    public string EndGameCreditSceneName;

    private int PlayerScore = 0;
    private int AIScore = 0;
    private bool Scored = false;
    // Use this for initialization
    void Start()
    {
        // Fist Set score to 0, might have to update it later when loading save data
        PlayerScore = 0;
        AIScore = 0;

        if (SpawnPlayer())
        {
            StartCoroutine(GameLoop());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool SpawnPlayer()
    {
        if (PlayerPrefab == null || PlayerSpawnLocation == null)
        {
            Debug.LogError("Need to Specify player or spawn location");
            return false;
        }
        PlayerInstance = Instantiate(PlayerPrefab, PlayerSpawnLocation.transform.position, PlayerSpawnLocation.transform.rotation) as GameObject;
        //PlayerCameraInstance = PlayerInstance.transform.Find("Camera").gameObject;
        PlayerCameraInstance = Instantiate(CameraPrefab);
        // var camFollowComp = PlayerCameraInstance.GetComponent<CameraFollow>();
        var imgCapComp = PlayerInstance.GetComponent<ImageCapture>();
        imgCapComp.RenderCamera = PlayerCameraInstance.GetComponent<Camera>();
        var videoCapCtrlComp = PlayerInstance.GetComponent<RockVR.Video.VideoCaptureCtrl>();
        videoCapCtrlComp.videoCaptures[0] = PlayerCameraInstance.GetComponent<RockVR.Video.VideoCapture>();
        var videoCapComp = PlayerInstance.GetComponent<VideoCapture>();
        videoCapComp.MainCamera = PlayerCameraInstance;
        // camFollowComp._followingRoot = PlayerInstance.transform.Find("CameraRoot").gameObject;
        // Disable movement until round start;
        SetCamFollowSpeed(0f);
        if (EnemyPrefab == null || EnemySpawnLocation == null)
        {
            Debug.LogError("Need to Specify enemy or spawn location");
            return false;
        }
        EnemyInstance = Instantiate(EnemyPrefab, EnemySpawnLocation.transform.position, EnemySpawnLocation.transform.rotation) as GameObject;

        BallInstance = Instantiate(BallPrefab);
        BallInstance.GetComponent<Ball>().AttachRoot = PlayerInstance.transform.Find("BallAttachRoot").gameObject;
        return true;
    }

    private void SetCamFollowSpeed(float newSpeed) {
        var camFollowComp = PlayerCameraInstance.GetComponent<CameraFollow>();
        camFollowComp._followingSpeed = newSpeed;
    }

    public void PauseGame()
    {
    }

    public void ResumeGame()
    {
    }

    public void Score(bool isPlayerScore) {
        
        Scored = true;
        if (isPlayerScore) {
            ++PlayerScore;
            ScoreMessageText.text = "Player Scored!";
            Debug.Log("Player Scored!");
        }
        else {
            ++AIScore;
            ScoreMessageText.text = "AI Scored!";
            Debug.Log("AI Scored!");
        }
        ScoreMessageText.enabled = true;
        UpdateSocreBorad();
    }

    public void UpdateSocreBorad() {
        PlayerScoreText.text = "Player: " + PlayerScore;
        AIScoreText.text = "AI: " + AIScore;
    }

    public void SetPlayerInput(bool isEnabled) {
        var playerControl = PlayerInstance.GetComponent<PongPlayerControl>();
        playerControl.enabled = isEnabled;

        var playerMenuControl = gameObject.GetComponent<PauseMenuController>();
        playerMenuControl.enabled = isEnabled;

        BallInstance.GetComponent<Ball>()._isLaunchable = isEnabled;
    }

    public void Quit()
    {
        SaveGame();
        Destroy(PlayerInstance);
        Destroy(EnemyInstance);
    }

    public void SaveGame()
    {
        //PlayerPrefs.SetFloat("PlayerX", PlayerInstance.transform.position.x);
        //PlayerPrefs.SetFloat("PlayerY", PlayerInstance.transform.position.y);
        //PlayerPrefs.SetFloat("PlayerZ", PlayerInstance.transform.position.z);
        //PlayerPrefs.SetFloat("EnemyX", EnemyInstance.transform.position.x);
        //PlayerPrefs.SetFloat("EnemyY", EnemyInstance.transform.position.y);
        //PlayerPrefs.SetFloat("EnemyZ", EnemyInstance.transform.position.z);

        PlayerPrefs.SetInt("PlayerScore", PlayerScore);
        PlayerPrefs.SetInt("AIScore", AIScore);
    }
    public void LoadGame()
    {
        //PlayerInstance.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ"));
        //EnemyInstance.transform.position = new Vector3(PlayerPrefs.GetFloat("EnemyX"), PlayerPrefs.GetFloat("EnemyY"), PlayerPrefs.GetFloat("EnemyZ"));

        PlayerScore = PlayerPrefs.GetInt("PlayerScore");
        AIScore = PlayerPrefs.GetInt("AIScore");
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(GameStart());

        yield return StartCoroutine(GameRunning());

        yield return StartCoroutine(GameEnd());
    }

    private IEnumerator GameStart()
    {
        Debug.Log("Game Start!");
        LoadGame();
        SetPlayerInput(false);
        // Attach HUD
        HUDInstance = Instantiate(HUDPrefab, PlayerCameraInstance.transform.position, PlayerCameraInstance.transform.rotation);
        HUDInstance.transform.SetParent(PlayerCameraInstance.transform, false);
        PlayerScoreText = HUDInstance.transform.Find("PlayerScore").GetComponent<UnityEngine.UI.Text>();
        AIScoreText = HUDInstance.transform.Find("AIScore").GetComponent<UnityEngine.UI.Text>();
        ScoreMessageText = HUDInstance.transform.Find("ScoreMessage").GetComponent<UnityEngine.UI.Text>();
        ScoreMessageText.enabled = false;
        UpdateSocreBorad();
        Debug.Log("Game Start End!");
        yield return null;
    }

    private IEnumerator GameRunning()
    {
        Debug.Log("Game Running!");
        var statusComponent = PlayerInstance.GetComponent<PongPlayerState>();

        //Keep Looping the status of the player
        while (statusComponent != null && statusComponent.IsAlive())
        {
            yield return RoundStart();
            yield return RoundRunning();
            yield return RoundEnd();
            yield return null;
        }
        Debug.Log("Game Running End!");
    }

    // Bring up game over menu
    private IEnumerator GameEnd()
    {
        Debug.Log("Game Ending!");
        GameOverScreenInstance = Instantiate(GameOverScreenPrefab);
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(EndGameCreditSceneName);
        Destroy(GameOverScreenInstance);
        Debug.Log("Game Ending End!");
    }

    private void InitializeTransform() {
        PlayerInstance.transform.position = PlayerSpawnLocation.transform.position;
        PlayerInstance.transform.rotation = PlayerSpawnLocation.transform.rotation;

        EnemyInstance.transform.position = EnemySpawnLocation.transform.position;
        EnemyInstance.transform.rotation = EnemySpawnLocation.transform.rotation;
    }

    private IEnumerator RoundStart()
    {
        Scored = false;
        SetCamFollowSpeed(1f);
        // Wait until transion ends
        var FadeInComp = PlayerCameraInstance.GetComponent<FadeInEffect>();
        Debug.Log("Transition start");
        FadeInComp._currentTime = 0f;
        FadeInComp._updating = true;
        FadeInComp._reverse = false;
        yield return new WaitForSeconds(FadeInComp._transitionTime);
        FadeInComp._updating = false;

        Debug.Log("Trans time: " + FadeInComp._transitionTime);
        yield return new WaitForSeconds(FadeInComp._transitionTime);
        Debug.Log("Transition end");

        // Starting countdown
        CountDownInstance = Instantiate(CountDownPrefab, PlayerCameraInstance.transform.position, PlayerCameraInstance.transform.rotation);
        CountDownInstance.transform.SetParent(PlayerCameraInstance.transform, false);
        var textComp = CountDownInstance.transform.Find("Text").GetComponent<UnityEngine.UI.Text>();
        textComp.text = "3";
        Debug.Log("3");
        yield return new WaitForSeconds(1f);
        textComp.text = "2";
        Debug.Log("2");
        yield return new WaitForSeconds(1f);
        textComp.text = "1";
        Debug.Log("1");
        yield return new WaitForSeconds(1f);
        Debug.Log("GO!");
        textComp.text = "GO!";
        SetCamFollowSpeed(10f);
        SetPlayerInput(true);
        yield return new WaitForSeconds(1f);
        Destroy(CountDownInstance);
    }

    private IEnumerator RoundRunning()
    {
        while (true) {
            // loop until someone scored
            if (Scored || Input.GetKeyDown(KeyCode.T)) {
                SetPlayerInput(false);
                break;
            }
            yield return null;
        }
    }

    private IEnumerator RoundEnd()
    {
        // Wait until transion ends
        var FadeInComp = PlayerCameraInstance.GetComponent<FadeInEffect>();
        Debug.Log("Transition start");
        FadeInComp._currentTime = 0f;
        FadeInComp._updating = true;
        FadeInComp._reverse = true;
        yield return new WaitForSeconds(FadeInComp._transitionTime);
        // Reset player, AI and ball transform
        // Do it in here because player won't see it
        InitializeTransform();
        FadeInComp._updating = false;
        BallInstance.SendMessage("Respawn");
        ScoreMessageText.enabled = false;
        Debug.Log("Transition end");
        yield return new WaitForSeconds(0.5f);
        PlayerCameraInstance.transform.position = Vector3.zero;
        PlayerCameraInstance.transform.rotation = Quaternion.identity;
    }

}
