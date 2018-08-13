using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPassed : MonoBehaviour {

	public int vehDamagedPenalty = 10;
	public int vehTotalledPenalty = 20;
	public int pedsKilledPenalty = 30;
	public int successBonus = 100;

	GameObject contentParent;
	Button nextBtn;
	Animator animator;

	Text levelNumText;
	Text PDText;
	Text VDText;
	Text VTText;
	Text PHText;
	Text CEText;
	Text TCText;

	void Awake() {
		contentParent = transform.Find("Dynamic Text").gameObject;
		levelNumText = contentParent.transform.Find("Level Number").GetComponent<Text>();
		PDText = contentParent.transform.Find("PD Text").GetComponent<Text>();
		VDText = contentParent.transform.Find("VD Text").GetComponent<Text>();
		VTText = contentParent.transform.Find("VT Text").GetComponent<Text>();
		PHText = contentParent.transform.Find("PH Text").GetComponent<Text>();
		CEText = contentParent.transform.Find("CE Text").GetComponent<Text>();
		TCText = contentParent.transform.Find("TC Text").GetComponent<Text>();
		nextBtn = transform.Find("Next Button").GetComponent<Button>();
		animator = transform.parent.gameObject.GetComponent<Animator>();
	}

	void ExitScreen() {
		nextBtn.onClick.RemoveAllListeners();
		animator.SetBool("isOpen", false);
		GameManager.instance.Upgrade();
	}

	public void InitScreen() {
		levelNumText.text = "DAY " + GameManager.instance.GetLevel() + " SUMMARY";
		int successfulDeliveries = (int) BoardManager.instance.numLevelItems - StatsManager.instance.failedDeliveries;
		PDText.text = successfulDeliveries + "/" + BoardManager.instance.numLevelItems;
		VDText.text = StatsManager.instance.vehiclesDamaged.ToString();
		VTText.text = StatsManager.instance.vehiclesTotalled.ToString();
		PHText.text = StatsManager.instance.pedestriansHit.ToString();
		int moneyEarned = successfulDeliveries * successBonus
						- StatsManager.instance.vehiclesDamaged * vehDamagedPenalty
						- StatsManager.instance.vehiclesTotalled * vehTotalledPenalty
						- StatsManager.instance.pedestriansHit * pedsKilledPenalty;
		if (moneyEarned < 0) moneyEarned = 0;
		int money = GameManager.instance.SetMoney(GameManager.instance.GetMoney() + moneyEarned);
		CEText.text = "$" + moneyEarned;
		TCText.text = "$" + money;

		nextBtn.onClick.AddListener(ExitScreen);

		animator.SetBool("isOpen", true);
	}
}
