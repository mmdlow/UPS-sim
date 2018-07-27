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
		prioritizeBtn.onClick.RemoveAllListeners();
		prioritizeBtn.onClick.AddListener( delegate { ItemManager.instance.ChangePriorityItem(GetSlotItem());});
	}

	public void ClearSlot() {
		slotItem = null;
		itemName = null;
		nameDisplay.text = null;
		icon.sprite = null;
		icon.enabled = false;
		prioritizeBtn.interactable = false;
		prioritizeBtn.onClick.RemoveAllListeners();
        priorityAlert.enabled = false;
	}

	public void UpdateItemIntegrity(float playerDamage) {
		nameDisplay.text = itemName + "\n" + slotItem.GetComponent<ItemController>().UpdateIntegrity(playerDamage) + "%";
	}

	public void SetPriorityAlert() {
		if (slotItem != null) {
			// Display alert icon
			priorityAlert.enabled = true;
			// Prevent slot from being clicked again
			prioritizeBtn.interactable = false;
		}
	}

	public void UnsetPriorityAlert() {
        priorityAlert.enabled = false;
		if (slotItem != null) {
            prioritizeBtn.interactable = true;
		} else {
			prioritizeBtn.interactable = false;
		}
	}
	public bool IsEmpty() {
		return slotItem == null;
	}
}
