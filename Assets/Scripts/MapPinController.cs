using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPinController : MonoBehaviour {

	GameObject worldmapCamera;
	GameObject player;

	void Start () {
		worldmapCamera = GameObject.Find("Worldmap Camera");
		player = GameObject.Find("Player");
	}
	
	void Update () {
		if (name.Equals("Worldmap Truck Pin")) {
			transform.rotation = worldmapCamera.transform.rotation;
			return;
		}
		transform.rotation = player.transform.rotation;
	}
	void OnDestroy() {
		MapPinManager.instance.RemovePin(this.gameObject);
	}
}
