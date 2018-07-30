using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleController : MonoBehaviour {

	// Use this for initialization
	TextMesh tm;
	public enum PreparedMessage {
		HITVEH,
		KILLVEH,
		KILLPED,

	}
	void Awake() {
		tm = GetComponent<TextMesh>();
		this.gameObject.GetComponent<MeshRenderer>().sortingLayerName = "Speech bubbles";
	}
	void Start () {
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		transform.eulerAngles = PlayerController.instance.transform.eulerAngles;
	}

	public void Say(string message, int timeout) {
		if (gameObject.activeSelf) return;
		gameObject.SetActive(true);
		tm.text = message;
		transform.eulerAngles = PlayerController.instance.transform.eulerAngles;
		StartCoroutine(clearAfter(timeout));
	}
	public void SayPreparedMessage(PreparedMessage pm) {
		string message = "";
		string[] messages = {};
		switch(pm) {
			case PreparedMessage.HITVEH:
				messages = new string[] {
					"You bastard",
					"Kids these days...",
					"Im gonna beat the piss out of you",
					"Why don't you insert that truck up your",
					"Seriously?!",
					"Wa lao eh",
					"KNNBCCB",
					"NBCB new car some more",
					"You duckface",
					"I'm gonna drive that truck up your a-",
					"How can dis b allow?",
					"Thanks Obama",
				};
                message = messages[UnityEngine.Random.Range(0, messages.Length)];
				break;
			case PreparedMessage.KILLPED:
				messages = new string[] {
					"You Motherf-",
					"Hello darkness my old friend...",
					"Wa si liao ah...",
					"Its too early, I haven't even fu-",
					"Shouldn't have cancelled my policy...",
					"used to be an adventurer like you...",
					"Goodbye cruel world",
					"Kanna sai lah",
					"Just like the gypsy woman said...",
					"Urgh kill me now - wait no",
					"How can dis b allow?",
				};
                message = messages[UnityEngine.Random.Range(0, messages.Length)];
				break;
			case PreparedMessage.KILLVEH:
				messages = new string[] {
					"I can't believe you've done this...",
					"NBCBC new car some more",
					"Walao want to die isit",
					"You son of a",
					"You stupid motherfu-",
				};
                message = messages[UnityEngine.Random.Range(0, messages.Length)];
				break;
			default:
				messages = new string[] {
					""
				};
                message = messages[UnityEngine.Random.Range(0, messages.Length)];
				Debug.LogError("Error: Speechbubble prepared message not found.");
				break;
		}
		Say(message, 3);
	}
	IEnumerator clearAfter(int timeout) {
		yield return new WaitForSeconds(timeout);
		int len = tm.text.Length;
		while (tm.text.Length > 0) {
			yield return new WaitForSeconds(0.05f);
			tm.text = tm.text.Remove(tm.text.Length - 1);
		}
		gameObject.SetActive(false);
	}
}
