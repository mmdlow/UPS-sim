using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPinRotationController : MonoBehaviour {

	GameObject worldmapCamera;
	GameObject player;

	// Use this for initialization
	void Start () {
		worldmapCamera = GameObject.Find("Worldmap Camera");
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (name.Equals("Worldmap Truck Pin")) {
			transform.rotation = worldmapCamera.transform.rotation;
			return;
		}
		transform.rotation = player.transform.rotation;
	}
}
