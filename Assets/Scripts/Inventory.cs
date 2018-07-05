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
	string[] pkgNames = new string[] {
		"AK-47", "ANTIQUE DRESS", "ARMADILLO",
		"BAT FOOD", "BLOOD BAGS", "BOMB", "BONG", "BOOKS",
		"CAT FOOD", "CHINA VASE", "CLAY VASE", "CONDOMS", "CREEPY DOLL",
		"DESIGNER CLOTHES", "DILDOS", "DOG FOOD", "DORITOS", "DUCT TAPE", 
		"EMPTY BOX",
		"FAECES", "FEDORA", "FIREWORKS", "FIGURINES",
		"GLITTER", "GUITAR", "GARLIC", "GLASS BOX",
		"HAND BAG", "HARD DRIVE", "HEADPHONES", "HOOKAH", "HOLY WATER",
		"INSECTISIDE",
		"KATANA", "KETCHUP", "KEYBOARD", "KNIFE",
		"LAMP", "LAPTOP", "LATEX SUIT", "LEATHER JACKET", 
		"MACBOOK", "MACHETE", "MARIJUANA", "MYSTERY BOX",
		"PARROT", "PIRANHA", "PORTAL GUN",
		"RAT POISON", "RATS", "RATTLING BOX",
		"SENTRY GUN", "SHOTGUN", "SOAP", "STRAP-ONS", "STUFFED TOY",
		"TABLE", "TV SET", "TOWELS", "TRENCHCOAT",
		"UMBRELLA",
		"WARDROBE", "WHIP", "WIRE"
	};

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
			// Pick a random pkg name
			items.Add(pkgNames[Mathf.RoundToInt(Random.Range(0, pkgNames.Length - 1))]);
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
