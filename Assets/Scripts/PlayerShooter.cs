using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour {

	private Rigidbody2D rb;
	public GameObject projectilePrefab;
	public float PROJ_VELOCITY = 0.1f;
	private bool fired = false;
	private GameObject priorityItem;
	void Start () {
        rb = GetComponent<Rigidbody2D>();
		ItemManager.instance.onPriorityItemChange += UpdateCurrentItem;
	}
	void OnEnable() {

	}
	void OnDisable() {

	}
	
	void Update () {
		if (Input.GetButton("Shoot") && fired == false && BoardManager.instance.IsDoingSetup() == false) {
			fired = true;
			Shoot(priorityItem);
		}
	}
	void Shoot(GameObject item) {
		GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation); 
		projectile.GetComponent<ProjectileController>().SetItem(item);
		Rigidbody2D prb = projectile.GetComponent<Rigidbody2D>();
		if (rb.velocity.magnitude < 1f) {
			prb.velocity = 1f * projectile.transform.up;
		} else {
			prb.velocity = PROJ_VELOCITY * rb.velocity;
		}
		Debug.Log("Vehicle vel mag: " + rb.velocity.magnitude);
		Debug.Log("proj vel magnitude: " + prb.velocity.magnitude);
	}
	void UpdateCurrentItem(GameObject priorityItem) {
		this.priorityItem = priorityItem;
	}
}
