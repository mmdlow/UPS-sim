using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

	InventorySlot[] slots; // Array of inventory slots
	PlayerController player;
	public static InventoryUI instance = null;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject); // enforce singleton pattern
		}
	}

	void Start () {
		ItemManager.instance.onItemAdd += UpdateUI;
		ItemManager.instance.onItemFired += UpdateUI;
		ItemManager.instance.onPriorityItemChange += UpdatePriorityIndicator;

		slots = GetComponentsInChildren<InventorySlot>();
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		player.onSustainDamageCallback += UpdateItemIntegrities;
	}
	
	public void UpdateUI (GameObject item) {
		for (int i=0; i<slots.Length; i++) {
			if (i < ItemManager.instance.items.Count) {
				slots[i].SetItem(ItemManager.instance.items[i]);
			} else {
				slots[i].ClearSlot();
			}
		}
	}

	/* Loops thorugh inventory slots and updates priority package based on
	the corresponding index in Inventory script */
	void UpdatePriorityIndicator(GameObject priorityItem) {
		for (int i = 0; i < slots.Length; i++) {
			GameObject slotItem = slots[i].GetSlotItem();
			if (slotItem != null && slotItem == ItemManager.instance.priorityItem) {
				slots[i].SetPriorityAlert();
			} else {
				slots[i].UnsetPriorityAlert();
			}
		}
	}

	/* Loops through inventory slots and updates slot item integrities based
	on damage sustained by player*/
	void UpdateItemIntegrities(float damage) {
		for (int i = 0; i < slots.Length; i++) {
			if (slots[i].GetSlotItem() != null) {
				slots[i].UpdateItemIntegrity(damage);
			}
		}
	}
	public void Clear() {
		for (int i=0; i<slots.Length; i++) {
            slots[i].ClearSlot();
			slots[i].UnsetPriorityAlert();
		}
	}
}
