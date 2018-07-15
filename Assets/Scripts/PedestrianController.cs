using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianController : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		anim.SetBool("Alive", true);
	}
	
	// Update is called once per frame
	void OnTriggerEnter2D(Collider2D col) {
		anim.SetBool("Alive", false);
	}
}
