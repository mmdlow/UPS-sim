using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

	Inventory inventory;
	Item slotItem;
	string itemName;
	public Image icon;
	public Text nameDisplay;
	public Image priorityAlert; // Alert icon
	Button prioritizeBtn; // Slot object itself is a button

	void Start() {
		priorityAlert.enabled = false;
		inventory = Inventory.instance;
		prioritizeBtn = GetComponentInChildren<Button>();
		prioritizeBtn.interactable = false;
	}

	public Item GetSlotItem() {
		return slotItem;
	}

	public void AddItem(Item item) {

		slotItem = item;
		itemName = slotItem.GetItemName();
		icon.sprite = slotItem.GetItemIcon();

		nameDisplay.text = itemName + "\n" + slotItem.UpdateIntegrity(0) + "%";
		icon.enabled = true;

		//Only allow slot item to be prioritized if it actually contains an item
		prioritizeBtn.interactable = true;
		prioritizeBtn.onClick.AddListener(delegate {
			inventory.UpdateItemPriorities(slotItem);
		});
	}

	public void ClearSlot() {
		slotItem = null;
		itemName = null;
		icon.sprite = null;
		icon.enabled = false;
	}

	public void UpdateItemIntegrity(int playerDamage) {
		nameDisplay.text = itemName + "\n" + slotItem.UpdateIntegrity(playerDamage) + "%";
	}

	public void Prioritize() {
		if (slotItem != null) {
			Debug.Log("En route to " + itemName);
			// Display alert icon
			priorityAlert.enabled = true;
			// Prevent slot from being clicked again
			prioritizeBtn.interactable = false;
			// Update the static method in Item script
			Item.ChangePriority(slotItem);
		}
	}

	public void Deprioritize() {
		if (slotItem != null) {
			if (!prioritizeBtn.interactable) {
				priorityAlert.enabled = false;
				prioritizeBtn.interactable = true;
			}
		}
	}
}
