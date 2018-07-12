using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

	public Transform itemsParent; // Reference Slot Parent to access slots
	public GameObject inventoryUI; // Reference UI
	Inventory inventory;
	InventorySlot[] slots; // Array of inventory slots
	PlayerController player;

	// Use this for initialization
	void Start () {
		// Close inventory by default
		inventoryUI.SetActive(false);
		inventory = Inventory.instance;
		inventory.onItemChangedCallback += UpdateUI;
		inventory.onPriorityChangedCallback += UpdatePriority;

		slots = itemsParent.GetComponentsInChildren<InventorySlot>();

		player = GameObject.Find("Player").GetComponent<PlayerController>();
		player.onSustainDamageCallback += UpdateItemIntegrities;
	}
	
	// Update is called once per frame
	void Update () {
		// Allow player to open/close inventory
		if (Input.GetButtonDown("Inventory")) {
			inventoryUI.SetActive(!inventoryUI.activeSelf);
		}
	}

	/* Loops through all Inventory script items array, adds item to slots if
	there is an item to add, else clear slot. Should theoretically only be
	called once, when Gen Pkg button is pressed */
	void UpdateUI () {
		Debug.Log("Updating UI");
		for (int i = 0; i < slots.Length; i++) {
			if (i < inventory.items.Count) {
				// pass pkg name and a random pkg sprite from our array
				slots[i].AddItem(inventory.items[i]);
			} else {
				slots[i].ClearSlot();
			}
		}
	}

	/* Loops thorugh inventory slots and updates priority package based on
	the corresponding index in Inventory script */
	void UpdatePriority() {
		Debug.Log("Updating package priority");
		for (int i = 0; i < slots.Length; i++) {
			Item slotItem = slots[i].GetSlotItem();
			if (slotItem != null && slotItem.Equals(inventory.priorityItem)) {
				slots[i].Prioritize();
			} else {
				slots[i].Deprioritize();
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
