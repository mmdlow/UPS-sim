﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

	InventorySlot[] slots; // Array of inventory slots
	PlayerController player;

	// Use this for initialization
	void Start () {
		ItemManager.instance.onItemAdd += UpdateUI;
		ItemManager.instance.onItemRemove += UpdateUI;
		ItemManager.instance.onPriorityItemChange += UpdatePriorityIndicator;

		slots = GetComponentsInChildren<InventorySlot>();
		for (int i=0; i<ItemManager.instance.items.Count; i++) {
			slots[i].SetItem(ItemManager.instance.items[i]);
		}
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		player.onSustainDamageCallback += UpdateItemIntegrities;
	}
	
	void Update () {
	}

	void UpdateUI (GameObject item) {
		for (int i=0; i<slots.Length; i++) {
			if (i < ItemManager.instance.items.Count) {
				slots[i].SetItem(ItemManager.instance.items[i]);
			} else {
				slots[i].ClearSlot();
				slots[i].UnsetPriorityAlert();
			}
		}
		UpdatePriorityIndicator(ItemManager.instance.priorityItem);
	}

	/* Loops thorugh inventory slots and updates priority package based on
	the corresponding index in Inventory script */
	void UpdatePriorityIndicator(GameObject priorityItem) {
		Debug.Log("Updating item priorities");
		for (int i = 0; i < slots.Length; i++) {
			GameObject slotItem = slots[i].GetSlotItem();
			if (slotItem != null && slotItem.Equals(priorityItem)) {
				slots[i].SetPriorityAlert();
			} else {
				slots[i].UnsetPriorityAlert();
			}
		}
	}

	/* Loops through inventory slots and updates slot item integrities based
	on damage sustained by player*/
	void UpdateItemIntegrities(int damage) {
		Debug.Log("Updating item integrities");
		for (int i = 0; i < slots.Length; i++) {
			if (slots[i].GetSlotItem() != null) {
				slots[i].UpdateItemIntegrity(damage);
			}
		}
	}
}
