using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSlotGeneration : MonoBehaviour {
    public bool _clickForLoad = true;
    public GameObject _saveSlotPrefab;
    public GameObject _loadingCompGO;
	// Use this for initialization
	void Start () {
        
        for (int i = 0; i < GameCapstone.SaveData._maxSaveSlotNum; ++i)
        {
            var newSaveSlot = Instantiate(_saveSlotPrefab, gameObject.transform);
            SetupSaveSlot(newSaveSlot, i);
        }
            
    }

    private void SetupSaveSlot(GameObject newSaveSlot, int index) {
        var slotNameGO = newSaveSlot.transform.Find("SlotName");
        if (index > 0)
        {
            slotNameGO.GetComponent<UnityEngine.UI.Text>().text = "Slot " + index;
        }
        else
        {
            slotNameGO.GetComponent<UnityEngine.UI.Text>().text = "Auto Save";
        }
        var imageGO = newSaveSlot.transform.Find("Image");
        var progressGO = newSaveSlot.transform.Find("Detail").transform.Find("Progress");
        var timeGO = newSaveSlot.transform.Find("Detail").transform.Find("Time");

        int saveSlotValid = PlayerPrefs.GetInt(GameCapstone.SaveData._saveSlotValidPrefName + index);
        if (saveSlotValid > 0)
        {
            var saveData = BayatGames.SaveGameFree.SaveGame.Load<GameCapstone.SaveData>(
            GameCapstone.SaveData._saveSlotPrefName + index,
            new GameCapstone.SaveData()
            );
            progressGO.GetComponent<UnityEngine.UI.Text>().text = saveData._currentLevel + ", Check Point " + saveData._currentProgress;
            timeGO.GetComponent<UnityEngine.UI.Text>().text =
                saveData._dayOfWeek + ", "
                + saveData._month + "/"
                + saveData._day + "/"
                + saveData._year + " - "

                + saveData._hour + ":"
                + saveData._minute + ":"
                + saveData._second;

            var clickEvent = newSaveSlot.GetComponent<UnityEngine.UI.Button>().onClick;
            clickEvent.RemoveAllListeners();
            if (_clickForLoad)
            {
                // This is some stupid shit because delegate will try to setup the reference instead of 
                // the current value of i
                clickEvent.AddListener(delegate { LoadGame(newSaveSlot.transform.GetSiblingIndex()); });
            }
            else
            {
                // clickEvent.AddListener(delegate { LoadGame(newSaveSlot.transform.GetSiblingIndex()); });
            }

        }
        else
        {
            progressGO.GetComponent<UnityEngine.UI.Text>().text = "Empty";
            timeGO.GetComponent<UnityEngine.UI.Text>().text = "Invalid";
        }
    }

    private void LoadGame(int slot) {
        Debug.Log("Load Slot " + slot);
        _loadingCompGO.GetComponent<LevelLoading>().LoadSavedSlot(slot);
    }

    private void SaveGame(int slot) {
        Debug.Log("Save Slot " + slot);
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO == null)
        {
            Debug.Log("Player doesn't exist!");
            return;
        }
        playerGO.GetComponent<SaveGameHelper>().SaveGame(slot, playerGO.GetComponent<PlayerStatus>()._currentProgress);

        // Need to update the save slot text
        SetupSaveSlot(gameObject.transform.GetChild(slot).gameObject, slot);
    }

	// Update is called once per frame
	//void Update () {
		
	//}
}
