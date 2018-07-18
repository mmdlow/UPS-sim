using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPinManager : MonoBehaviour {

	public GameObject locationPinBig;
	public GameObject locationPinSmall;

	List<Vector3> locations = new List<Vector3>();
	List<GameObject> smallPins = new List<GameObject>();
	List<GameObject> bigPins = new List<GameObject>();

	Color pinColor;
	float disabledAlpha = 0.3f;

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
		pinColor = bigPin.GetComponentInChildren<SpriteRenderer>().color;
		pinColor.a = disabledAlpha;
		bigPin.GetComponentInChildren<SpriteRenderer>().color = pinColor;
		smallPin.SetActive(false);				
		bigPins.Add(bigPin);
		smallPins.Add(smallPin);
	}

	void UpdatePinPriority(GameObject priorityItem) {
		Debug.Log("Updating pin priority, locations count: " + locations.Count);
		for (int i = 0; i < locations.Count; i++) {
			Vector3 itemLoc = locations[i];
			Vector3 piLoc = priorityItem.GetComponent<ItemController>().GetDropzone().GetComponent<DropzoneController>().transform.position;
			if (itemLoc == piLoc) {
				pinColor.a = 1f;
				bigPins[i].GetComponentInChildren<SpriteRenderer>().color = pinColor;			
				smallPins[i].SetActive(true);

			} else {
				pinColor.a = disabledAlpha;
				bigPins[i].GetComponentInChildren<SpriteRenderer>().color = pinColor;
				smallPins[i].SetActive(false);
			}
		}
	}
}
