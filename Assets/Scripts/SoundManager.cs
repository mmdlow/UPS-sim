using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static Queue<AudioClip> clipQueue;

	public AudioSource efxSource;
	public AudioSource musicSource;
	public static SoundManager instance = null;

	public float lowPitchRange = .9f;
	public float highPitchRange = 1.1f;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);

		clipQueue = new Queue<AudioClip>();
	}

	public void PlaySingle(AudioClip clip) {
		efxSource.PlayOneShot(clip, 0.8f);
	}

	public void RandomSfx(params AudioClip[] clips) {
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);
		int randomIndex = Random.Range(0, clips.Length);
		efxSource.pitch = randomPitch;
		SoundManager.clipQueue.Enqueue(clips[randomIndex]);
		int currCount = clipQueue.Count;
		for (int i = 0; i < currCount; i++) {
			efxSource.PlayOneShot(clipQueue.Dequeue(), 0.8f);
		}
	}
}
