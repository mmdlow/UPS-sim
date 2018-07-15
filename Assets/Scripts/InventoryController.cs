using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

	public List<GameObject> items = new List<GameObject>(); // Randomized item list

	public int maxSpace = 9;

	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;

	public static InventoryController instance;


	// Create Singleton Inventory for reference
	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(this.gameObject); // enforce singleton pattern
		}
	}

	public void AddInventoryItemList(List<GameObject> itemList) {
		items = itemList;
		if (onItemChangedCallback != null) {
			onItemChangedCallback.Invoke();
		}
	}

	public void Remove(GameObject item) {
		items.Remove(item);
		if (onItemChangedCallback != null) {
			onItemChangedCallback.Invoke();
		}
	}
}
