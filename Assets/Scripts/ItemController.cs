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
	public static GameObject priorityItem = null;
	public delegate void PriorityItemHandler(GameObject item);
	public static event PriorityItemHandler onPriorityItemChange;
	public static Inventory inventory;
	public static List<GameObject> items = new List<GameObject>(); // Randomized item list

	void UpdateStats(string name, string drb, Sprite icon) {
		this.itemName = name;
		this.durability = drb;
		this.itemIntegrity = 100;
		this.icon = icon;
		// Instantiate(dropzone);

		// Set Item Integrity
		switch (durability) {
			case "HIGH":
				this.itemBreakFactor = 1f;
				break;
			case "MED":
				this.itemBreakFactor = 1.5f;
				break;
			case "LOW":
				this.itemBreakFactor = 2f;
				break;
			default:
				Debug.LogWarning("Invalid durability status");
				break;
		}
	}

	public static Item InitItem(GameObject container, string name, string drb, Sprite icon) {
		bool oldState = container.activeSelf;
		container.SetActive(false); // To enable following code to be called before Awake()
		Item item = container.AddComponent<Item>() as Item;

		item.UpdateStats(name, drb, icon);

		container.SetActive(oldState);
		return item;
	}

	public static void GenerateLevelItems() {
		//Item.inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
		Item.inventory = Inventory.instance;
		int numItems = Mathf.RoundToInt(Random.Range(3, Item.inventory.maxSpace));
		if (numItems > Item.inventory.maxSpace - items.Count) {
			Debug.Log("Not enough room");
			return;
		}
		for (int i = 0; i < numItems; i++) {
			// Pick a random item name
			string itemName = itemNames[Mathf.RoundToInt(Random.Range(0, itemNames.Length - 1))];
			// Pick a random item sprite from sprites, using Inventory sprites array
			Sprite itemImg = Item.inventory.sprites[Mathf.RoundToInt(Random.Range(0, Item.inventory.sprites.Length - 1))];

			// Temp GameObject that will hold an Item instance
			GameObject itemContainer = new GameObject(); 
			// All Item durabilities currently set to HIGH, to be changed in the future
			Item item = Item.InitItem(itemContainer, itemName, "HIGH", itemImg);
			items.Add(item);
		}
		Debug.Log("Generated " + numItems + " packages");

		/* We only pass the List of items to the Inventory upon clicking the Level Start Button.
		This is because GenerateLevelItems() is called before any of the Inventory-associated
		game objects are instantiated, and will cause problems with the Inventory delegates 
		(methods in InventoryUI will be added to their respective delegates only after invoking
		said delegates, causing nothing to happen). By letting the Level Start Button control
		when the items List gets passed to the Inventory, we allow the InventoryUI to be
		initialized first.

		Ideal solution would involve inventory.AddInventoryItemList() being called directly at the
		end of GenerateLevelItems(), without the need to reference the Level Start Button.
		*/
		Button LevelStart = GameObject.Find("Level Start Button").GetComponent<Button>();
		LevelStart.onClick.AddListener(delegate {
			inventory.AddInventoryItemList(items);
		});
	}

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