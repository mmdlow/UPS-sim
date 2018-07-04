using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/*
	BoardManager manages instances of Unity scenes i.e. levels. For every new 
	scene there should be a new instance of boardManager.

	GM and BM follow the singleton pattern, i.e. there can only be one.
	GameObject, their static methods, and BM should all communicate using 
	message passing.
 */
public class BoardManager : MonoBehaviour {

	public static BoardManager instance = null;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject); // enforce singleton pattern wrt BoardManager
		}
	}

	public void SetupLevel() {
		Waypoint.Init();
	}

	// Use message passing to talk to BM. Check your spelling!
	public void Send(String message) {
		switch(message) {
			case "LoadNextWaypoint":
				Waypoint.LoadNextWaypoint();
				break;
			default:
				Debug.LogError("Unknown message! Check your spelling.");
				break;
		}
	}
	public void StartLevel() {
		this.Send("LoadNextWaypoint");  // Try to avoid directly calling Waypoint.LoadNextWaypoint()
	}
}
