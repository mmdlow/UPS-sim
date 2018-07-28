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
		this.gameObject.GetComponent<MeshRenderer>().sortingLayerName = "Units";
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
		bool mature = GameManager.instance.GetMature();
		Debug.Log(mature);
		Debug.Log(GameManager.instance.MATURE);
		string message = "";
		string[] messages = {};
		switch(pm) {
			case PreparedMessage.HITVEH:
				messages = mature ? new string[] {
					"Oi u fookin wanker",
					"Im gonna fuk u up m8",
					"Hey you, go SUCK an enormous bag of cocks!",
					"I hope you choke on a buffet of dicks.",
					"Who let this retard on the road",
					"Out of my way u fukin twat",
					"Sir, are you blind, retarded, or both?",
					"Kan ni na bei chao chi bai"
				} : new string[] {
					"Oi u bastard",
					"Im gonna beat the piss out of you",
					"Why don't you stick that stick shift up your",
					"Seriously?!",
					"Wa lao eh"
				};
                message = messages[UnityEngine.Random.Range(0, messages.Length)];
				break;
			case PreparedMessage.KILLPED:
				messages = mature ? new string[] {
					"Motherfuc-",
					"Son of a bitc-",
					"Are you fucking kidding m-",
					"Its too early, I haven't even fuc-",
					"Hello darkness my old friend...",
					"Wa si liao ah...",
				} : new string[] {
					"Run over by a truck, just like the gypsy sai-",
					"Hello darkness my old friend...",
					"Why don't you stick that stick shift up your",
					"Seriously?!",
					"Wa si liao ah...",
				};
                message = messages[UnityEngine.Random.Range(0, messages.Length)];
				break;
			case PreparedMessage.KILLVEH:
				messages = mature ? new string[] {
					"Ahh fuck me.",
					"Aww fuck I can't believe you've done this...",
					"Hey fuck you too buddy",
					"Chi Bai crash my car for what",
					"Walao want to die isit",
					"NBCB, new car some more..."
				} : new string[] {
					"New car some more...",
					"I can't believe you've done this...",
					"You son of a",
					"You stupid motherfu-"
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
