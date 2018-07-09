using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

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
}
