using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameHelper : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}

    public void SaveGame(int slot, int progress){
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO == null) {
            Debug.LogError("Player does not exist!");
            return;
        }
        if (progress < 0) {
            Debug.LogError("Invalid progress index");
            return;
        }
        GameCapstone.SaveData saveData = new GameCapstone.SaveData();
        saveData._currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        saveData._currentProgress = progress;

        BayatGames.SaveGameFree.SaveGame.Save<GameCapstone.SaveData>(
            GameCapstone.SaveData._saveSlotPrefName + slot, 
            saveData);
        // Make this save data valid
        PlayerPrefs.SetInt(GameCapstone.SaveData._saveSlotValidPrefName + slot, 1);
        Debug.Log("Game saved at slot: " + slot + " progress: " + progress);
    }
}
