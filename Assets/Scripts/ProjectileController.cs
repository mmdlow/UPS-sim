using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	private GameObject item;
	private SpriteRenderer spriteR;
	private int DELAY = 3;
	void Awake () {
		spriteR = gameObject.GetComponent<SpriteRenderer>();
	}
	void Start() {
		Destroy(gameObject, DELAY);
	}
	
	void Update () {
		
	}
	
	// Item this projectile represents
	public void SetItem(GameObject item) {
		if (item == null) return;
		this.item = item;
		ItemController ic = this.item.GetComponent<ItemController>();
		Sprite sprite = ic.GetItemIcon(); 
		spriteR.sprite = sprite;
	}
}
