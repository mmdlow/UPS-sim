using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

	public Transform itemsParent; // Reference Slot Parent to access slots
	public GameObject inventory; // Reference UI
	InventorySlot[] slots; // Array of inventory slots
	PlayerController player;

	// Use this for initialization
	void Start () {
		inventory = transform.Find("Inventory").gameObject;

		// Close inventory by default
		inventory.SetActive(false);
		InventoryController.instance.onItemChangedCallback += UpdateUI;
		ItemManager.instance.onPriorityItemChange += UpdatePriorityIndicator;

		slots = itemsParent.GetComponentsInChildren<InventorySlot>();

		player = GameObject.Find("Player").GetComponent<PlayerController>();
		player.onSustainDamageCallback += UpdateItemIntegrities;
	}
	
	// Update is called once per frame
	void Update () {
		// Allow player to open/close inventory
		if (Input.GetButtonDown("Inventory")) {
			inventory.SetActive(!inventory.activeSelf);
		}
	}

	/* Loops through all Inventory script items array, adds item to slots if
	there is an item to add, else clear slot. Should theoretically only be
	called once, when Gen Pkg button is pressed */
	void UpdateUI () {
		Debug.Log("Updating UI");
		for (int i = 0; i < slots.Length; i++) {
			if (i < InventoryController.instance.items.Count) {
				// pass pkg name and a random pkg sprite from our array
				slots[i].SetItem(InventoryController.instance.items[i]);
			} else {
				slots[i].ClearSlot();
			}
		}
	}

	/* Loops thorugh inventory slots and updates priority package based on
	the corresponding index in Inventory script */
	void UpdatePriorityIndicator(GameObject priorityItem) {
		Debug.Log("Updating package priority");
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
