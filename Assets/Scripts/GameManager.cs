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
	public bool doingSetup;
	public int maxHealth = 100;
	public int health = 100;
	public int money = 0;
	public int level = 1;

	GameObject contentParent;
	GameObject levelStart;
	GameObject levelPassed;
	GameObject workshop;
	GameObject gameOver;
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
		levelPassed.SetActive(false);
		workshop.SetActive(false);
		gameOver.SetActive(false);

		// Get content parent for levelStart
		contentParent = levelStart.transform.Find("Level Image").gameObject;
		levelNumText = contentParent.transform.Find("Level Number").GetComponent<Text>();
		levelHealthText = contentParent.transform.Find("Level Health").GetComponent<Text>();
		levelMoneyText = contentParent.transform.Find("Level Money").GetComponent<Text>();
		levelItemNamesText = contentParent.transform.Find("Level Item Names").GetComponent<Text>();
		levelItemDrbText = contentParent.transform.Find("Level Item Drb").GetComponent<Text>();
		levelNumText.text = "DAY " + level;
		levelHealthText.text = health.ToString();
		levelMoneyText.text = money.ToString();
	}

	void Start() {
		doingSetup = true;

		foreach(GameObject item in ItemManager.instance.items) {
			levelItemNamesText.text += item.GetComponent<ItemController>().GetItemName() + "\n";
			levelItemDrbText.text += item.GetComponent<ItemController>().GetItemDurability() + "\n";
		}

		ItemManager.instance.onItemRemove += TriggerLevelPassed;

		levelStart.SetActive(true);

		inventory = GameObject.Find("Inventory");
		worldmap = GameObject.Find("Worldmap");
		minimap = GameObject.Find("Minimap");
		messages = GameObject.Find("Messages");
		Button levelStartBtn = contentParent.transform.Find("Level Start Button").GetComponent<Button>();
		levelStartBtn.onClick.AddListener(() => HideLevelStart());
	}

	void TriggerLevelPassed(GameObject item) {
		if (ItemManager.instance.items.Count == 0) {
			if (StatsManager.instance.successfulDeliveries > 0) {
				Invoke("LevelPassed", DELAY_END_GAME);
			} else {
				Invoke("GameOver", DELAY_END_GAME);
			}
		}
	}

	void LevelPassed() {
		AIManager.instance.ClearLevelAI();
		LevelPassed levelPassedComp = levelPassed.GetComponentInChildren<LevelPassed>();
		levelPassedComp.InitScreen();
		levelPassed.SetActive(true);	
	}

	public void GameOver() {
		Debug.Log("game over");
		GameObject contentParent = gameOver.transform.Find("Failed Image").Find("Dynamic Text").gameObject;
		Text levelNumText = contentParent.transform.Find("Level Number").GetComponent<Text>();
		Text DeathCauseText = contentParent.transform.Find("Cause Text").GetComponent<Text>();
		Text FinalCashText = contentParent.transform.Find("TC Text").GetComponent<Text>();

		if (level == 1) {
			levelNumText.text = "YOU ENDURED FOR " + level + " DAY";
		} else {
			levelNumText.text = "YOU ENDURED FOR " + level + " DAYS";
		}

		if (health <= 0) {
			DeathCauseText.text = "BEFORE WRECKING YOUR TRUCK";
		}
		if (StatsManager.instance.successfulDeliveries == 0) {
		 	DeathCauseText.text = "BEFORE FAILING ALL DELIVERIES";			
		}
		FinalCashText.text = "$" + money;

		gameOver.SetActive(true);

		Button menuBtn = gameOver.transform.Find("Failed Image").Find("Menu Button").GetComponent<Button>();
		Button restartBtn = gameOver.transform.Find("Failed Image").Find("Restart Button").GetComponent<Button>();
		// menuBtn.onClick.AddListener(go to menu);
		// restartBtn.onClick.AddListener(restart game);
	}

	public void Upgrade() {
		Workshop workshopComp = workshop.GetComponentInChildren<Workshop>();
		workshopComp.InitWorkshop();
		workshop.SetActive(true);
	}

	void HideLevelStart() {
		doingSetup = false;
		GameObject.Find("Level Start Screen").SetActive(false);
		inventory.SetActive(false);
		worldmap.SetActive(false);
	}

	void Update() {
		if (Input.GetButtonDown("Inventory")) {
			inventory.SetActive(!inventory.activeInHierarchy);
			minimap.SetActive(!minimap.activeInHierarchy);
			messages.SetActive(!messages.activeInHierarchy);

			if (inventory.activeSelf) worldmap.SetActive(true);
			else worldmap.SetActive(false);

		}
		// Allow for toggling of worldmap when inventory is active
		if (inventory.activeSelf) {
			if (Input.GetButtonDown("Worldmap")) {
				worldmap.SetActive(!worldmap.activeInHierarchy);
			}
		}
		//if (health == 0) GameOver();
	}

	public bool IsDoingSetup() {
		return doingSetup;
	}
}
