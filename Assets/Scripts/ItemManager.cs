﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {

	public static ItemManager instance = null;

	public GameObject itemPrefab;
	private int MAX_ITEMS = 2;
	private int MIN_ITEMS = 2;
	public GameObject priorityItem = null;
	public delegate void ItemChangeHandler(GameObject item);
	public event ItemChangeHandler onPriorityItemChange;
	public event ItemChangeHandler onItemAdd;
	public event ItemChangeHandler onItemRemove;
	public event ItemChangeHandler onItemFired;
	public List<GameObject> items = new List<GameObject>();
	public Sprite[] sprites; // Item sprites to choose from

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject); // enforce singleton pattern
		}
        InitLevelItems();
	}

	void Start() {
	}

	private List<string[]> ReadItemNames() {
		string line;
		List<string[]> arr = new List<string[]>();
		System.IO.StreamReader file = new System.IO.StreamReader(@"Assets/Scripts/ITEM_NAMES.txt");
		while ((line = file.ReadLine()) != null) {
			string[] s = line.Split(',');
			arr.Add(new string[] {s[0], s[1]});
		}
		file.Close();
		return arr;
	}

	public void InitLevelItems() {
		List<string[]> itemNamesDurabilities = ReadItemNames();
		int numItems = Random.Range(MIN_ITEMS, MAX_ITEMS+1);
		for (int i=0; i<numItems; i++) {
            int randomIndex = Random.Range(0, itemNamesDurabilities.Count);
            string[] s = itemNamesDurabilities[randomIndex];   // s[0] is name, s[1] is durability
            itemNamesDurabilities.RemoveAt(randomIndex);
			
			GameObject itemClone = Instantiate(itemPrefab);
			itemClone.GetComponent<ItemController>().UpdateStats(s[0], s[1], sprites[Random.Range(0, sprites.Length)]);
			items.Add(itemClone);
		}
	}

	public void ChangePriorityItem(GameObject item) {
		priorityItem = item;
		if (onPriorityItemChange != null) {
			onPriorityItemChange(item);
		}
	}

	public void AddItem(GameObject item) {
		// Might not work!
		items.Add(item);
		onItemAdd(item);
	}

	// item fired from player
	public void FireItem(GameObject item) {
		items.Remove(item);
		onItemFired(item);
	}

	// item hits dropzone and is removed (after fired)
	public void RemoveItem(GameObject item) {
		onItemRemove(item);
		if (priorityItem == item) {
			ChangePriorityItem(null);
		} else {
			ChangePriorityItem(ItemManager.instance.priorityItem);
		}
		Destroy(item);
	}
	// item does not hit dropzone or hits then exits (after fired)
}
