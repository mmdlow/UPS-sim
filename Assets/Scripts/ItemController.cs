﻿using System.Collections;
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
	public GameObject dropzone;

	public void UpdateStats(string name, string drb, Sprite icon) {
		this.itemName = name;
		this.durability = drb;
		this.itemIntegrity = 100;
		this.icon = icon;

		// Set Item Integrity
		switch (durability) {
			case "H":
				this.itemBreakFactor = 1f;
				break;
			case "M":
				this.itemBreakFactor = 1.5f;
				break;
			case "L":
				this.itemBreakFactor = 2f;
				break;
			default:
				Debug.LogWarning("Invalid durability status");
				break;
		}
	}

	void Start() {
		if (dropzonePrefab != null) {
			dropzone = Instantiate(dropzonePrefab);
			dropzone.GetComponent<DropzoneController>().SetItem(this.gameObject);
			dropzone.transform.parent = gameObject.transform;
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

	public GameObject GetDropzone() {
		return dropzone;
	}

	public int UpdateIntegrity(float playerDamage) {
		itemIntegrity -= Mathf.RoundToInt(playerDamage * itemBreakFactor);
		if (itemIntegrity <= 25) Debug.Log("Warning! " + itemName + " at risk");
		return itemIntegrity;
	}
}