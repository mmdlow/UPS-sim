using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPedestrianController : MonoBehaviour {

	TileGrid grid;
	Vector3[] path;
	Vector3 currentWaypoint;
	int targetIndex;
	Animator anim;
	bool alive = true;

	public AudioClip splatSound;
	public float steering = 3f;
	public float speed = 0.01f;
	public int recoverTime = 3;
	public int health = 20;
	public float damageConstant = 2f;
	public SpeechBubbleController sbcPrefab;
	private SpeechBubbleController bubble;


	void Awake() {
		grid = GameObject.Find("Pathfinder").GetComponent<TileGrid>();
	}

	void Start() {
		anim = GetComponent<Animator>();
		anim.SetBool("Alive", true);
		transform.position = grid.GetRandomRoadPosition();
		currentWaypoint = transform.position;
		GetNewDestination();
		bubble = Instantiate(sbcPrefab, transform) as SpeechBubbleController;
		bubble.transform.parent = gameObject.transform;
	}

	void FixedUpdate () {
		if (alive) {
			Steer();
			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed);
		} else {
			//rb.velocity = new Vector3(0, 0, 0);
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.GetComponent<AIPedestrianController>() != null) return;
		if (col.name.StartsWith("Tilemap")) return;
		if (alive && col.gameObject.GetComponent<PlayerController>() != null) {
			SoundManager.instance.RandomSfx(splatSound);
			StatsManager.instance.pedestriansHit++;
			MessageManager.instance.SayPreparedMessage(MessageManager.PreparedMessage.KILL, 5);
			bubble.SayPreparedMessage(SpeechBubbleController.PreparedMessage.KILLPED);
			Debug.Log("Pedestrians hit: " + StatsManager.instance.pedestriansHit);
		}
		anim.SetBool("Alive", false);
		alive = false;
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
}
