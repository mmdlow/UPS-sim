using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	bool open = false;
	Button resumeBtn;
	Button quitBtn;
	Animator animator;

	void Awake() {
		resumeBtn = transform.Find("Resume Button").GetComponent<Button>();
		quitBtn = transform.Find("Quit Button").GetComponent<Button>();
		animator = transform.parent.gameObject.GetComponent<Animator>();
	}

	void Start() {
		resumeBtn.onClick.AddListener(GameManager.instance.UnpauseGame);
		quitBtn.onClick.AddListener(QuitGame);
	}

	void QuitGame() {
		Debug.Log("Quitting game");
		SceneManager.LoadScene("Main Menu");
	}

	public void ScreenIn() {
		open = true;
		animator.SetBool("isOpen", true);
	}

	public void ScreenOut() {
		open = false;
		animator.SetBool("isOpen", false);
	}

	public bool IsOpen() {
		return open;
	}
}
