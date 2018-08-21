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

	void OnEnable() {
		StartCoroutine("WaitForPointer");
		menuButtons[buttonIndex].OnSelect(null);
	}

	IEnumerator WaitForPointer() {
		yield return null;
		pointerAnim.SetInteger("selectOption", buttonIndex);
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			if (buttonIndex <= 0) return;
			menuButtons[buttonIndex].OnDeselect(null);
			pointerAnim.SetInteger("selectOption", --buttonIndex);
			menuButtons[buttonIndex].Select();
			menuButtons[buttonIndex].OnSelect(null);
		} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			if (buttonIndex >= 3) return;
			menuButtons[buttonIndex].OnDeselect(null);
			pointerAnim.SetInteger("selectOption", ++buttonIndex);
			menuButtons[buttonIndex].Select();
			menuButtons[buttonIndex].OnSelect(null);
		} else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
			menuButtons[buttonIndex].onClick.Invoke();
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
