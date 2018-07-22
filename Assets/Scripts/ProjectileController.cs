using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	private GameObject item;
	private SpriteRenderer spriteR;
	void Awake () {
		spriteR = gameObject.GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		
	}
	
	// Item this projectile represents
	public void SetItem(GameObject item) {
		if (item == null) return;
		this.item = item;
		ItemController ic = this.item.GetComponent<ItemController>();
		Sprite sprite = ic.GetItemIcon(); // why does this not work?
		spriteR.sprite = sprite;
	}
}
