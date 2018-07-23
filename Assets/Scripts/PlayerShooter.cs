using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour {

	private Rigidbody2D rb;
	public GameObject projectilePrefab;
	public float PROJ_VELOCITY = 12f;
	private bool fired = false;
	private bool firing = false;
	private GameObject priorityItem;
	private Image powerBarImage;
	private float fillAmount = 0f;
	private float deltaFill = 0.02f;
	private bool upward = true;
	void Start () {
        rb = GetComponent<Rigidbody2D>();
		ItemManager.instance.onPriorityItemChange += UpdateCurrentItem;
		powerBarImage = GameObject.Find("PowerBar").GetComponent<Image>();
		powerBarImage.enabled = false;
	}
	void OnEnable() {

	}
	void OnDisable() {

	}
	
	void FixedUpdate () {
		if (!firing && Input.GetButton("Shoot") && fired == false) {
			// Begin firing sequence
            firing = true;
            fillAmount = 0f;
			powerBarImage.enabled = true;
		}
		if (firing) {
			UpdateFillAmount();
            if (!Input.GetButton("Shoot")) {
                // End firing sequence
                Shoot(priorityItem, powerBarImage.fillAmount);
                firing = false;
                powerBarImage.enabled = false;
            } else {
				powerBarImage.fillAmount = fillAmount;
			}
		}
	}

	// Ensure Type in Image component is set to Filled
	void UpdateFillAmount() {
		if (upward) {
            if (fillAmount + deltaFill < 1f) {
                fillAmount += deltaFill;
            }
            else {
                // switch directions
				upward = false;
            }
		} else {
			// downward
			if (fillAmount - deltaFill > 0) {
				fillAmount -= deltaFill;
			} else {
				// switch directions
				upward = true;
			}
		}
	}

	// power is between 0 and 1
	void Shoot(GameObject item, float power) {
		GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation); 
		projectile.GetComponent<ProjectileController>().SetItem(item);
		Rigidbody2D prb = projectile.GetComponent<Rigidbody2D>();
        prb.velocity = PROJ_VELOCITY * power * rb.transform.up;
	}

	void UpdateCurrentItem(GameObject priorityItem) {
		this.priorityItem = priorityItem;
	}
}
