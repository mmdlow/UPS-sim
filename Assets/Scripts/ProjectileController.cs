using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public GameObject item;
	private SpriteRenderer spriteR;

	void Awake () {
		spriteR = gameObject.GetComponent<SpriteRenderer>();
	}
	void Start() {
		this.transform.parent = item.transform;
		Invoke("RemoveMissedItem", DropzoneController.SETTLING_DELAY + 1);
	}

	void RemoveMissedItem() {
		ItemManager.instance.MissedItem(item);
		ItemManager.instance.RemoveItem(item);
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
