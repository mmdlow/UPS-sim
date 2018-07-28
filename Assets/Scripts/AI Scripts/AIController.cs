using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIController : MonoBehaviour {

	TileGrid grid;
	Vector3[] path;
	Vector3 currentWaypoint;
	int targetIndex;
	Rigidbody2D rb;

	public float steering = 3f;
	public float speed = 0.01f;
	public int recoverTime = 3;
	public int health = 20;
	public float damageConstant = 2f;

	bool damaged = false;
	bool beenHitByPlayer = false;
	bool dead = false;

	void Awake() {
		rb = GetComponent<Rigidbody2D>();
		grid = GameObject.Find("Pathfinder").GetComponent<TileGrid>();
	}

	void Start() {
		transform.position = grid.GetRandomRoadPosition();
		currentWaypoint = transform.position;
		GetNewDestination();
	}

	void FixedUpdate() {
		if (dead) {
			StopCoroutine("FollowPath");
			return;
		}
		if (!damaged) {
			Steer();
			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed);
		}
	}

	IEnumerator Recover() {
		yield return new WaitForSeconds(recoverTime);
		damaged = false;
	}

	void OnCollisionEnter2D(Collision2D col) {

		if (col.gameObject.GetComponent<PlayerController>() != null && !dead) {
			if (!beenHitByPlayer) {
				StatsManager.instance.vehiclesDamaged++;
				MessageManager.instance.SayPreparedMessage(MessageManager.PreparedMessage.HITVEH, 5);
				Debug.Log("Vehicles hit: " + StatsManager.instance.vehiclesDamaged);
				beenHitByPlayer = true;
			}
			damaged = true;
			int damage = Mathf.RoundToInt(damageConstant * col.relativeVelocity.magnitude);
			health -= damage;
			if (health > 0) {
				StartCoroutine("Recover");
			} else {
				StopCoroutine("FollowPath");
				StatsManager.instance.vehiclesTotalled++;
				MessageManager.instance.SayPreparedMessage(MessageManager.PreparedMessage.TOTALLEDVEH, 5);
				Debug.Log("Vehicles totalled: " + StatsManager.instance.vehiclesTotalled);
				dead = true;
			}
		}
	}

	void GetNewDestination() {
		Vector3 target = grid.GetRandomRoadPosition();
		PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound));
	}

	IEnumerator FollowPath() {
		currentWaypoint = path[0];
		while (true) {
			if (transform.position == currentWaypoint) {
				targetIndex++;
				if (targetIndex >= path.Length) {
					GetNewDestination();
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
			yield return null;
		}
	}

	void Steer() {
		Vector2 newDirection = currentWaypoint - transform.position;
		float nextRotation = 0f;
		if (newDirection.x < 0.0f) {
			nextRotation = Vector2.Angle(Vector2.up, newDirection);
		} else {
			nextRotation = -Vector2.Angle(Vector2.up, newDirection);
		}
		float finalRotation = Mathf.MoveTowardsAngle(transform.localEulerAngles.z, nextRotation, steering);
		transform.eulerAngles = new Vector3(0.0f, 0.0f, finalRotation);
	}

	public void OnPathFound(Vector3[] newPath, bool success) {
		if (success) {
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	public void SetSprite(Sprite carSprite) {
		SpriteRenderer spriteR = gameObject.GetComponent<SpriteRenderer>();
		spriteR.sprite = carSprite;
	}

	// public void OnDrawGizmos() {
	// 	if (path != null) {
	// 		for (int i = targetIndex; i < path.Length; i ++) {
	// 			Gizmos.color = Color.black;
	// 			Gizmos.DrawCube(path[i], Vector3.one);

	// 			if (i == targetIndex) {
	// 				Gizmos.DrawLine(transform.position, path[i]);
	// 			}
	// 			else {
	// 				Gizmos.DrawLine(path[i-1],path[i]);
	// 			}
	// 		}
	// 	}
	// }
}
