﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIController : MonoBehaviour {

	TileGrid grid;
	Vector3[] path;
	Vector3 currentWaypoint;
	int targetIndex;

	public float steering = 3f;
	public float speed = 0.01f;
	public int recoverTime = 3;
	public int health = 20;
	public float damageConstant = 2f;

	public AudioClip hornSound;

	bool damaged = false;
	bool beenHitByPlayer = false;
	bool dead = false;
	public SpeechBubbleController sbcPrefab;
	private SpeechBubbleController bubble;

	void Awake() {
		grid = GameObject.Find("Pathfinder").GetComponent<TileGrid>();
	}

	void Start() {
		transform.position = grid.GetRandomRoadPosition();
		currentWaypoint = transform.position;
		GetNewDestination();
		bubble = Instantiate(sbcPrefab, transform.position + new Vector3(0.2f, 0.1f, 0), transform.rotation) as SpeechBubbleController;
		bubble.transform.parent = gameObject.transform;
	}

	void FixedUpdate() {
		if (dead) return;
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
			SoundManager.instance.RandomSfx(hornSound);
			if (!beenHitByPlayer) {
				StatsManager.instance.vehiclesDamaged++;
				beenHitByPlayer = true;
			}
			bubble.SayPreparedMessage(SpeechBubbleController.PreparedMessage.HITVEH);
			damaged = true;
			int damage = Mathf.RoundToInt(damageConstant * col.relativeVelocity.magnitude);
			health -= damage;
			if (health > 0) {
				StartCoroutine("Recover");
				
			} else {
				StopCoroutine("FollowPath");
				StatsManager.instance.vehiclesTotalled++;
                bubble.SayPreparedMessage(SpeechBubbleController.PreparedMessage.KILLVEH);
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
}
