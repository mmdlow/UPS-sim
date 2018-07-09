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

	public float gameTime = 90f;
	public Text timer;

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

	void InitGame() {
		if (BoardManager.instance == null) {
			Instantiate(boardManager);
		}
		BoardManager.instance.SetupLevel();
		BoardManager.instance.StartLevel();
	}

	void Update() {
		if (gameTime > 0) gameTime -= Time.deltaTime;
		int actualGameTime = Mathf.RoundToInt(gameTime);
		if (actualGameTime == 0) Debug.Log("Game Over");
		timer.text = actualGameTime.ToString();
	}
}
