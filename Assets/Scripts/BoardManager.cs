using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	public GameObject ground;
	public GameObject waypoint;
	private GameObject currentWaypoint;
	private List<Vector3Int> reachablePositions = new List<Vector3Int>();
	public int waypointCount = 10;
	private int currWaypointCount = 0;
	void InitReachablePositions() {
		reachablePositions.Clear();
		Tilemap groundTilemap = ground.transform.Find("Tilemap-ground").gameObject.GetComponent(typeof(Tilemap)) as Tilemap;
        // TileBase[] groundTilesArray = groundTilemap.GetTilesBlock(groundTilemap.cellBounds);
		for (int x=groundTilemap.origin.x; x<groundTilemap.size.x; x++) {
			for (int y=groundTilemap.origin.y; y<groundTilemap.size.y; y++) {
				TileBase tile = groundTilemap.GetTile(new Vector3Int(x, y, 0));
				if (tile != null && tile.name.StartsWith("pavement")) {
					reachablePositions.Add(new Vector3Int(x, y, 0));
				}
			}
		}
	}

	void LayoutWaypointAtRandom() {
		Vector3 randomPosition = reachablePositions[Random.Range (0, reachablePositions.Count)];
		currentWaypoint = Instantiate(waypoint, randomPosition, Quaternion.identity);
	}
	public GameObject GetCurrentWaypoint() {
		return currentWaypoint;
	}

	public void LoadNextWaypoint() {
		if (currWaypointCount <= waypointCount) {
			LayoutWaypointAtRandom();
			currWaypointCount++;
		}
	}
	public void SetupLevel() {
		// Reset reachablePositions
		InitReachablePositions();
	}
	public void StartLevel() {
		LoadNextWaypoint();
	}
}
