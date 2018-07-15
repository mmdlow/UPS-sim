using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour {

	public Transform path;
	public float acceleration;
	public float steering;
	public float turnTrigger;

	public int recoverTime;
	public int health;
	public float damageConstant = 2f;

	List<Transform> nodes;
	int currentNode = 0;
	bool damaged = false;
	Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
		nodes = new List<Transform>();
		for (int i = 0; i < pathTransforms.Length; i++) {
			if (pathTransforms[i] != path.transform) nodes.Add(pathTransforms[i]);
		}
		for (int i = 0; i < nodes.Count; i++) {

		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!damaged) {
			Steer();
			Drive();
			CheckWayPointDistance();
		}
	}

	IEnumerator Recover() {
		yield return new WaitForSeconds(recoverTime);
		damaged = false;
	}

	void OnCollisionEnter2D(Collision2D col) {
		damaged = true;
		int damage = Mathf.RoundToInt(damageConstant * col.relativeVelocity.magnitude);
		health -= damage;
		if (health > 0) {
			StartCoroutine(Recover());
		} else {
			Debug.Log("Vehicle totalled");
		}
	}

	void Drive() {
		// Always accelerate
		float velocity = rb.velocity.magnitude;
		velocity += acceleration;
		// Apply car movement
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
