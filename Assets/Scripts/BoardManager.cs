using UnityEngine;
using UnityEngine.UI;
/*
	BoardManager manages instances of Unity scenes i.e. levels. For every new 
	scene there should be a new instance of boardManager.

	GM and BM follow the singleton pattern, i.e. there can only be one.
 */
public class BoardManager : MonoBehaviour {

	public static BoardManager instance = null;

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
	GameObject levelStart;
	bool doingSetup;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject); // enforce singleton pattern wrt BoardManager
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

		ItemController.InitLevelItems();

		foreach(GameObject item in ItemController.items) {
			levelItemNamesText.text += item.GetComponent<ItemController>().GetItemName() + "\n";
			levelItemDrbText.text += item.GetComponent<ItemController>().GetItemDurability() + "\n";
		}

		levelStart.SetActive(true);
	}

	public void HideLevelStart() {
		levelStart.SetActive(false);
		doingSetup = false;
	}

	void Update() {
		UpdateGameTime();
	}
	void UpdateGameTime() {
		if (gameTime > 0) gameTime -= Time.deltaTime;
		int actualGameTime = Mathf.RoundToInt(gameTime);
		if (actualGameTime == 0) Debug.Log("Game Over");
		timer.text = actualGameTime.ToString();
	}
}
