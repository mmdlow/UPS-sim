using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldmapUI : MonoBehaviour {

	Animator animator;
	bool open = false;

	// Use this for initialization
	void Awake() {
		animator = gameObject.GetComponent<Animator>();
	}

	public void ScreenIn() {
		open = true;
		animator.SetBool("isOpen", true);
	}

	public void ScreenOut() {
		open = false;
		animator.SetBool("isOpen", false);
	}

	public void ToggleScreen() {
		if (!open) {
			ScreenIn();
		} else {
			ScreenOut();
		}
	}

	public bool IsOpen() {
		return open;
	}
}
