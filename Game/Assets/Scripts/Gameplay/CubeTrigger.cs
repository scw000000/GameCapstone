using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTrigger : MonoBehaviour {

    public MazeRotation _maze;
    public GameObject _elevator;
	// Use this for initialization
	void Start () {
        _maze = GameObject.FindGameObjectWithTag("Maze").GetComponent<MazeRotation>();
        _elevator = GameObject.Find("Elevator");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("TriggerBall"))
        {
            _maze.Rotate90();
            _elevator.GetComponent<Elevator>().GoingDown();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
