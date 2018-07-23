using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour {

	public GameObject deliveryOutput;
	public GameObject messageLog;

	void Start() {
		ItemManager.instance.onPriorityItemChange += UpdateDeliveryOutput;
		messageLog.SetActive(false);

		Text output = deliveryOutput.GetComponentInChildren<Text>();
		output.text = "Select an item in Inventory";
	}

	public void UpdateDeliveryOutput(GameObject item) {
		Text output = deliveryOutput.GetComponentInChildren<Text>();
		if (item == null) {
			output.text = "Select a new item!";
			return;
		}
		string itemName = item.GetComponent<ItemController>().GetItemName();
		output.text = "Now delivering: " + itemName;
	}
}
