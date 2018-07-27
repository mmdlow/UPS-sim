using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianController : MonoBehaviour {

	public Transform path;
	public float velocity;
	public float steering;
	public float turnTrigger;

	List<Transform> nodes;
	int currentNode = 0;
	Animator anim;
	Rigidbody2D rb;
	bool alive = true;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		anim.SetBool("Alive", true);

		rb = GetComponent<Rigidbody2D>();
		Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
		nodes = new List<Transform>();
		for (int i = 0; i < pathTransforms.Length; i++) {
			if (pathTransforms[i] != path.transform) nodes.Add(pathTransforms[i]);
		}
	}

	void FixedUpdate () {
		if (alive) {
			Steer();
			Move();
			CheckWayPointDistance();
		} 
	}
	
	void OnTriggerEnter2D(Collider2D col) {
		if (col.name == "Pedestrian Char") return;
		anim.SetBool("Alive", false);
		alive = false;
        rb.velocity = new Vector3(0, 0, 0);
	}

	void Move() {
		// Apply movement
		rb.velocity = transform.up * velocity;
		rb.angularVelocity = 0.0f;
	}

	void Steer() {
		Vector2 newDirection = nodes[currentNode].position - transform.position;
		float nextRotation = 0f;
		if (newDirection.x < 0.0f) {
			nextRotation = Vector2.Angle(Vector2.up, newDirection);
		} else {
			nextRotation = -Vector2.Angle(Vector2.up, newDirection);
		}
		float finalRotation = Mathf.MoveTowardsAngle(transform.localEulerAngles.z, nextRotation, steering);
		transform.eulerAngles = new Vector3(0.0f, 0.0f, finalRotation);
	}

	void CheckWayPointDistance() {
		if (Vector3.Distance(transform.position, nodes[currentNode].position) < 0.3f) {
			if (currentNode == nodes.Count - 1) currentNode = 0;
			else currentNode++;
		}
	}
}
