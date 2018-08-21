using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public Animator pointerAnim;
	Button[] menuButtons;
	int buttonIndex = 0;

	void Awake() {
		menuButtons = GetComponentsInChildren<Button>();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			if (buttonIndex <= 0) return;
			pointerAnim.SetInteger("selectOption", --buttonIndex);
		} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			if (buttonIndex >= 3) return;
			pointerAnim.SetInteger("selectOption", ++buttonIndex);
		}
	}

	public void PlayGame() {
		// Load next level in queue
		Debug.Log("Starting game");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void GoToSettings() {
		Debug.Log("Opening Settings menu");
	}

	public void GoToCredits() {
		Debug.Log("Opening Credits");
	}

	public void QuitGame() {
		Debug.Log("Quitting game");
		Application.Quit();
	}
	public void ToggleOn() {
		gameObject.SetActive(gameObject.activeSelf);
	}
}
