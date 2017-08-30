﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongGameManager : MonoBehaviour {
    public GameObject BallPrefab;
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;
    private GameObject PlayerInstance;
    private GameObject EnemyInstance;
    public GameObject PlayerSpawnLocation;
    public GameObject EnemySpawnLocation;
    public GameObject BallSpawnLocation;

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

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(GameStart());

        yield return StartCoroutine(GameRunning());

        yield return StartCoroutine(GameEnding());
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
            yield return null;
        }
        Debug.Log("Game Running End!");
    }

    // Bring up game over menu
    private IEnumerator GameEnding()
    {
        Debug.Log("Game Ending!");
        GameOverScreenInstance = Instantiate(GameOverScreenPrefab);
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(EndGameCreditSceneName);
        Destroy(GameOverScreenInstance);
        Debug.Log("Game Ending End!");
    }
}
