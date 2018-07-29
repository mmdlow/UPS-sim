using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
		if (GameManager.instance.GetLevel() == 1) {
			levelNumText.text = "YOU ENDURED FOR JUST " + GameManager.instance.GetLevel() + " DAY";
		} else {
			levelNumText.text = "YOU ENDURED FOR " + GameManager.instance.GetLevel() + " DAYS";
		}

		if (GameManager.instance.GetHealth() <= 0) {
			DeathCauseText.text = "BEFORE WRECKING YOUR TRUCK";
		}
		if (StatsManager.instance.successfulDeliveries == 0) {
		 	DeathCauseText.text = "BEFORE FAILING ALL DELIVERIES";			
		}

		FinalCashText.text = "$" + GameManager.instance.GetMoney();
		menuBtn.onClick.AddListener(() => {
			ExitScreen();
			SceneManager.LoadScene("Main Menu");
			// Back to main menu
		});
		restartBtn.onClick.AddListener(() => {
			ExitScreen();
			SceneManager.LoadScene("Level 1");
			// Restart game;
		});
	}
}
