using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	GameObject contentParent;
	Button menuBtn;
	Button restartBtn;

	Text levelNumText;
	Text DeathCauseText;
	Text FinalCashText;

	void Awake() {
		contentParent = transform.Find("Dynamic Text").gameObject;
		levelNumText = contentParent.transform.Find("Level Number").GetComponent<Text>();
		DeathCauseText = contentParent.transform.Find("Cause Text").GetComponent<Text>();
		FinalCashText = contentParent.transform.Find("TC Text").GetComponent<Text>();

		menuBtn = transform.Find("Menu Button").GetComponent<Button>();
		restartBtn = transform.Find("Restart Button").GetComponent<Button>();
	}

	void ExitScreen() {
		menuBtn.onClick.RemoveAllListeners();
		restartBtn.onClick.RemoveAllListeners();
		// transform.parent.gameObject.SetActive(false);
	}

	public void InitScreen() {
		if (GameManager.instance.level == 1) {
			levelNumText.text = "YOU ENDURED FOR JUST " + GameManager.instance.level + " DAY";
		} else {
			levelNumText.text = "YOU ENDURED FOR " + GameManager.instance.level + " DAYS";
		}

		if (GameManager.instance.health <= 0) {
			DeathCauseText.text = "BEFORE WRECKING YOUR TRUCK";
		}
		if (StatsManager.instance.successfulDeliveries == 0) {
		 	DeathCauseText.text = "BEFORE FAILING ALL DELIVERIES";			
		}

		FinalCashText.text = "$" + GameManager.instance.money;
		menuBtn.onClick.AddListener(() => {
			ExitScreen();
			// Back to main menu
		});
		restartBtn.onClick.AddListener(() => {
			ExitScreen();
			// Restart game;
		});
	}
}
