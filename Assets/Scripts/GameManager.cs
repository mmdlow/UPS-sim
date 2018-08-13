using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	public SoundManager soundManager;
	public AudioClip passedSound;
	public AudioClip failedSound;
	public bool doingSetup;
	public bool paused = false;
	private int maxHealth = 100;
	private int health = 100;
	private int money = 0;
	private int level = 1;

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
		//DontDestroyOnLoad(gameObject);

		if (BoardManager.instance == null) {
			Instantiate(boardManager);
		}

		if (StatsManager.instance == null) {
			Instantiate(statsManager);
		}

		if (SoundManager.instance == null) {
			Instantiate(soundManager);
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

		workshop.SetActive(false);
		gameOver.SetActive(false);
	}

	void Start() {
		InitGame();
		LoadLevel(level);
	}

	void InitGame() {
		if (BoardManager.instance == null) {
			Instantiate(boardManager);
		}

		doingSetup = true;

		ItemManager.instance.onItemRemove += CheckDeliveriesDone;
	}
	void LoadLevel(int level) {
		int minItems = 0, maxItems = 0, population = 0;
		switch (level) {
			case 1:
				minItems = 1;
				maxItems = 1;
				population = 100;
				break;
			case 2:
				minItems = 2;
				maxItems = 4;
				population = 130;
				break;
			case 3:
				minItems = 4;
				maxItems = 6;
				population = 160;
				break;
			case 4:
				minItems = 7;
				maxItems = 9;
				population = 190;
				break;
			default:
				minItems = 9;
				maxItems = 9;
				population = 300;
				break;
		}
		ItemManager.instance.ClearAndLoad(minItems, maxItems);
		AIManager.instance.ClearAndLoad(population);
		BoardManager.instance.ClearAndLoad();
		PlayerController.instance.ResetPosition();
		StatsManager.instance.ResetStats();
		MessageManager.instance.Clear();
		
		levelStart.GetComponentInChildren<LevelStart>().ScreenIn();
		Time.timeScale = 0;
		PlayerController.instance.MuteEngine();
	}

	public void LoadNextLevel() {
		LoadLevel(++level);
	}

	public void TriggerLevelPassed() {
		if (StatsManager.instance.failedDeliveries == BoardManager.instance.numLevelItems) {
			Invoke("GameOver", DELAY_END_GAME);
		} else {
			Invoke("LevelPassed", DELAY_END_GAME);
		}
	}

	void CheckDeliveriesDone(GameObject item) {
		if (ItemManager.instance.items.Count == 0) {
			TriggerLevelPassed();
		}
	}

	void LevelPassed() {
		Time.timeScale = 0;
		PlayerController.instance.MuteEngine();
		SoundManager.instance.PlaySingle(passedSound);
		levelPassed.GetComponentInChildren<LevelPassed>().InitScreen();
	}

	void PauseGame() {
		paused = true;
		Time.timeScale = 0;
		inventory.SetActive(false);
		worldmap.SetActive(false);
		minimap.SetActive(true);
		messages.SetActive(true);
		SoundManager.instance.PauseSound();
		PlayerController.instance.MuteEngine();
		pauseScreen.GetComponentInChildren<PauseMenu>().ScreenIn();
	}

	public void UnpauseGame() {
		paused = false;
		Time.timeScale = 1;
		pauseScreen.GetComponentInChildren<PauseMenu>().ScreenOut();
		pauseScreen.transform.Find("Pause Menu").gameObject.SetActive(true);
		pauseScreen.transform.Find("Settings Menu").gameObject.SetActive(false);
		SoundManager.instance.UnpauseSound();
		PlayerController.instance.UnmuteEngine();
	}

	public void GameOver() {
		SoundManager.instance.PlaySingle(failedSound);
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
		levelStart.GetComponentInChildren<LevelStart>().ScreenOut();
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

			if (Input.GetButtonDown("Pause")) {
				GameObject settingsMenu = pauseScreen.transform.Find("Settings Menu").gameObject;
				// If settings is open, go back to main pause menu
				if (settingsMenu.activeSelf) {
					settingsMenu.SetActive(false);
					pauseScreen.transform.Find("Pause Menu").gameObject.SetActive(true);
					return;
				}
				if (!pauseScreen.GetComponentInChildren<PauseMenu>().IsOpen()) {
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
	public int SetHealth(int health) { this.health = health; return this.health; }
	public int SetMoney(int money) { this.money = money; return this.money; }
	public int SetLevel(int level) { this.level = level; return this.level; }
}
