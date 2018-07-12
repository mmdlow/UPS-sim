using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public List<Item> items = new List<Item>(); // Randomized item list

	public int maxSpace = 9;
	public Item priorityItem = null; // Current priority item

	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;

	public delegate void OnPriorityChanged();
	public OnPriorityChanged onPriorityChangedCallback;

	public static Inventory instance;

	public Sprite[] sprites; // Item sprites to choose from

	// Create Singleton Inventory for reference
	void Awake() {
		if (instance != null) {
			Debug.LogWarning("More than one instance of Inventory found!");
			return;
		}
		instance = this;
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

	public void UpdateItemPriorities(Item item) {
		priorityItem = item;
		if (onPriorityChangedCallback != null) {
			onPriorityChangedCallback.Invoke();
		}
	}
}
