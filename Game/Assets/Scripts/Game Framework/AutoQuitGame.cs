using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoQuitGame : MonoBehaviour {
    public float _waitTimeBeforeQuit = 3f;
	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine("AutoQuit");
    }

    // Update is called once per frame
    void Update () {
        if (Input.anyKeyDown)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
        }
    }

    IEnumerator AutoQuit() {
        yield return new WaitForSeconds(_waitTimeBeforeQuit);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
        yield return null;
    }
}
