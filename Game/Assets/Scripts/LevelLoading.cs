using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoading : MonoBehaviour {
    public GameObject LoadingScreenPrefab;
    private GameObject LoadingScreenInstance;
    private AsyncOperation AsyncOp = null;
    // Use this for initialization
    void Start () {
        // For testing purpose
        StartLoadLevel("");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Hard coded, should be fixed later
    public void StartLoadLevel(string levelName) {
        StartCoroutine(LoadLevel("Assets/Scenes/Level1.unity"));
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
        InitLoadingScreen();
        Debug.Log("Start updating loading screen");
        yield return StartCoroutine(UpdateLoadingScreen() );
        yield return StartCoroutine(SwitchToLevel());
    }

    private void InitLoadingScreen() {
        // Load texture and text or something
        if (LoadingScreenPrefab == null){
            Debug.LogError("Need to Specify a loading screen");
            return;
        }
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
        yield return new WaitForSeconds(3f);
        Destroy(LoadingScreenInstance);
        AsyncOp.allowSceneActivation = true;
        yield return null;
    }
}
