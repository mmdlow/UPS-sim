using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour {

	public static string BREAKING, BROKEN, DELIVERED, FLAWLESS, COLLATERAL, KILL;
	public int fadeTime = 5;
	public GameObject deliveryLog;
	public GameObject messageLog;
	Text delOutput;
	Text msgOutput;
	List<GameObject> messages = new List<GameObject>();

	void Awake() {
		BREAKING = "breaking";
		BROKEN = "broken";
		DELIVERED = "delivered";
		FLAWLESS = "flawless";
		COLLATERAL = "collateral";
		KILL = "kill";
		delOutput = deliveryLog.GetComponentInChildren<Text>();
	}

	void Start() {
		ItemManager.instance.onPriorityItemChange += UpdateDeliveryOutput;
		messageLog.SetActive(false);

		delOutput.text = "Select an item in Inventory";
	}

	IEnumerator FadeOut(GameObject msgLog) {
		yield return new WaitForSeconds(fadeTime);
		// In the future, the message log will actually fade out
		msgLog.SetActive(false);
		if (messages.Count > 1) {
			int msgIndex = messages.BinarySearch(msgLog);
			messages.RemoveAt(msgIndex);
			Destroy(msgLog);
			if (messages.Count > 1) {
				for (int i = msgIndex; i < messages.Count - 1; i++) {
					// Move bottom messages, if any, upwards to fill space
					Vector3 oldPos = messages[i].transform.position;
					Vector3 newPos = new Vector3(oldPos.x, oldPos.y + 36, oldPos.z);
					messages[i].transform.position = Vector3.MoveTowards(oldPos, newPos, 5);
				}
			}
		} else {
			messages.RemoveAt(0);
		}
	}

	public void UpdateDeliveryOutput(GameObject item) {
		if (item == null) {
			delOutput.text = "Select a new item!";
			return;
		}
		string itemName = item.GetComponent<ItemController>().GetItemName();
		delOutput.text = "Now delivering: " + itemName;
	}

	public void ItemAlert(GameObject item, string type) {
		string itemName = item.GetComponent<ItemController>().GetItemName();
		
		GameObject newMsgLog;
		if (messages.Count > 0) {
			newMsgLog = Instantiate(messageLog);
			newMsgLog.transform.SetParent(transform);
			Vector3 oldPos = messages[messages.Count - 1].transform.position;
			newMsgLog.transform.position = new Vector3(oldPos.x, oldPos.y - 36, oldPos.z);
			newMsgLog.SetActive(false);
		} else {
			newMsgLog = messageLog;
		}
		msgOutput = newMsgLog.GetComponentInChildren<Text>();
		messages.Add(newMsgLog);
		
		switch (type) {
			case "breaking":
				msgOutput.text = itemName + " is breaking!";
				break;
			case "broken":
				msgOutput.text = itemName + " has broken";
				break;
			case "delivered":
				msgOutput.text = itemName + " delivered";
				break;
			case "flawless":
				msgOutput.text = itemName + " flawlessly delivered!";
				break;
			default:
				Debug.LogWarning("Invalid message type detected");
				break;
		}
		newMsgLog.SetActive(true);
		StartCoroutine(FadeOut(newMsgLog));
	}

	public void VehicleAlert(string type) {

		GameObject newMsgLog;
		if (messages.Count > 0) {
			newMsgLog = Instantiate(messageLog);
			newMsgLog.transform.SetParent(transform);
			Vector3 oldPos = messages[messages.Count - 1].transform.position;
			Debug.Log(oldPos);
			newMsgLog.transform.position = new Vector3(oldPos.x, oldPos.y - 36, 0);
			newMsgLog.SetActive(false);
		} else {
			newMsgLog = messageLog;
		}
		msgOutput = newMsgLog.GetComponentInChildren<Text>();
		messages.Add(newMsgLog);

		switch (type) {
			case "breaking":
				msgOutput.text = "Vehicle heavily damaged!";
				break;
			case "collateral":
				msgOutput.text = "You totalled another car";
				break;
			case "kill":
				msgOutput.text = "You killed a pedestrian";
				break;
			default:
				Debug.LogWarning("Invalid message type detected");
				break;
		}
		newMsgLog.SetActive(true);
		StartCoroutine(FadeOut(newMsgLog));
	}
}
