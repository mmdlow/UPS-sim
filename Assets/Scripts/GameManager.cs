﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
	GameManager manages every instance of this game. There should only ever one
	gameManager per instance of this program. GameManager can be referenced 
	from any class by calling 'GameManager.instance'.

	GameManager is loaded at the start by Loader which is a component of 
	the main camera.
 */
public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public BoardManager boardManager;
	public StatsManager statsManager;
	public bool doingSetup;
	public bool paused = false;
	private int maxHealth = 100;
	private int health = 100;
	private int money = 0;
	private int level = 1;
	private bool MATURE = true;

	GameObject contentParent;
	GameObject levelStart;
	GameObject levelPassed;
	GameObject workshop;
	GameObject gameOver;
	GameObject pauseScreen;
	GameObject inventory;
	GameObject worldmap;
	GameObject minimap;
	GameObject messages;

	Text levelNumText;
	Text levelHealthText;
	Text levelMoneyText;
	Text levelItemNamesText;
	Text levelItemDrbText;

	private int DELAY_END_GAME = 3;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject); // enforce singleton pattern wrt GameManger
		}
		DontDestroyOnLoad(gameObject);

		if (BoardManager.instance == null) {
			Instantiate(boardManager);
		}

		if (StatsManager.instance == null) {
			Instantiate(statsManager);
		}

		levelStart = GameObject.Find("Level Start Screen");
		levelPassed = GameObject.Find("Level Passed Screen");
		workshop = GameObject.Find("Workshop Screen");
		gameOver = GameObject.Find("Game Over Screen");
		pauseScreen = GameObject.Find("Pause Screen");

		inventory = GameObject.Find("Inventory");
		worldmap = GameObject.Find("Worldmap");
		minimap = GameObject.Find("Minimap");
		messages = GameObject.Find("Messages");

		levelPassed.SetActive(false);
		workshop.SetActive(false);
		gameOver.SetActive(false);
		pauseScreen.SetActive(false);
	}

	void Start() {
		InitGame();
		LoadLevel(level);
	}

	void InitGame() {
		Debug.Log("level" + level);
		if (BoardManager.instance == null) {
			Instantiate(boardManager);
		}
		StatsManager.instance.ResetStats();

		doingSetup = true;

		ItemManager.instance.onItemRemove += TriggerLevelPassed;
	}
	void LoadLevel(int level) {
		ItemManager.instance.ClearAndLoad(1, 1);
		AIManager.instance.ClearAndLoad(100);
		BoardManager.instance.ClearAndLoad();
		PlayerController.instance.ResetPosition();
		
		LevelStart levelStartComp = levelStart.GetComponentInChildren<LevelStart>();
		levelStartComp.ShowScreen();
		levelStart.SetActive(true);
		
	}
	public void LoadNextLevel() {
		LoadLevel(++level);
	}

	void TriggerLevelPassed(GameObject item) {
		if (ItemManager.instance.items.Count == 0) {
			if (StatsManager.instance.successfulDeliveries > 0) {
				Invoke("LevelPassed", DELAY_END_GAME);
			} else {
				Invoke("GameOver", DELAY_END_GAME);
			}
			//ItemManager.instance.onItemRemove -= TriggerLevelPassed;
		}
	}

	void LevelPassed() {
		Debug.Log("level passed");
		LevelPassed levelPassedComp = levelPassed.GetComponentInChildren<LevelPassed>();
		levelPassedComp.InitScreen();
		levelPassed.SetActive(true);	
	}

	void PauseGame() {
		paused = true;
		Time.timeScale = 0;
		pauseScreen.SetActive(true);
		inventory.SetActive(false);
		worldmap.SetActive(false);
		minimap.SetActive(true);
		messages.SetActive(true);
	}

	public void UnpauseGame() {
		paused = false;
		Time.timeScale = 1;
		pauseScreen.SetActive(false);
	}

	public void GameOver() {
		Debug.Log("game over");
		GameOver gameOverComp = gameOver.GetComponentInChildren<GameOver>();
		gameOverComp.InitScreen();
		gameOver.SetActive(true);
		//enabled = false;
	}

	public void Upgrade() {
		Workshop workshopComp = workshop.GetComponentInChildren<Workshop>();
		workshopComp.InitWorkshop();
		workshop.SetActive(true);
	}

	public void HideLevelStart() {
		doingSetup = false;
		levelStart.SetActive(false);
		inventory.SetActive(false);
		worldmap.SetActive(false);
	}

	void Update() {
		if (!doingSetup) {
			if (!paused) {
				if (Input.GetButtonDown("Inventory")) {
					inventory.SetActive(!inventory.activeInHierarchy);
					minimap.SetActive(!minimap.activeInHierarchy);
					messages.SetActive(!messages.activeInHierarchy);

					// Enable pausing when inventory is opened
					if (inventory.activeSelf) {
						worldmap.SetActive(true);
						Time.timeScale = 0;
						//paused = true;
					} else {
						worldmap.SetActive(false);
						Time.timeScale = 1;
						//paused = false;
					}
				}
				// Allow for toggling of worldmap when inventory is active
				if (inventory.activeSelf) {
					if (Input.GetButtonDown("Worldmap")) {
						worldmap.SetActive(!worldmap.activeInHierarchy);
						minimap.SetActive(!minimap.activeInHierarchy);
					}
				}
			}

			//if (health == 0) GameOver();

			if (Input.GetButtonDown("Pause")) {
				if (!pauseScreen.activeSelf) {
					PauseGame();
				} else {
					UnpauseGame();
				}
			}
		}
	}

	public bool IsDoingSetup() {
		return doingSetup;
	}

	public int GetMaxHealth() { return maxHealth; }
	public int GetHealth() { return health; }
	public int GetMoney() { return money; }
	public int GetLevel() { return level; }
	public bool GetMature() { return MATURE; }
	public int SetHealth(int health) { this.health = health; return this.health; }
	public int SetMoney(int money) { this.money = money; return this.money; }
	public int SetLevel(int level) { this.level = level; return this.level; }
}
