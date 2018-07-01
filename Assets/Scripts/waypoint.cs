using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("Started!");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider col) {
        Debug.Log("Hit!");
    }
}
