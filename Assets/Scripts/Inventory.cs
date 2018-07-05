using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public List<string> items = new List<string>();
	public int maxSpace = 9;
	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;
	public GameObject GeneratePackagesBtn;
	public static Inventory instance;

	// Create Singleton Inventory for reference
	void Awake() {
		if (instance != null) {
			Debug.LogWarning("More than one instance of Inventory found!");
			return;
		}
		instance = this;
	}

	// Generate random number of packages from 1 to maxSpace
	public void GeneratePackages() {
		int numItems = Mathf.RoundToInt(Random.Range(1, maxSpace));
		if (numItems > maxSpace - items.Count) {
			Debug.Log("Not enough room");
			return;
		}
		for (int i = 0; i < numItems; i++) {
			// This will be replaced with a pkg name from a list
			items.Add("new item");
		}
		Debug.Log("Generated " + numItems + " packages");
		if (onItemChangedCallback != null) {
			onItemChangedCallback.Invoke();
		}
		// Remove Generate Packages Button
		GeneratePackagesBtn.SetActive(false);
	}

	public void Remove(string item) {
		items.Remove(item);
		if (onItemChangedCallback != null) {
			onItemChangedCallback.Invoke();
		}
	}
}
