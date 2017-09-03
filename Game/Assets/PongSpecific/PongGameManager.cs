using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongGameManager : MonoBehaviour {
    public GameObject BallPrefab;
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;
    private GameObject PlayerInstance;
    private GameObject EnemyInstance;
    private GameObject PlayerCameraInstance;
    public GameObject PlayerSpawnLocation;
    public GameObject EnemySpawnLocation;
    public GameObject BallSpawnLocation;
    public GameObject CountDownPrefab;
    private GameObject CountDownInstance;

    public GameObject GameOverScreenPrefab;
    private GameObject GameOverScreenInstance;

    public string EndGameCreditSceneName;
    // Use this for initialization
    void Start()
    {
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
        PlayerCameraInstance = PlayerInstance.transform.Find("Camera").gameObject;
        if (EnemyPrefab == null || EnemySpawnLocation == null)
        {
            Debug.LogError("Need to Specify enemy or spawn location");
            return false;
        }
        EnemyInstance = Instantiate(EnemyPrefab, EnemySpawnLocation.transform.position, EnemySpawnLocation.transform.rotation) as GameObject;
        return true;
    }

    public void PauseGame()
    {
    }

    public void ResumeGame()
    {
    }

    public void Quit()
    {
        SaveGame();
        Destroy(PlayerInstance);
        Destroy(EnemyInstance);
    }

    public void SaveGame()
    {
        PlayerPrefs.SetFloat("PlayerX", PlayerInstance.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", PlayerInstance.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", PlayerInstance.transform.position.z);
        PlayerPrefs.SetFloat("EnemyX", EnemyInstance.transform.position.x);
        PlayerPrefs.SetFloat("EnemyY", EnemyInstance.transform.position.y);
        PlayerPrefs.SetFloat("EnemyZ", EnemyInstance.transform.position.z);
    }
    public void LoadGame()
    {
        PlayerInstance.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ"));
        EnemyInstance.transform.position = new Vector3(PlayerPrefs.GetFloat("EnemyX"), PlayerPrefs.GetFloat("EnemyY"), PlayerPrefs.GetFloat("EnemyZ"));
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
        yield return new WaitForSeconds(1f);
        Debug.Log("Game Start End!");
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
        // Wait until transion ends
        var FadeInComp = PlayerCameraInstance.GetComponent<FadeInEffect>();
        Debug.Log("Transition start");
        FadeInComp.CurrentTime = 0f;
        FadeInComp.Updating = true;
        FadeInComp.Reverse = false;
        yield return new WaitForSeconds(FadeInComp.TransitionTime);
        FadeInComp.Updating = false;

        Debug.Log("Trans time: " + FadeInComp.TransitionTime);
        yield return new WaitForSeconds(FadeInComp.TransitionTime);
        Debug.Log("Transition end");

        // Reset player and AI transform
        InitializeTransform();

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
        yield return new WaitForSeconds(1f);
        Destroy(CountDownInstance);
    }

    private IEnumerator RoundRunning()
    {
        while (true) {
            // loop until someone scored
            if (Input.GetKeyDown(KeyCode.T)) {
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
        FadeInComp.CurrentTime = 0f;
        FadeInComp.Updating = true;
        FadeInComp.Reverse = true;
        yield return new WaitForSeconds(FadeInComp.TransitionTime);
        FadeInComp.Updating = false;
        Debug.Log("Transition end");
        yield return null;
    }

}
