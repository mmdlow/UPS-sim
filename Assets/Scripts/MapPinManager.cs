﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPinManager : MonoBehaviour {

	public GameObject locationPinBig;
	public GameObject locationPinSmall;
	public static MapPinManager instance = null;

	List<Vector3> locations = new List<Vector3>();
	List<GameObject> smallPins = new List<GameObject>();
	List<GameObject> bigPins = new List<GameObject>();

	Color pinColor;
	float disabledAlpha = 0.3f;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject); // enforce singleton pattern
		}
	}
	void Start () {
		ItemManager.instance.onPriorityItemChange += UpdatePinPriority;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddPin(GameObject dropzone) {
		locations.Add(dropzone.transform.position);
		GameObject bigPin = Instantiate(locationPinBig, dropzone.transform.position, Quaternion.identity);
		GameObject smallPin = Instantiate(locationPinSmall, dropzone.transform.position, Quaternion.identity);
		pinColor = bigPin.GetComponentInChildren<SpriteRenderer>().color;
		pinColor.a = disabledAlpha;
		bigPin.GetComponentInChildren<SpriteRenderer>().color = pinColor;
		smallPin.SetActive(false);				
		bigPin.transform.parent = dropzone.transform;
		smallPin.transform.parent = dropzone.transform;
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
	public void RemovePin(GameObject pin) {
		bigPins.Remove(pin);
		smallPins.Remove(pin);
		locations.Remove(pin.transform.position);
	}
}
