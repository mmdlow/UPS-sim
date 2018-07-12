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
	Text levelNumText;
	Text levelHealthText;
	Text levelMoneyText;
	Text levelItemNamesText;
	Text levelItemDrbText;
	GameObject levelStart;
	bool doingSetup;

	// Use this for initialization
	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject); // enforce singleton pattern wrt GameManger
		}
		DontDestroyOnLoad(gameObject);
		InitGame();
	}

	void Start() {
		if (BoardManager.instance == null) {
			Instantiate(boardManager);
		}
	}

	void InitGame() {
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

		Item.GenerateLevelItems();

		foreach(Item item in Item.items) {
			levelItemNamesText.text += item.GetItemName() + "\n";
			levelItemDrbText.text += item.GetItemDurability() + "\n";
		}

		levelStart.SetActive(true);
	}

	public void HideLevelStart() {
		levelStart.SetActive(false);
		doingSetup = false;
	}
}
