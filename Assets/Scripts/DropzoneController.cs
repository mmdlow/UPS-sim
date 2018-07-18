using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DropzoneController : MonoBehaviour {

	private static List<Vector3Int> reachablePositions;

	public WorldmapUI worldmap;

	// public GameObject locationPinBig;
	// public GameObject locationPinSmall;
	// GameObject bigPin;
	// GameObject smallPin;

	public static void InitReachablePositions() {
		reachablePositions = new List<Vector3Int>();
		GameObject ground = GameObject.Find("Ground");
		Tilemap groundTilemap = ground.transform.Find("Tilemap-ground").gameObject.GetComponent(typeof(Tilemap)) as Tilemap;
		for (int x=groundTilemap.origin.x; x<groundTilemap.size.x; x++) {
			for (int y=groundTilemap.origin.y; y<groundTilemap.size.y; y++) {
				TileBase tile = groundTilemap.GetTile(new Vector3Int(x, y, 0));
				if (tile != null && tile.name.StartsWith("pavement")) {
					reachablePositions.Add(new Vector3Int(x, y, 0));
				}
			}
		}
	}

	public static Vector3 GetRandomPosition() {
        int randomIndex = Random.Range(0, reachablePositions.Count);
        Vector3 randomPosition = reachablePositions[randomIndex];
        reachablePositions.RemoveAt(randomIndex);
        return randomPosition;
	}

	
	void Start() {
		if (reachablePositions == null) {
			InitReachablePositions();
		}
		transform.position = GetRandomPosition();
		worldmap = GameObject.Find("Worldmap").GetComponent<WorldmapUI>();
		worldmap.AddPin(transform.position);
	}

	void OnTriggerEnter2D(Collider2D other) {
		Destroy(this.gameObject);
	}
}
