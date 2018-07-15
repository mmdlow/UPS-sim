using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {

	public static ItemManager instance = null;

	public GameObject itemPrefab;
	public GameObject priorityItem = null;
	public delegate void PriorityItemHandler(GameObject item);
	public event PriorityItemHandler onPriorityItemChange;
	public List<GameObject> items = new List<GameObject>();
	public Sprite[] sprites; // Item sprites to choose from

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject); // enforce singleton pattern
		}
	}

	void Start() {
		InitLevelItems();
	}

	private List<string[]> ReadItemNames() {
		string line;
		List<string[]> arr = new List<string[]>();
		System.IO.StreamReader file = new System.IO.StreamReader(@"Assets/Scripts/ITEM_NAMES.txt");
		while ((line = file.ReadLine()) != null) {
			Debug.Log(line);
			string[] s = line.Split(',');
			arr.Add(new string[] {s[0], s[1]});
		}
		file.Close();
		return arr;
	}

	public void InitLevelItems() {
		List<string[]> itemNamesDurabilities = ReadItemNames();
		int numItems = Random.Range(3, InventoryController.instance.maxSpace);
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
}
