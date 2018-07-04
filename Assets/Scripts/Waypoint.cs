using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Waypoint : MonoBehaviour {

    private static GameObject ground;
	private static List<Vector3Int> reachablePositions = new List<Vector3Int>();
	private BoardManager boardManager;

	public static void Init() {
		ground = GameObject.Find("Ground");
	}

	public static void InitReachablePositions() {
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

	public static void LayoutWaypointAtRandom() {
		Vector3 randomPosition = reachablePositions[Random.Range (0, reachablePositions.Count)];
	}

	public static void LoadNextWaypoint() {
	}
	void Awake () {
		boardManager = BoardManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
	}
     void OnTriggerEnter2D(Collider2D other) {
		Destroy(this.gameObject);
     }
	 void OnDestroy() {
	 }
}
