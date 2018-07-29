using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour {

	public enum PreparedMessage {
		BREAKING,
		BROKEN,
		DELIVERED,
		FLAWLESS,
		KILL,
		MISSED,
		HITVEH,
		TOTALLEDVEH,
		LOWHEALTH,
		TAKINGDAMAGE
	}
	public static MessageManager instance = null;
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
		output = GetComponent<Text>();
	}

	void Start() {
		//rect = transform.GetChild(0).GetComponent<RectTransform>();
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
	
	// adds message to end of list, returns hash of message that matches its index
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
	public void SayPreparedMessage(PreparedMessage pm, int timeout) {
		string message = "";
		string[] messages;
		switch(pm) {
			case PreparedMessage.BREAKING:
				message = "An item is breaking soon!";
				break;
			case PreparedMessage.BROKEN:
				message = "You broke an item!";
				break;
			case PreparedMessage.DELIVERED:
				message = "Delivery made.";
				break;
			case PreparedMessage.FLAWLESS:
				message = "Delivered flawlessly!";
				break;
			case PreparedMessage.HITVEH:
				messages = new string[] {
					"You hit another vehicle!",
					"Another vehicle was hit!"
				};
				message = messages[UnityEngine.Random.Range(0, messages.Length)];
				break;
			case PreparedMessage.TOTALLEDVEH:
				messages = new string[] {
					"You destroyed another vehicle!",
					"You totalled someone else's car!"
				};
				message = messages[UnityEngine.Random.Range(0, messages.Length)];
				break;
			case PreparedMessage.KILL:
				messages = new string[] {
					"Uh-oh, you ran over somebody!",
					"Welp, there goes another one.",
					"Somebody was run over!",
					"Why are the wheels turning red? Hmm...",
					"Wonder what that bump was...",
					"Let's hope no one saw that."
				};
				message = messages[UnityEngine.Random.Range(0, messages.Length)];
				break;
			case PreparedMessage.MISSED:
				message = "You missed! Slow down next time";
				break;
			case PreparedMessage.LOWHEALTH:
				message = "Your health is low, watch out!";
				break;
			case PreparedMessage.TAKINGDAMAGE:
				messages = new string[] {
					"Be careful!",
					"Your goods are taking damage!",
				};
				message = messages[UnityEngine.Random.Range(0, messages.Length)];
				break;
			default:
				Debug.LogError("Error: Prepared message not found in enums: " + pm);
				break;
		}
		Say(message, timeout);
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

	private void SayAt(string messageHash, string newMessage) {
		int index = dict[messageHash];
		messages[index] = newMessage;
		Refresh();
	}
	private IEnumerator clearAfter(string messageHash, int timeout) {
		yield return new WaitForSecondsRealtime(timeout);
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
	public void Clear() {
		messages.Clear();
		dict.Clear();
		SayPersistentlyPriorityItemHash = null;
		SayPersistentlyPriorityItem(null);
	}
}