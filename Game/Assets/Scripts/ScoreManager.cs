using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    private float _totalScore;
    private float _subSpeed;
	// Use this for initialization
	void Start () {
        if (_totalScore <=0)
        {
            _totalScore = 5000.0f;
        }
        _subSpeed = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (_totalScore > 0)
        {
            _totalScore -= Time.deltaTime*_subSpeed;
        }
	}

    public void RegulateSubSpeed(float s)
    {
        if (s > 0.0f)
        {
            _subSpeed = s;
        }
        else
        {
            Debug.Log("Substract speed Should always be positive.");
        }
    }
    public void AddScore(float s)
    {
        if (s > 0.0f)
        {
            _totalScore += s;
        }
        else
        {
            Debug.Log("Score Should always be positive.");
        }
    }
    public void SubstractScore(float s)
    {
        if (s > 0.0f)
        {
            _totalScore -= s;
        }
        else
        {
            Debug.Log("Score Should always be positive.");
        }
    }
    public float ReturnFloatScore()
    {
        return _totalScore;
    }
    public int ReturnIntTotalScore()
    {
        return (int)_totalScore;
    }
}
