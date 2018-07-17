using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

	InventorySlot[] slots; // Array of inventory slots
	PlayerController player;

	// Use this for initialization
	void Start () {

		// Close inventory by default
		//gameObject.SetActive(false);
		ItemManager.instance.onItemAdd += AddSlot;
		ItemManager.instance.onItemRemove += RemoveSlot;
		ItemManager.instance.onPriorityItemChange += UpdatePriorityIndicator;

		slots = GetComponentsInChildren<InventorySlot>();
		for (int i=0; i<ItemManager.instance.items.Count; i++) {
			slots[i].SetItem(ItemManager.instance.items[i]);
		}
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		player.onSustainDamageCallback += UpdateItemIntegrities;
	}
	
	// Update is called once per frame
	void Update () {
		// Allow player to open/close inventory
		// if (Input.GetButtonDown("Inventory")) {
		// 	Debug.Log("Toggling inventory");
		// 	gameObject.SetActive(!gameObject.activeInHierarchy);
		// }
	}

	void RemoveSlot(GameObject item) {
		for (int i=0; i<slots.Length; i++) {
			if (slots[i].GetSlotItem().Equals(item)) {
				slots[i].ClearSlot();
			}
		}
	}
	void AddSlot(GameObject item) {
		for (int i=0; i<slots.Length; i++) {
			if (slots[i].GetSlotItem().Equals(null)) {
				slots[i].SetItem(item);
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
