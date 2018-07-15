using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour {


	string itemName;
	string durability; // LOW, MED, HIGH -> Displayed in level start screen
	float itemBreakFactor; // Affected by drb -> Item resilience to damage
	int itemIntegrity; // Item health -> Any damage to item = Player damage * BreakFactor
	Sprite icon;

	public GameObject dropzonePrefab;
	public static GameObject itemPrefab;
	public static GameObject priorityItem = null;
	public delegate void PriorityItemHandler(GameObject item);
	public static event PriorityItemHandler onPriorityItemChange;
	public static List<GameObject> items = new List<GameObject>(); // Randomized item list


	private static List<string[]> ReadItemNames() {
		string line;
		List<string[]> arr = new List<string[]>();
		System.IO.StreamReader file = new System.IO.StreamReader(@"ITEM_NAMES.txt");
		while ((line = file.ReadLine()) != null) {
			Debug.Log(line);
			string[] s = line.Split(',');
			arr.Add(new string[] {s[0], s[1]});
		}
		file.Close();
		return arr;
	}

	public static void InitLevelItems() {
		List<string[]> itemNamesDurabilities = ReadItemNames();
		int numItems = Random.Range(3, InventoryController.instance.maxSpace);
		for (int i=0; i<numItems; i++) {
            int randomIndex = Random.Range(0, itemNamesDurabilities.Count);
            string[] s = itemNamesDurabilities[randomIndex];
            itemNamesDurabilities.RemoveAt(randomIndex);
			GameObject itemClone = Instantiate(itemPrefab);

		}
	}

	public static void ChangePriorityItem(GameObject item) {
		priorityItem = item;
		if (onPriorityItemChange != null) {
			onPriorityItemChange(item);
		}
	}

	void Start() {
		if (dropzonePrefab != null) {
			Instantiate(dropzonePrefab);
		}
		if (priorityItem == null) {
			ChangePriorityItem(this.gameObject);
		}
	}

	public string GetItemName() {
		return itemName;
	}

	public string GetItemDurability() {
		return durability;
	}

	public Sprite GetItemIcon() {
		return icon;
	}

	public int UpdateIntegrity(int playerDamage) {
		itemIntegrity -= Mathf.RoundToInt(playerDamage * itemBreakFactor);
		if (itemIntegrity <= 25) Debug.Log("Warning! " + itemName + " at risk");
		return itemIntegrity;
	}
}