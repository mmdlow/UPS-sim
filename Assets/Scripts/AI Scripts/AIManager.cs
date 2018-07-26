using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

	public static AIManager instance = null;
	public GameObject carPrefab;
	public int population = 100;
	public Sprite[] sprites;
	public Queue<GameObject> cars = new Queue<GameObject>();

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
		InitLevelAI();
	}

	public void InitLevelAI() {
		for (int i = 0; i < population; i++) {
			GameObject car = Instantiate(carPrefab);
			car.GetComponent<AIController>().SetSprite(sprites[Random.Range(0, sprites.Length)]);
			cars.Enqueue(car);
		}
	}

	public void ClearLevelAI() {
		while (cars.Count > 0) {
			GameObject car = cars.Dequeue();
			Destroy(car);
		}
	}
}
