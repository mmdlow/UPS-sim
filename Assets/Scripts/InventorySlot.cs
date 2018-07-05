using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

	string name;
	public Image icon;
	public Text nameDisplay;

	public void AddItem(string newName, Sprite img) {
		name = newName;
		nameDisplay.text = name;
		icon.sprite = img;
		icon.enabled = true;
		Debug.Log("Added " + name);
	}

	public void ClearSlot() {
		name = null;
		icon.sprite = null;
		icon.enabled = false;
	}
}
