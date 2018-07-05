using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

	string name;
	public Image icon;

	public void AddItem(string newName, Sprite img) {
		name = newName;
		icon.sprite = img;
		icon.enabled = true;
	}

	public void ClearSlot() {
		name = null;
		icon.sprite = null;
		icon.enabled = false;
	}
}
