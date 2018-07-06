using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public List<string> items = new List<string>(); // Randomized item name list
	public List<Sprite> spriteRand = new List<Sprite>(); // Randomized sprite list

	public int maxSpace = 9;
	public int priorityPkg = -1; // Current priority package

	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;

	public delegate void OnPriorityChanged();
	public OnPriorityChanged onPriorityChangedCallback;

	public GameObject GeneratePackagesBtn;
	public static Inventory instance;

	public Sprite[] sprites; // Package sprites to choose from

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

	// Generate random number of packages from 3 to maxSpace
	public void GeneratePackages() {
		int numItems = Mathf.RoundToInt(Random.Range(3, maxSpace));
		if (numItems > maxSpace - items.Count) {
			Debug.Log("Not enough room");
			return;
		}
		for (int i = 0; i < numItems; i++) {
			// Pick a random pkg name
			items.Add(pkgNames[Mathf.RoundToInt(Random.Range(0, pkgNames.Length - 1))]);
			// Pick a random pkg sprite from sprites, add to spriteRand
			spriteRand.Add(sprites[Mathf.RoundToInt(Random.Range(0, sprites.Length - 1))]);
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

	public void UpdatePackagePriorities(int index) {
		priorityPkg = index;
		if (onPriorityChangedCallback != null) {
			onPriorityChangedCallback.Invoke();
		}
	}
}
