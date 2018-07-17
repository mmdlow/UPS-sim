using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPinController : MonoBehaviour {

	public GameObject rotationParent;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = rotationParent.transform.rotation;
	}
}
