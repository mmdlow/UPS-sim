using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour {

	public static string BREAKING, BROKEN, DELIVERED, FLAWLESS, COLLATERAL, KILL;
	public static MessageManager instance = null;
	public int fadeTime = 5;
	List<string> messages = new List<string>();
	Dictionary<string, int> dict = new Dictionary<string, int>();
	Text output;
	private string SayPersistentlyPriorityItemHash = null;
	RectTransform rect;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject); // enforce singleton pattern 
		}

		BREAKING = "breaking";
		BROKEN = "broken";
		DELIVERED = "delivered";
		FLAWLESS = "flawless";
		COLLATERAL = "collateral";
		KILL = "kill";
		output = GetComponent<Text>();
	}

	void Start() {
		rect = transform.GetChild(0).GetComponent<RectTransform>();
		Refresh();

		ItemManager.instance.onPriorityItemChange += SayPersistentlyPriorityItem;

		SayPersistentlyPriorityItem(null);
	}

	// Reads list of messages from messages and refreshes the text output.
	void Refresh() {
		output.text = string.Join("\n", messages.ToArray());
		//rect.sizeDelta = new Vector2(rect.sizeDelta.x, messages.Count*35f);
		//rect.localPosition = new Vector3(rect.localPosition.x, rect.sizeDelta.y, rect.localPosition.z);
	}
	
	// adds message to end of list, returns index
	public string Say(string message, int timeout) {
		messages.Add(message);
        string messageHash = Guid.NewGuid().ToString("n").Substring(0, 8);
		dict[messageHash] = messages.Count - 1;
		if (timeout < int.MaxValue) {
			// set clearAfter
			StartCoroutine(clearAfter(messageHash, timeout));
		}
		Refresh();
		return messageHash;
	}
	
	// persistent
	void SayPersistentlyPriorityItem(GameObject item) {
		string message = (item == null ? "Pick an item from the inventory!" :
										 "Now delivering: " + item.GetComponent<ItemController>().GetItemName());
		if (SayPersistentlyPriorityItemHash == null) {
			SayPersistentlyPriorityItemHash = Say(message, int.MaxValue);
		} else {
			SayAt(SayPersistentlyPriorityItemHash, message);
		}
	}

	void SayAt(string messageHash, string newMessage) {
		int index = dict[messageHash];
		messages[index] = newMessage;
		Refresh();
	}
	IEnumerator clearAfter(string messageHash, int timeout) {
		yield return new WaitForSeconds(timeout);
		int index = dict[messageHash];
		messages.RemoveAt(index);
		// messages after index are automatically moved forward
		// so update the dict
		List<string> keys = new List<string>(dict.Keys);
		foreach(string key in keys) {
			if (dict[key] >= index) {
				dict[key]--;
			}
		}
		Refresh();
	}
}