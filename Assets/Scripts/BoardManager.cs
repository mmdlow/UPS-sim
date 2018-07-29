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
	public float gameTime = 90f;
	private float maxGameTime = 90f;
	public float numLevelItems = 0;
	public Text timer;

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
		if (GameManager.instance.doingSetup) return;
		UpdateGameTime();
	}

	void UpdateGameTime() {
		if (gameTime > 0) gameTime -= Time.deltaTime;
		int actualGameTime = Mathf.RoundToInt(gameTime);
		if (actualGameTime == 0) GameManager.instance.TimerFinished(numLevelItems);
		timer.text = actualGameTime.ToString();
	}
	public void ClearAndLoad() {
		gameTime = maxGameTime;
	}
}
