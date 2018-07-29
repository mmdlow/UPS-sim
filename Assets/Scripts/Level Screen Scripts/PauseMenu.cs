using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public SettingsMenu sm;
	Button resumeBtn;
	Button settingsBtn;
	Button quitBtn;

	void Awake() {
		resumeBtn = transform.Find("Resume Button").GetComponent<Button>();
		settingsBtn = transform.Find("Settings Button").GetComponent<Button>();
		quitBtn = transform.Find("Quit Button").GetComponent<Button>();
	}

	void Start() {
		resumeBtn.onClick.AddListener(GameManager.instance.UnpauseGame);
		settingsBtn.onClick.AddListener(Settings);
		quitBtn.onClick.AddListener(QuitGame);
	}

	void Settings() {
	}

	void QuitGame() {
		Debug.Log("Quitting game");
		SceneManager.LoadScene("Main Menu");
	}
}
