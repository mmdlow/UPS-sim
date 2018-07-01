using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	private BoardManager boardManager;
	public GameObject gameManager;
	// Use this for initialization
	void Awake () {
		boardManager = gameManager.GetComponent<BoardManager>();
	}
	
	// Update is called once per frame
	void Update () {
	}
     void OnTriggerEnter2D(Collider2D other) {
		Destroy(this.gameObject);
     }
	 void OnDestroy() {
		 boardManager.LoadNextWaypoint();
	 }
}
