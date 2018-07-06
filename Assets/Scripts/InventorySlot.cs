using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

	string pkgName;
	public Image icon;
	public Text nameDisplay;
	public Image priorityAlert; // Alert icon
	public Button prioritizeBtn; // Slot object itself is a button

	void Start() {
		priorityAlert.enabled = false;
	}

	public void AddItem(string newName, Sprite img) {
		pkgName = newName;
		nameDisplay.text = pkgName;
		icon.sprite = img;
		icon.enabled = true;
		Debug.Log("Added " + pkgName);
	}

	public void ClearSlot() {
		pkgName = null;
		icon.sprite = null;
		icon.enabled = false;
	}

	public void Prioritize() {
		if (pkgName != null) {
			Debug.Log("En route to " + pkgName);
			// Display alert icon
			priorityAlert.enabled = true;
			// Prevent slot from being clicked again
			prioritizeBtn.interactable = false;
		}
	}

	public void Deprioritize() {
		if (pkgName != null) {
			if (!prioritizeBtn.interactable) {
				priorityAlert.enabled = false;
				prioritizeBtn.interactable = true;
			}
		}
	}
}
