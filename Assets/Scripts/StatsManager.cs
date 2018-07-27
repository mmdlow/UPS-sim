using UnityEngine;
using System.Collections;

public class StatsManager : MonoBehaviour {
	
	public static StatsManager instance = null;
	public int successfulDeliveries = 0;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject); // enforce singleton pattern
		}
	}

    void Start() {
		ItemManager.instance.onItemMissed += UpdateFailedDeliveries;
		successfulDeliveries = ItemManager.instance.items.Count;
    }

	void UpdateFailedDeliveries(GameObject item) {
		// Message manager output
		Debug.Log("Failed to deliver " + item.GetComponent<ItemController>().GetItemName());
		successfulDeliveries--;
	}
    // packages delivered
    // vehicles damaged
    // vehicles totalled
    // pedestrians hit
    // money
}