using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCubeDestory : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Ball_Fire"))
        {
            GameObject.Destroy(this.gameObject);
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
