
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer am;
    public void SetVolume(float vol) {
        vol = vol * 80f - 80f;
        am.SetFloat("Volume", vol);
    }
	public void ToggleOn() {
		gameObject.SetActive(gameObject.activeSelf);
	}
}
