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
	public AIManager aiManager;
	public float gameTime = 50f;
	public float numLevelItems = 0;
	public Text timer;
	bool timerUp = false;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject); // enforce singleton pattern wrt BoardManager
		}

		if (ItemManager.instance == null) {
			Instantiate(itemManager);
		}

		if (AIManager.instance == null) {
			Instantiate(aiManager);
		}
	}

	void Start() {
		timer = GameObject.Find("Timer").GetComponent<Text>();
		numLevelItems = ItemManager.instance.items.Count;
	}

	void Update() {
		if (GameManager.instance.doingSetup || timerUp) return;
		UpdateGameTime();
	}

	void UpdateGameTime() {
		if (gameTime > 0) gameTime -= Time.deltaTime;
		int actualGameTime = Mathf.RoundToInt(gameTime);
		if (actualGameTime <= 0) {
			timer.text = actualGameTime.ToString();
			StatsManager.instance.failedDeliveries += ItemManager.instance.items.Count;
			GameManager.instance.TriggerLevelPassed();
			timerUp = true;
			return;
		}
		timer.text = actualGameTime.ToString();
	}

	float SetGameTime(float numItems) {
		if (numItems > 1) return 50 + (numItems - 1) * 20;
		else return 50;
	}

	public void ClearAndLoad() {
		numLevelItems = ItemManager.instance.items.Count;
		gameTime = SetGameTime(numLevelItems);
		timerUp = false;
	}
}
