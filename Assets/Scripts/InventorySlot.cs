using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

	GameObject slotItem;
	string itemName;
	public Image icon;
	public Text nameDisplay;
	public Image priorityAlert; // Alert icon
	Button prioritizeBtn; // Slot object itself is a button

	void Start() {
		priorityAlert.enabled = false;
		prioritizeBtn = GetComponentInChildren<Button>();
		prioritizeBtn.interactable = false;
	}

	public GameObject GetSlotItem() {
		return slotItem;
	}

	public void SetItem(GameObject item) {
		slotItem = item;
		itemName = slotItem.GetComponent<ItemController>().GetItemName();
		icon.sprite = slotItem.GetComponent<ItemController>().GetItemIcon();

		nameDisplay.text = itemName + "\n" + slotItem.GetComponent<ItemController>().UpdateIntegrity(0) + "%";
		icon.enabled = true;

		//Only allow slot item to be prioritized if it actually contains an item
		prioritizeBtn.interactable = true;
		prioritizeBtn.onClick.AddListener(() => ItemManager.instance.ChangePriorityItem(item));
	}

	public void ClearSlot() {
		slotItem = null;
		itemName = null;
		nameDisplay.text = null;
		icon.sprite = null;
		icon.enabled = false;
		prioritizeBtn.interactable = false;
	}

	public void UpdateItemIntegrity(int playerDamage) {
		nameDisplay.text = itemName + "\n" + slotItem.GetComponent<ItemController>().UpdateIntegrity(playerDamage) + "%";
	}

	public void SetPriorityAlert() {
		if (slotItem != null) {
			Debug.Log("En route to " + itemName);
			// Display alert icon
			priorityAlert.enabled = true;
			// Prevent slot from being clicked again
			prioritizeBtn.interactable = false;
		}
	}

	public void UnsetPriorityAlert() {
		if (slotItem != null) {
			if (!prioritizeBtn.interactable) {
				priorityAlert.enabled = false;
				prioritizeBtn.interactable = true;
			}
		}
	}
	public bool IsEmpty() {
		return slotItem == null;
	}
}
