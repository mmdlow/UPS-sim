using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

	public Transform itemsParent; // Reference Slot Parent to access slots
	public Sprite[] spriteCol; // Package sprites to choose from
	public GameObject inventoryUI; // Reference UI
	bool hasPackages;
	Inventory inventory;
	InventorySlot[] slots; // Array of inventory slots

	// Use this for initialization
	void Start () {
		inventory = Inventory.instance;
		inventory.onItemChangedCallback += UpdateUI;
		slots = itemsParent.GetComponentsInChildren<InventorySlot>();
	}
	
	// Update is called once per frame
	void Update () {
		// Only let player close inventory after generating packages
		if (hasPackages && Input.GetButtonDown("Inventory")) {
			inventoryUI.SetActive(!inventoryUI.activeSelf);
		}
	}

	/* Loop through all Inventory script items array, adds item to slots if
	there is an item to add, else clear slot. Should theoretically only be
	called once, when Gen Pkg button is pressed */
	void UpdateUI () {
		Debug.Log("Updating UI");
		for (int i = 0; i < slots.Length; i++) {
			if (i < inventory.items.Count) {
				// pass pkg name and a random pkg sprite from our array
				slots[i].AddItem(inventory.items[i], spriteCol[Mathf.RoundToInt(Random.Range(0, spriteCol.Length - 1))]);
			} else {
				slots[i].ClearSlot();
			}
		}
		hasPackages = true;
	}
}
