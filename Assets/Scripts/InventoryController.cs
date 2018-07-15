using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public List<Item> items = new List<Item>(); // Randomized item list

	public int maxSpace = 9;

	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;

	public static Inventory instance;

	public Sprite[] sprites; // Item sprites to choose from

	// Create Singleton Inventory for reference
	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(this.gameObject); // enforce singleton pattern
		}
	}

	public void AddInventoryItemList(List<Item> itemList) {
		items = itemList;
		if (onItemChangedCallback != null) {
			onItemChangedCallback.Invoke();
		}
	}

	public void Remove(Item item) {
		items.Remove(item);
		if (onItemChangedCallback != null) {
			onItemChangedCallback.Invoke();
		}
	}
}
