using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour {

	private Rigidbody2D rb;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
		if (Input.GetButton("Shoot")) {
			Shoot(ItemManager.instance.priorityItem);
		}
	}
	void Shoot(GameObject item) {
	
	}
}
