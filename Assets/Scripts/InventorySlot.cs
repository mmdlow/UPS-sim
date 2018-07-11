using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

	Item slotItem;

	string itemName;
	public Image icon;
	public Text nameDisplay;
	public Image priorityAlert; // Alert icon
	public Button prioritizeBtn; // Slot object itself is a button

	void Start() {
		priorityAlert.enabled = false;
	}

	public void AddItem(Item item) {

		slotItem = item;
		itemName = slotItem.itemName;
		icon.sprite = slotItem.icon;

		nameDisplay.text = slotItem.itemName;
		icon.enabled = true;
		Debug.Log("Added " + itemName);
	}

	public void ClearSlot() {
		slotItem = null;
		itemName = null;
		icon.sprite = null;
		icon.enabled = false;
	}

	public void Prioritize() {
		if (itemName != null) {
			Debug.Log("En route to " + itemName);
			// Display alert icon
			priorityAlert.enabled = true;
			// Prevent slot from being clicked again
			prioritizeBtn.interactable = false;
		}
	}

	public void Deprioritize() {
		if (itemName != null) {
			if (!prioritizeBtn.interactable) {
				priorityAlert.enabled = false;
				prioritizeBtn.interactable = true;
			}
		}
	}
}
