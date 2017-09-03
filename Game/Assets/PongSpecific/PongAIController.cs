using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongAIController : MonoBehaviour {
    
    public float MovingSpeed = 10f;
    // For tracking
    private GameObject Ball;
    private float MinX;
    private float MaxX;
    // Use this for initialization
    void Start()
    {
        var leftMostPos = GameObject.Find("P1LeftMostPos");
        MinX = leftMostPos.transform.position.x + 0.5f + gameObject.transform.localScale.x * 0.5f;
        var rightMostPos = GameObject.Find("P1RightMostPos");
        MaxX = rightMostPos.transform.position.x - 0.5f - gameObject.transform.localScale.x * 0.5f;
        Ball = GameObject.FindGameObjectWithTag("Ball");
    }
	
	// Update is called once per frame
	void Update () {
        float targetX = gameObject.transform.position.x;
        // Dont move if close enough
        if (Mathf.Abs(targetX - Ball.transform.position.x ) <= 0.3f) {
            return;
        }
        // Move right
        if (gameObject.transform.position.x < Ball.transform.position.x) {
            targetX += MovingSpeed * Time.deltaTime;
            targetX = Mathf.Min(targetX, Ball.transform.position.x);
        }
        // Move left
        else {
            targetX -= MovingSpeed * Time.deltaTime;
            targetX = Mathf.Max(targetX, Ball.transform.position.x);
        }
        
        // Limit the position so that it won't clip through wall
        float fixedX = Mathf.Clamp(targetX, MinX, MaxX);
        gameObject.transform.position = new Vector3(fixedX
            , gameObject.transform.position.y
            , gameObject.transform.position.z);
    }
}
