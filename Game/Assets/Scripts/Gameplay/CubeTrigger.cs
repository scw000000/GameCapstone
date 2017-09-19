using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTrigger : MonoBehaviour {

    public MazeRotation _maze; 
	// Use this for initialization
	void Start () {
        _maze = GameObject.FindGameObjectWithTag("Maze").GetComponent<MazeRotation>();
	}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("TriggerBall"))
        {
            _maze.Rotate90();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
