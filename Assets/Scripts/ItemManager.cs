using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {

	public static ItemManager instance = null;

	public GameObject itemPrefab;
	public GameObject priorityItem = null;
	public delegate void ItemChangeHandler(GameObject item);
	public event ItemChangeHandler onPriorityItemChange;
	public event ItemChangeHandler onItemAdd;
	public event ItemChangeHandler onItemRemove;
	public event ItemChangeHandler onItemFired;
	public event ItemChangeHandler onItemMissed;
	public List<GameObject> items = new List<GameObject>();
	public Sprite[] sprites; // Item sprites to choose from

	private int MAX_ITEMS = 2;
	private int MIN_ITEMS = 2;
	
	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject); // enforce singleton pattern
		}
        //InitLevelItems();
	}

	//private List<string[]> ReadItemNames() {
	//	string line;
	//	List<string[]> arr = new List<string[]>();
	//	System.IO.StreamReader file = new System.IO.StreamReader(@"Assets/Scripts/ITEM_NAMES.txt");
	//	while ((line = file.ReadLine()) != null) {
	//		string[] s = line.Split(',');
	//		arr.Add(new string[] {s[0], s[1]});
	//	}
	//	file.Close();
	//	return arr;
	//}

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

	private void AddItem(GameObject item) {
		// Might not work!
		items.Add(item);
		onItemAdd(item);
	}

	// item fired from player
	public void FireItem(GameObject item) {
		items.Remove(item);
		onItemFired(item);
	}

	// item is removed (after fired) whether it hits dropzone or not
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
	public void MissedItem(GameObject item) {
		onItemMissed(item);
		MessageManager.instance.SayPreparedMessage(MessageManager.PreparedMessage.MISSED, 5);
	}
	public void ClearAndLoad(int minItems, int maxItems) {
		// while (this.items.Count > 0) {
		// 	RemoveItem(items[items.Count-1]);
		// }
		foreach (GameObject item in items) {
			Destroy(item);
		}
		this.items.Clear();
		InventoryUI.instance.Clear();
		MapPinManager.instance.Clear();

		MIN_ITEMS = minItems;
		MAX_ITEMS = maxItems;
		InitLevelItems();
		InventoryUI.instance.UpdateUI(null);
	}
	private List<string[]> ReadItemNames() {
		List<string[]> arr = new List<string[]>();
		arr.Add( new string[] { "AK-47", "M" } );
		arr.Add( new string[] { "ANTIQUE DRESS", "H" } );
		arr.Add( new string[] { "ARMADILLO", "M" } );
		arr.Add( new string[] { "BAT FOOD", "M" } );
		arr.Add( new string[] { "BLOOD BAGS", "L" } );
		arr.Add( new string[] { "BOMB", "L" } );
		arr.Add( new string[] { "BONG", "L" } );
		arr.Add( new string[] { "BOOK", "H" } );
		arr.Add( new string[] { "CAT FOOD", "M" } );
		arr.Add( new string[] { "CHINA VASE", "L" } );
		arr.Add( new string[] { "CLAY VASE", "L" } );
		arr.Add( new string[] { "CONDOMS", "M" } );
		arr.Add( new string[] { "CREEPY DOLL", "M" } );
		arr.Add( new string[] { "DESIGNER CLOTHES", "L" } );
		arr.Add( new string[] { "DILDOS", "H" } );
		arr.Add( new string[] { "DOG FOOD", "M" } );
		arr.Add( new string[] { "DORITOS", "L" } );
		arr.Add( new string[] { "DUCT TAPE", "M" } );
		arr.Add( new string[] { "EMPTY BOX", "L" } );
		arr.Add( new string[] { "FAECES", "L" } );
		arr.Add( new string[] { "FEDORA", "M" } );
		arr.Add( new string[] { "FIREWORKS", "L" } );
		arr.Add( new string[] { "FIGURINE", "L" } );
		arr.Add( new string[] { "GLITTER", "M" } );
		arr.Add( new string[] { "GUITAR", "M" } );
		arr.Add( new string[] { "GARLIC", "M" } );
		arr.Add( new string[] { "GLASS BOX", "L" } );
		arr.Add( new string[] { "HAND BAG", "M" } );
		arr.Add( new string[] { "HARD DRIVE", "L" } );
		arr.Add( new string[] { "HEADPHONES", "M" } );
		arr.Add( new string[] { "HOOKAH", "M" } );
		arr.Add( new string[] { "HOLY WATER", "H" } );
		arr.Add( new string[] { "INSECTISIDE", "H" } );
		arr.Add( new string[] { "KATANA", "M" } );
		arr.Add( new string[] { "KETCHUP", "M" } );
		arr.Add( new string[] { "KEYBOARD", "M" } );
		arr.Add( new string[] { "KNIFE", "M" } );
		arr.Add( new string[] { "LAMP", "L" } );
		arr.Add( new string[] { "LAPTOP", "L" } );
		arr.Add( new string[] { "LATEX SUIT", "M" } );
		arr.Add( new string[] { "LEATHER JACKET", "H" } );
		arr.Add( new string[] { "MACBOOK", "L" } );
		arr.Add( new string[] { "MACHETE", "M" } );
		arr.Add( new string[] { "MARIJUANA", "M" } );
		arr.Add( new string[] { "MYSTERY BOX", "M" } );
		arr.Add( new string[] { "PARROT", "L" } );
		arr.Add( new string[] { "PIRANHA", "M" } );
		arr.Add( new string[] { "PORTAL GUN", "M" } );
		arr.Add( new string[] { "RAT POISON", "M" } );
		arr.Add( new string[] { "RATS", "M" } );
		arr.Add( new string[] { "RATTLING BOX", "L" } );
		arr.Add( new string[] { "SENTRY GUN", "M" } );
		arr.Add( new string[] { "SHOTGUN", "M" } );
		arr.Add( new string[] { "SOAP", "M" } );
		arr.Add( new string[] { "STRAP-ONS", "M" } );
		arr.Add( new string[] { "STUFFED TOY", "M" } );
		arr.Add( new string[] { "TABLE", "M" } );
		arr.Add( new string[] { "TV SET", "L" } );
		arr.Add( new string[] { "TOWELS", "M" } );
		arr.Add( new string[] { "TRENCHCOAT", "M" } );
		arr.Add( new string[] { "UMBRELLA", "M" } );
		arr.Add( new string[] { "WARDROBE", "M" } );
		arr.Add( new string[] { "WHIP", "M" } );
		arr.Add( new string[] { "WIENER", "L" } );
		return arr;
	}
}
