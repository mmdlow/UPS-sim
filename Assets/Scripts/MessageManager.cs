using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour {

	public static string BREAKING, BROKEN, DELIVERED, FLAWLESS, COLLATERAL, KILL;
	public int fadeTime = 5;
	public GameObject msgParent;
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
		
		int msgIndex = messages.IndexOf(msgLog);
		messages.RemoveAt(msgIndex);
		Destroy(msgLog);
		if (messages.Count > 0) {
			for (int i = msgIndex; i < messages.Count; i++) {
				// Move bottom messages, if any, upwards to fill space
				Vector3 oldPos = messages[i].transform.position;
				Vector3 newPos = new Vector3(oldPos.x, oldPos.y + 0.23f, oldPos.z);
				messages[i].transform.position = newPos;
			}
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
		GameObject newMsgLog = AddNewMessageLog();
		msgOutput = newMsgLog.GetComponentInChildren<Text>();
		
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

		GameObject newMsgLog = AddNewMessageLog();
		msgOutput = newMsgLog.GetComponentInChildren<Text>();

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

	private GameObject AddNewMessageLog() {
		GameObject newMsgLog;
		newMsgLog = Instantiate(messageLog);
		newMsgLog.transform.SetParent(msgParent.transform);
		newMsgLog.transform.localPosition = Vector3.zero;
		newMsgLog.transform.localRotation = Quaternion.identity;
		newMsgLog.transform.localScale = Vector3.one;
		if (messages.Count > 0) {
			Vector3 oldPos = messages[messages.Count - 1].transform.position;
			newMsgLog.transform.position = new Vector3(oldPos.x, oldPos.y - 0.23f, oldPos.z);
		} else {
			newMsgLog.transform.position = messageLog.transform.position;
		}
		newMsgLog.SetActive(false);
		messages.Add(newMsgLog);
		return newMsgLog;
	}
}