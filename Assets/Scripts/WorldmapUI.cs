using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WorldmapUI : MonoBehaviour {

	public GameObject locationMarker;

	List<Transform> locations = new List<Transform>();

	// Use this for initialization
	void Start () {
		ItemManager.instance.onItemAdd += AddLocation;
		ItemManager.instance.onItemAdd += RemoveLocation;
		//ItemManager.instance.onItemAdd += UpdatePriorityLocation;
	}

	void AddLocation(GameObject item) {
		GameObject dropzone = item.GetComponent<ItemController>().GetDropzone();
		if (!dropzone) {
			Debug.LogWarning("Error: dropzone does not exist");
			return;
		}
		locations.Add(dropzone.transform);
		GameObject loc = Instantiate(locationMarker, dropzone.transform.position, Quaternion.identity);
		loc.layer = LayerMask.NameToLayer("Worldmap");
	}

	void RemoveLocation(GameObject item) {
		GameObject dropzone = item.GetComponent<ItemController>().GetDropzone();
		if (!dropzone) {
			Debug.LogWarning("Error: dropzone does not exist");
			return;
		}
		locations.Remove(dropzone.transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
