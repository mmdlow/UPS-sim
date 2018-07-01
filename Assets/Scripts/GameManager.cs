using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public int count = 4;
	public BoardManager boardManager;

	// Use this for initialization
	void Awake () 
	{
		boardManager = GetComponent<BoardManager>();
		InitGame();
	}
	void InitGame()
	{
		boardManager.SetupLevel();
		boardManager.StartLevel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
