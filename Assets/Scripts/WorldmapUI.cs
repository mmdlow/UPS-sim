using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldmapUI : MonoBehaviour {

	public GameObject locationPinBig;
	public GameObject locationPinSmall;

	List<Vector3> locations = new List<Vector3>();
	List<GameObject> smallPins = new List<GameObject>();
	List<GameObject> bigPins = new List<GameObject>();

	Color original;

	// Use this for initialization
	void Start () {
		ItemManager.instance.onPriorityItemChange += UpdatePinPriority;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddPin(Vector3 location) {
		locations.Add(location);
		GameObject bigPin = Instantiate(locationPinBig, location, Quaternion.identity);
		GameObject smallPin = Instantiate(locationPinSmall, location, Quaternion.identity);
		bigPins.Add(bigPin);
		smallPins.Add(smallPin);
 		original = bigPin.GetComponentInChildren<Renderer>().material.GetColor("_Color");	
	}

	void UpdatePinPriority(GameObject priorityItem) {
		for (int i = 0; i < locations.Count; i++) {
			Vector3 itemLoc = locations[i];
			Vector3 piLoc = priorityItem.GetComponent<ItemController>().GetDropzone().GetComponent<DropzoneController>().transform.position;
			if (itemLoc == piLoc) {
				Debug.Log("Updating pin color");
				bigPins[i].GetComponentInChildren<Renderer>().material.color = Color.yellow;
				smallPins[i].GetComponentInChildren<Renderer>().material.color = Color.yellow;
			} else {
				bigPins[i].GetComponentInChildren<Renderer>().material.color = original;
				smallPins[i].GetComponentInChildren<Renderer>().material.color = original;
			}
		}
	}
}
