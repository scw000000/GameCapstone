using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSlotGeneration : MonoBehaviour {
    public bool _clickForLoad = true;
    public int _promptAnswer = 0;
    public GameObject _saveSlotPrefab;
    private GameObject[] _saveSlots;
    public GameObject _loadingCompGO;
    public GameObject _promptGO;
	// Use this for initialization
	void Start () {
    }

    public void InitSaveSlot() {
        if (gameObject.transform.childCount <= 0) {
            _saveSlots = new GameObject[GameCapstone.SaveData._maxSaveSlotNum];
            for (int i = 0; i < GameCapstone.SaveData._maxSaveSlotNum; ++i)
            {
                _saveSlots[i] = Instantiate(_saveSlotPrefab, gameObject.transform);
            }
        }
        for (int i = 0; i < GameCapstone.SaveData._maxSaveSlotNum; ++i)
        {
            SetupSaveSlot(gameObject.transform.GetChild(i).gameObject, i);
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
        // var imageGO = newSaveSlot.transform.Find("Image");
        var progressGO = newSaveSlot.transform.Find("Detail").transform.Find("Progress");
        var timeGO = newSaveSlot.transform.Find("Detail").transform.Find("Time");
        var clickEvent = newSaveSlot.GetComponent<UnityEngine.UI.Button>().onClick;

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

            clickEvent.RemoveAllListeners();
            if (_clickForLoad)
            {
                // This is some stupid shit because delegate will try to setup the reference instead of 
                // the current value of i
                clickEvent.AddListener(delegate {
                    StartCoroutine("TryLoadGame", newSaveSlot.transform.GetSiblingIndex());
                    // TryLoadGame(newSaveSlot.transform.GetSiblingIndex());
                });
            }
            // Only non-auto save slot can be saved
            else if(newSaveSlot.transform.GetSiblingIndex() != 0)
            {
                clickEvent.AddListener(delegate {
                    StartCoroutine("TrySaveGame", newSaveSlot.transform.GetSiblingIndex());
                    // TrySaveGame(newSaveSlot.transform.GetSiblingIndex());
                });
                // clickEvent.AddListener(delegate { LoadGame(newSaveSlot.transform.GetSiblingIndex()); });
            }

        }
        else
        {
            progressGO.GetComponent<UnityEngine.UI.Text>().text = "Empty";
            timeGO.GetComponent<UnityEngine.UI.Text>().text = "Invalid";

            // Only save button works if the save data is not valid
            if (!_clickForLoad && newSaveSlot.transform.GetSiblingIndex() != 0)
            {
                clickEvent.AddListener(delegate {
                    StartCoroutine("TrySaveGame", newSaveSlot.transform.GetSiblingIndex());
                    // TrySaveGame(newSaveSlot.transform.GetSiblingIndex());
                });
            }
        }
    }

    private IEnumerator TryLoadGame(int slot) {
        Debug.Log("Load Slot " + slot);
        _promptAnswer = 0;
        _promptGO.SetActive(true);
        SetUpButtonEnabled(false);
        _promptGO.transform.Find("PromptText").GetComponent<UnityEngine.UI.Text>().text = "Load " +
            (slot == 0 ? "Auto Save Data":"Slot " + slot + " Data") + "?";
        while (_promptAnswer == 0) {
            yield return null;
        }
        if (_promptAnswer == 1) {
            var loadCanvas = GameObject.Find("LoadGameCanvas");
            if (loadCanvas != null)
            {
                loadCanvas.SetActive(false);
            }
            var hudCanvas = GameObject.Find("HUDCanvas");
            if (hudCanvas != null)
            {
                hudCanvas.SetActive(false);
            }
            loadCanvas = GameObject.Find("LoadGameMenu");
            if (loadCanvas != null)
            {
                loadCanvas.SetActive(false);
            }
            
            _loadingCompGO.GetComponent<LevelLoading>().LoadSavedSlot(slot);
            
        }
        else{
            SetUpButtonEnabled(true);
        }
        _promptGO.SetActive(false);
        yield return null;
    }

    private IEnumerator TrySaveGame(int slot) {
        Debug.Log("Save Slot " + slot);
        _promptAnswer = 0;
        _promptGO.SetActive(true);
        _promptGO.transform.Find("PromptText").GetComponent<UnityEngine.UI.Text>().text = "Save To " +
            (slot == 0 ? "Auto Save Data" : "Slot " + slot + " Data") + "?";
        while (_promptAnswer == 0)
        {
            yield return null;
        }
        if (_promptAnswer == 1)
        {
            var playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
            {
                playerGO.GetComponent<SaveGameHelper>().SaveGame(slot, playerGO.GetComponent<PlayerStatus>()._currentProgress);
                // Need to update the save slot text
                SetupSaveSlot(gameObject.transform.GetChild(slot).gameObject, slot);
            }
                        
        }
        else
        {
            SetUpButtonEnabled(true);
        }
        _promptGO.SetActive(false);
    }

    private void SetUpButtonEnabled(bool isEnabled) {
        foreach (var button in _saveSlots) {
            button.GetComponent<UnityEngine.UI.Button>().enabled = isEnabled;
        }
    }

    public void SetPromptAnswer(int ans) {
        _promptAnswer = ans;
    }

	// Update is called once per frame
	//void Update () {
		
	//}
}
