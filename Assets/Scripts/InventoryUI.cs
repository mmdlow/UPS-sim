﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

	InventorySlot[] slots; // Array of inventory slots
	PlayerController player;
	Animator animator;
	bool open = false;
	public static InventoryUI instance = null;
	private int currentIndex = 0;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject); // enforce singleton pattern
		}
		animator = gameObject.GetComponent<Animator>();
	}

	void Start () {
		ItemManager.instance.onItemAdd += UpdateUI;
		ItemManager.instance.onItemFired += UpdateUI;
		ItemManager.instance.onPriorityItemChange += UpdatePriorityIndicator;

		slots = GetComponentsInChildren<InventorySlot>();
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		player.onSustainDamageCallback += UpdateItemIntegrities;
	}

	void Update() {
		if (!open) return;
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
			MoveToPrevItem();
		} else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
			MoveToNextItem();
		} else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
			MoveToTopItem();
		} else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
			MoveToBottomItem();
		}
	}

	void MoveToSlot(int incr) {
		if (incr == 0) {
			// Prioritize first item
			if (slots[0].GetSlotItem() != null) {
				currentIndex = 0;
				ItemManager.instance.ChangePriorityItem(slots[0].GetSlotItem());
			}
		} else if (slots[currentIndex + incr].GetSlotItem() != null) {
			currentIndex += incr;
			ItemManager.instance.ChangePriorityItem(slots[currentIndex].GetSlotItem());
		}
	}

	void MoveToPrevItem() {
		if (ItemManager.instance.priorityItem == null) {
			// If priority item not currently set, prioritize first item by default
			MoveToSlot(0);
		} else if (currentIndex > 0) {
			MoveToSlot(-1);
		}
	}

	void MoveToNextItem() {
		if (ItemManager.instance.priorityItem == null) {
			MoveToSlot(0);
		} else if (currentIndex < ItemManager.instance.items.Count - 1) {
			MoveToSlot(1);
		}
	}

	void MoveToTopItem() {
		if (ItemManager.instance.priorityItem == null) {
			MoveToSlot(0);
		} else if (currentIndex >= 3) {
			MoveToSlot(-3);
		}
	}

	void MoveToBottomItem() {
		if (ItemManager.instance.priorityItem == null) {
			MoveToSlot(0);
		} else if (currentIndex <= 5) {
			MoveToSlot(3);
		}
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
				currentIndex = i;
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

	public void ScreenIn() {
		open = true;
		animator.SetBool("isOpen", true);
	}

	public void ScreenOut() {
		open = false;
		animator.SetBool("isOpen", false);
	}

	public void ToggleScreen() {
		if (!open) {
			ScreenIn();
		} else {
			ScreenOut();
		}
	}

	public bool IsOpen() {
		return open;
	}
}
