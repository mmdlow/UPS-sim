﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Workshop : MonoBehaviour {

	public AudioClip canBuySound;
	public AudioClip cantBuySound;
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
	Animator animator;

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

		animator = transform.parent.gameObject.GetComponent<Animator>();		
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
		if (GameManager.instance.GetHealth() >= 100) {
			mechDialogue.text = "Already at full health!";
			SoundManager.instance.PlaySingle(cantBuySound);
			return;
		} else {
			if (GameManager.instance.GetMoney() < repairCost) {
				mechDialogue.text = "You don't have enough...";
				SoundManager.instance.PlaySingle(cantBuySound);
			} else {
				mechDialogue.text = "An excellent choice";
				SoundManager.instance.PlaySingle(canBuySound);
				int diff = GameManager.instance.GetMaxHealth() - GameManager.instance.GetHealth();
				if (diff < healthIncr) {
					GameManager.instance.SetHealth(GameManager.instance.GetHealth() + diff);
				} else {
					GameManager.instance.SetHealth(GameManager.instance.GetHealth() + healthIncr);
				}
				GameManager.instance.SetMoney(GameManager.instance.GetMoney() - repairCost);
				healthText.text = GameManager.instance.GetHealth().ToString();
				cashText.text = GameManager.instance.GetMoney().ToString();
			}
		}
	}

	void UpgradeSpeed() {
		if (speedLvl >= maxLvl) {
			mechDialogue.text = "Can't upgrade further";
			SoundManager.instance.PlaySingle(cantBuySound);
			return;
		} else {
			if (GameManager.instance.GetMoney() < upSpeedCost) {
				mechDialogue.text = "You're short on cash";
				SoundManager.instance.PlaySingle(cantBuySound);
			} else {
				mechDialogue.text = "All good";
				SoundManager.instance.PlaySingle(canBuySound);
				speedLvl++;
				PlayerController.instance.acceleration += speedIncr;
				GameManager.instance.SetMoney(GameManager.instance.GetMoney() - upSpeedCost);
				cashText.text = GameManager.instance.GetMoney().ToString();
				RefreshStatLevels();
				// Increase speed of player
			}
		}
	}

	void UpgradeDurability() {
		if (durbLvl >= maxLvl) {
			mechDialogue.text = "Can't upgrade further";
			SoundManager.instance.PlaySingle(cantBuySound);
			return;
		}
		if (GameManager.instance.GetMoney() < upDurbCost) {
			mechDialogue.text = "Get some money first";
			SoundManager.instance.PlaySingle(cantBuySound);
		} else {
			mechDialogue.text = "All good";
			SoundManager.instance.PlaySingle(canBuySound);
			durbLvl++;
			PlayerController.instance.damageConstant -= durbIncr;
            GameManager.instance.SetMoney(GameManager.instance.GetMoney() - upDurbCost);
			cashText.text = GameManager.instance.GetMoney().ToString();
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
		animator.SetBool("isOpen", false);
		// Go to next level
		GameManager.instance.LoadNextLevel();
	}

	public void InitWorkshop() {
		healthText.text = GameManager.instance.GetHealth().ToString();
		cashText.text = GameManager.instance.GetMoney().ToString();
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

		animator.SetBool("isOpen", true);
	}
}