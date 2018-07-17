using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WorldmapUI : MonoBehaviour {

	public GameObject locationMarker;

	List<GameObject> markers = new List<GameObject>();

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
		GameObject marker = Instantiate(locationMarker, dropzone.transform.position, Quaternion.identity);
		marker.layer = LayerMask.NameToLayer("Worldmap");
		markers.Add(marker);
	}

	void RemoveLocation(GameObject item) {
		GameObject dropzone = item.GetComponent<ItemController>().GetDropzone();
		if (!dropzone) {
			Debug.LogWarning("Error: dropzone does not exist");
			return;
		}
		GameObject markerToRemove = null;
		foreach (GameObject marker in markers) {
			if (marker.transform.position == dropzone.transform.position) {
				markerToRemove = marker;
				break;
			}
		}
		markers.Remove(markerToRemove);
		Destroy(markerToRemove);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
