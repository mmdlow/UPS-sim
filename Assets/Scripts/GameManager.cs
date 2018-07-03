using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	private BoardManager boardManager;

	// Use this for initialization
	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject); // enforce singleton pattern wrt GameManger
		}
		DontDestroyOnLoad(gameObject);
		boardManager = GetComponent<BoardManager>();
		InitGame();
	}
	void InitGame() {
		boardManager.SetupLevel();
		boardManager.StartLevel();
	}
}
