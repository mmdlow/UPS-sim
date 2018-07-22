using UnityEngine;
using UnityEngine.UI;
/*
	BoardManager manages instances of Unity scenes i.e. levels. For every new 
	scene there should be a new instance of boardManager.

	GM and BM follow the singleton pattern, i.e. there can only be one.
 */
public class BoardManager : MonoBehaviour {

	public static BoardManager instance = null;

	public ItemManager itemManager;
	public float gameTime = 90f;
	public Text timer;
	public int health = 100;
	int money = 0;
	int level = 1;
	int repairCost = 100;
	bool doingSetup;
	GameObject levelStart;
	GameObject levelPassed;
	GameObject workshop;
	GameObject gameOver;
	GameObject inventory;
	GameObject worldmap;
	GameObject minimap;
	GameObject messages;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject); // enforce singleton pattern wrt BoardManager
		}

		if (ItemManager.instance == null) {
			Instantiate(itemManager);
		}
	}

	void Start() {

		timer = GameObject.Find("Timer").GetComponent<Text>();

		doingSetup = true;

		levelStart = GameObject.Find("Level Start Screen");
		levelPassed = GameObject.Find("Level Passed Screen");
		workshop = GameObject.Find("Workshop Screen");
		gameOver = GameObject.Find("Game Over Screen");
		levelPassed.SetActive(false);
		workshop.SetActive(false);
		gameOver.SetActive(false);

		// Get content parent for levelStart
		GameObject contentParent = levelStart.transform.Find("Level Image").gameObject;
		Text levelNumText = contentParent.transform.Find("Level Number").GetComponent<Text>();
		Text levelHealthText = contentParent.transform.Find("Level Health").GetComponent<Text>();
		Text levelMoneyText = contentParent.transform.Find("Level Money").GetComponent<Text>();
		Text levelItemNamesText = contentParent.transform.Find("Level Item Names").GetComponent<Text>();
		Text levelItemDrbText = contentParent.transform.Find("Level Item Drb").GetComponent<Text>();

		levelNumText.text = "DAY " + level;
		levelHealthText.text = health.ToString();
		levelMoneyText.text = money.ToString();

		foreach(GameObject item in ItemManager.instance.items) {
			levelItemNamesText.text += item.GetComponent<ItemController>().GetItemName() + "\n";
			levelItemDrbText.text += item.GetComponent<ItemController>().GetItemDurability() + "\n";
		}

		levelStart.SetActive(true);

		inventory = GameObject.Find("Inventory");
		worldmap = GameObject.Find("Worldmap");
		minimap = GameObject.Find("Minimap");
		messages = GameObject.Find("Messages");
		Button levelStartBtn = contentParent.transform.Find("Level Start Button").GetComponent<Button>();
		levelStartBtn.onClick.AddListener(HideLevelStart);
	}

	void LevelPassed() {
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

	void HideLevelStart() {
		doingSetup = false;
		GameObject.Find("Level Start Screen").SetActive(false);
		inventory.SetActive(false);
		worldmap.SetActive(false);
	}

	void Update() {
		if (doingSetup) return;
		UpdateGameTime();
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

	void UpdateGameTime() {
		if (gameTime > 0) gameTime -= Time.deltaTime;
		int actualGameTime = Mathf.RoundToInt(gameTime);
		if (actualGameTime == 0) {} ;
		timer.text = actualGameTime.ToString();
	}
}
