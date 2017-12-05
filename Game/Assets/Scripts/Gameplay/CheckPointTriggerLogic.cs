using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTriggerLogic : MonoBehaviour {
    public int _progress = 0;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update () {
        //if (Input.GetKeyDown(KeyCode.L)) {
        //    var playerGO = GameObject.FindGameObjectWithTag("Player");
        //    var saveData = BayatGames.SaveGameFree.SaveGame.Load<GameCapstone.SaveData>(
        //        GameCapstone.SaveData._saveSlotPrefName + 0,
        //        new GameCapstone.SaveData()
        //        );
        //    playerGO.GetComponent<PlayerStatus>()._currentProgress = saveData._currentProgress;
        //}
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.tag.Equals("Player")) {
            return;
        }
        other.gameObject.GetComponent<SaveGameHelper>().SaveGame(0, _progress);
        
        if (other.gameObject.GetComponent<PlayerStatus>()._currentProgress != _progress) {
            // Also need to update current progress in player status component
            other.gameObject.GetComponent<PlayerStatus>()._currentProgress = _progress;
            GameObject.Find("GameManager").GetComponent<GameManager>().DisplaySystemMessage("Checkpoint " + (_progress + 1) + " saved", 2f);

        }
    }
}
