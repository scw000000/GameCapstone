using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoading : MonoBehaviour {
    public GameObject LoadingScreenPrefab;
    private GameObject LoadingScreenInstance;
	public GameObject Mainmenu;
	public Text LoadingText;
    private AsyncOperation AsyncOp = null;

    // Use this for initialization
    void Start () {
        // For testing purpose
        //StartLoadLevel("");
    }
	
	// Update is called once per frame
	void Update () {
        if (LoadingText != null)
        {
            LoadingText.color = new Color(LoadingText.color.r, LoadingText.color.g, LoadingText.color.b, Mathf.PingPong(Time.time, 1));
        }
    }

    // Hard coded, should be fixed later
    public void StartLoadLevel(string levelName) {
        StartCoroutine(LoadLevel("Assets/Scenes/" + levelName + ".unity"));
        // StartCoroutine(LoadLevel("Assets/Scenes/StartingCave.unity"));
    }

    // Hard coded, should be fixed later
    public void GameOverMenu(string levelName)
    {
        SceneManager.LoadScene("Assets/Scenes/GameOverMenu.unity");
    }

    public IEnumerator LoadLevel (string levelName){
        Debug.Log("Loadlevel start");
        AsyncOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelName);
        if (AsyncOp == null) {
            Debug.LogError("Level loading failed");
            yield break;
        }
        // Disable sceen switch until we are good to go;
        AsyncOp.allowSceneActivation = false;
		// Mainmenu.SetActive (false);
        InitLoadingScreen();
        Debug.Log("Start updating loading screen");
        yield return UpdateLoadingScreen();
        yield return SwitchToLevel();
    }

    private void InitLoadingScreen() {
        // Load texture and text or something
        if (LoadingScreenPrefab == null){
            Debug.LogError("Need to Specify a loading screen");
            return;
        }
		LoadingScreenPrefab.SetActive (true);
        LoadingScreenInstance = Instantiate(LoadingScreenPrefab) as GameObject;

    }

    private IEnumerator UpdateLoadingScreen() {

        while (AsyncOp != null && AsyncOp.progress < 0.9f) {
            // Use this progress to display progress bar
            // progress will be stucked at 0.9 if AsyncOp.allowSceneActivation is false
            float currentProgress = AsyncOp.progress / 0.9f;
            Debug.Log(currentProgress);
            yield return null;
        }


        Debug.Log("Load Complete");
    }

    private IEnumerator SwitchToLevel() {
        Time.timeScale = 1;
        Destroy(LoadingScreenInstance);
        AsyncOp.allowSceneActivation = true;
        yield return null;
    }

	public void Quit () 
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

    public void ClearSaveData() {
        // PlayerPrefs.SetInt("PlayerScore", 0);
        // PlayerPrefs.SetInt("AIScore", 0);
    }
}
