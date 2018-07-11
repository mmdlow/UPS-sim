using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {

	public static Item prioritizedItem;

	public string itemName;
	public string durability; // LOW, MED, HIGH
	public Sprite icon;
	//public Dropzone dropzone;

	public static Item InitItem(GameObject container, string name, string drb, Sprite icon) {
		bool oldState = container.activeSelf;
		container.SetActive(false); // To enable following code to be called before Awake()
		Item item = container.AddComponent<Item>() as Item;

		item.UpdateStats(name, drb, icon);

		container.SetActive(oldState);
		return item;
	}

	void UpdateStats(string name, string drb, Sprite icon) {
		this.itemName = name;
		this.durability = drb;
		this.icon = icon;
		//Instantiate(dropzone);
	}

	void Awake() {
		
	}

	static void ChangePriority(Item item) {
		prioritizedItem = item;
	}
}