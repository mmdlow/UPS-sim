using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Workshop : MonoBehaviour {

	public int speedLvl = 1;
	public int durbLvl = 1;
	public int maxLvl = 5;
	public int repairCost = 100;
	public int upSpeedCost = 500;
	public int upDurbCost = 500;
	public int healthIncr = 20;
	public float speedIncr = 1;
	public float durbIncr = 0.5f;

	Text healthText;
	Text cashText;
	Text mechDialogue;
	Text speedLvlText;
	Text durbLvlText;

	Button repairBtn;
	Button upSpeedBtn;
	Button upDurbBtn;
	Button nextBtn;

	GameObject speedCostText;
	GameObject durbCostText;

	void Awake() {
		repairBtn = transform.Find("Repair Button").GetComponent<Button>();
		upSpeedBtn = transform.Find("Upgrade Speed Button").GetComponent<Button>();
		upDurbBtn = transform.Find("Upgrade Durability Button").GetComponent<Button>();
		nextBtn = transform.Find("Next Button").GetComponent<Button>();

		healthText = transform.Find("Health Text").GetComponent<Text>();
		cashText = transform.Find("Cash Text").GetComponent<Text>();
		mechDialogue = transform.Find("Mechanic Text").GetComponent<Text>();
		speedLvlText = upSpeedBtn.gameObject.transform.Find("Value").GetComponent<Text>();
		durbLvlText = upDurbBtn.gameObject.transform.Find("Value").GetComponent<Text>();

		speedCostText = transform.Find("Speed Cost").gameObject;
		durbCostText = transform.Find("Durability Cost").gameObject;
	}

	void Start() {
		//InitWorkshop();
	}

	void RefreshStatLevels() {
		if (speedLvl < maxLvl) {
			speedLvlText.text = "LVL " + speedLvl + " -> " + (speedLvl + 1).ToString();
		} else if (speedLvl == maxLvl) {
			speedLvlText.text = "MAX LEVEL";
		}
		if (durbLvl < maxLvl) {
			durbLvlText.text = "LVL " + durbLvl + " -> " + (durbLvl + 1).ToString(); 
		} else if (durbLvl == maxLvl) {
			durbLvlText.text = "MAX LEVEL";
		}
	}

	void Repair() {
		if (GameManager.instance.health >= 100) {
			mechDialogue.text = "Already at full health!";
			return;
		}
		if (GameManager.instance.money < repairCost) {
			mechDialogue.text = "You don't have enough...";
		} else {
			mechDialogue.text = "An excellent choice";
			int diff = GameManager.instance.maxHealth - GameManager.instance.health;
			if (diff < healthIncr) {
				GameManager.instance.health += diff;
			} else {
				GameManager.instance.health += healthIncr;
			}
			GameManager.instance.money -= repairCost;
			healthText.text = GameManager.instance.health.ToString();
			cashText.text = GameManager.instance.money.ToString();
		}
	}

	void UpgradeSpeed() {
		if (speedLvl >= maxLvl) {
			mechDialogue.text = "Can't upgrade further";
			return;
		}
		if (GameManager.instance.money < upSpeedCost) {
			mechDialogue.text = "You're short on cash";
		} else {
			mechDialogue.text = "All good";
			speedLvl++;
			PlayerController.instance.acceleration += speedIncr;
			GameManager.instance.money -= upSpeedCost;
			cashText.text = GameManager.instance.money.ToString();
			RefreshStatLevels();
			// Increase speed of player
		}
	}

	void UpgradeDurability() {
		if (durbLvl >= 5) {
			mechDialogue.text = "Can't upgrade further";
			return;
		}
		if (GameManager.instance.money < upDurbCost) {
			mechDialogue.text = "Get some money first";
		} else {
			mechDialogue.text = "All good";
			durbLvl++;
			PlayerController.instance.damageConstant -= durbIncr;
			GameManager.instance.money -= upDurbCost;
			cashText.text = GameManager.instance.money.ToString();
			RefreshStatLevels();
			// Increase durability of player
		}
	}

	void ExitWorkshop() {
		Debug.Log("Exiting workshop");
		upSpeedBtn.onClick.RemoveAllListeners();
		upDurbBtn.onClick.RemoveAllListeners();
		repairBtn.onClick.RemoveAllListeners();
		nextBtn.onClick.RemoveAllListeners();
		// Go to next level
	}

	public void InitWorkshop() {
		healthText.text = GameManager.instance.health.ToString();
		cashText.text = GameManager.instance.money.ToString();
		mechDialogue.text = "Upgrade and repair here!";
		RefreshStatLevels();
		repairBtn.onClick.AddListener(Repair);
		nextBtn.onClick.AddListener(ExitWorkshop);

		if (speedLvl >= maxLvl) {
			upSpeedBtn.gameObject.SetActive(false);
			speedCostText.SetActive(false);
		}
		if (durbLvl >= maxLvl) {
			upDurbBtn.gameObject.SetActive(false);
			durbCostText.SetActive(false);
		}
		if (durbLvl >= maxLvl && speedLvl >= maxLvl) return;

		upSpeedBtn.onClick.AddListener(UpgradeSpeed);
		upDurbBtn.onClick.AddListener(UpgradeDurability);
	}
}