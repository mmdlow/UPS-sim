using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {

	public static ItemManager instance = null;

	public GameObject itemPrefab;
	public int maxSpace = 9;
	public GameObject priorityItem = null;
	public delegate void ItemChangeHandler(GameObject item);
	public event ItemChangeHandler onPriorityItemChange;
	public event ItemChangeHandler onItemAdd;
	public event ItemChangeHandler onItemRemove;
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
		int numItems = Random.Range(3, maxSpace);
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

	public void RemoveItem(GameObject item) {
		Debug.Log("Item removed");
		items.Remove(item);
		onItemRemove(item);
		Destroy(item);
	}
}
