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
	public int health = 100;
	int money = 0;
	int level = 1;
	int repairCost = 100;

	GameObject levelStart;
	GameObject levelPassed;
	GameObject workshop;
	GameObject gameOver;
	GameObject inventory;
	GameObject worldmap;
	GameObject minimap;
	GameObject messages;
	public bool doingSetup;
	Text levelNumText;
	Text levelHealthText;
	Text levelMoneyText;
	Text levelItemNamesText;
	Text levelItemDrbText;
	GameObject contentParent;
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
			Invoke("LevelPassed", DELAY_END_GAME);
		}
	}
	void LevelPassed() {
		AIManager.instance.ClearLevelAI();

		GameObject contentParent = levelPassed.transform.Find("Passed Image").Find("Dynamic Text").gameObject;
		Text levelNumText = contentParent.transform.Find("Level Number").GetComponent<Text>();
		Text PDText = contentParent.transform.Find("PD Text").GetComponent<Text>();
		Text VDText = contentParent.transform.Find("VD Text").GetComponent<Text>();
		Text VTText = contentParent.transform.Find("VT Text").GetComponent<Text>();
		Text PHText = contentParent.transform.Find("PH Text").GetComponent<Text>();
		Text CEText = contentParent.transform.Find("CE Text").GetComponent<Text>();
		Text TCText = contentParent.transform.Find("TC Text").GetComponent<Text>();

		levelNumText.text = "DAY " + level + " SUMMARY";
		// PDText.text = "6/9";
		// VDText.text = "12";
		// VTText.text = "5";
		// PHText.text = "23";
		// CEText.text = "$300";
		// TCText.text = "$" + money;

		levelPassed.SetActive(true);
		Button nextBtn = levelPassed.transform.Find("Passed Image").Find("Next Button").GetComponent<Button>();
		nextBtn.onClick.AddListener(Upgrade);
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
		// if (all items destroyed) {
		// 	DeathCauseText.text = "BEFORE FAILING ALL DELIVERIES";			
		// }
		FinalCashText.text = "$" + money;

		gameOver.SetActive(true);

		Button menuBtn = gameOver.transform.Find("Failed Image").Find("Menu Button").GetComponent<Button>();
		Button restartBtn = gameOver.transform.Find("Failed Image").Find("Restart Button").GetComponent<Button>();
		// menuBtn.onClick.AddListener(go to menu);
		// restartBtn.onClick.AddListener(restart game);
	}

	public void Upgrade() {
		GameObject contentParent = workshop.transform.Find("Workshop Image").gameObject;
		Text healthText = contentParent.transform.Find("Health Text").GetComponent<Text>();
		Text cashText = contentParent.transform.Find("Cash Text").GetComponent<Text>();
		Text mechDialogue = contentParent.transform.Find("Mechanic Text").GetComponent<Text>();

		healthText.text = health.ToString();
		cashText.text = money.ToString();
		mechDialogue.text = "Upgrade and repair here!";

		workshop.SetActive(true);
		Button repairBtn = contentParent.transform.Find("Repair Button").GetComponent<Button>();
		Button nextBtn = contentParent.transform.Find("Next Button").GetComponent<Button>();
		repairBtn.onClick.AddListener(() => {
			if (money < 100) {
				mechDialogue.text = "Get yo broke ass outta here";
			} else {
				mechDialogue.text = "An excellent choice";
				health += 20;
			}
		});
		// nextBtn.onClick.AddListener(go to next level);
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

		if (Input.GetButtonDown("Submit")) {
			//messages.GetComponent<MessageManager>().VehicleAlert(MessageManager.BREAKING);
		}
		//if (health == 0) GameOver();
	}

	public bool IsDoingSetup() {
		return doingSetup;
	}
}
