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
	Text levelNumText;
	Text levelHealthText;
	Text levelMoneyText;
	Text levelItemNamesText;
	Text levelItemDrbText;
	public GameObject levelStart;
	bool doingSetup;
	GameObject inventory;
	GameObject worldmap;
	GameObject minimap;
	Button levelStartBtn;

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
		levelNumText = GameObject.Find("Level Number").GetComponent<Text>();
		levelHealthText = GameObject.Find("Level Health").GetComponent<Text>();
		levelMoneyText = GameObject.Find("Level Money").GetComponent<Text>();
		levelItemNamesText = GameObject.Find("Level Item Names").GetComponent<Text>();
		levelItemDrbText = GameObject.Find("Level Item Drb").GetComponent<Text>();

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
		levelStartBtn = GameObject.Find("Level Start Button").GetComponent<Button>();
		levelStartBtn.onClick.AddListener(HideLevelStart);
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
			worldmap.SetActive(!worldmap.activeInHierarchy);
			minimap.SetActive(!minimap.activeInHierarchy);
		}
	}

	void UpdateGameTime() {
		if (gameTime > 0) gameTime -= Time.deltaTime;
		int actualGameTime = Mathf.RoundToInt(gameTime);
		if (actualGameTime == 0) {} ;
		timer.text = actualGameTime.ToString();
	}
}
