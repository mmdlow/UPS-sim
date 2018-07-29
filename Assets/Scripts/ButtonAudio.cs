using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour {

	public AudioClip hoverSound;
	public AudioClip clickSound;

	public void PlayClickSound() {
		SoundManager.instance.PlaySingle(clickSound);
	}

	public void PlayHoverSound() {
		SoundManager.instance.PlaySingle(hoverSound);
	}
}
