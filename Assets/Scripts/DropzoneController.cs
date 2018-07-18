using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DropzoneController : MonoBehaviour {

	private static List<Vector3Int> reachablePositions;

	public WorldmapUI worldmap;
	private BoxCollider2D boxCol;
	private int MIN_HEIGHT = 50;
	private int MAX_HEIGHT = 100;
	private int MIN_WIDTH = 50;
	private int MAX_WIDTH = 100;
    private LineRenderer outline; // Line Renderer

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

		InitBoxCol();
	}

	void InitBoxCol() {
		boxCol = GetComponent<BoxCollider2D>();
		int height = Random.Range(MIN_HEIGHT, MAX_HEIGHT);
		int width = Random.Range(MIN_WIDTH, MAX_WIDTH);
		boxCol.size = new Vector3(height, width, 0f);

        outline = this.gameObject.AddComponent<LineRenderer>();
		Vector3[] positions = new Vector3[5];
        positions[0] = new Vector3(0.5f*(boxCol.offset.x + boxCol.size.x), 0.5f*(boxCol.offset.y - boxCol.size.y), -1);
        positions[1] = new Vector3(0.5f*(boxCol.offset.x - boxCol.size.x), 0.5f*(boxCol.offset.y - boxCol.size.y), -1);
        positions[2] = new Vector3(0.5f*(boxCol.offset.x - boxCol.size.x), 0.5f*(boxCol.offset.y + boxCol.size.y), -1);
        positions[3] = new Vector3(0.5f*(boxCol.offset.x + boxCol.size.x), 0.5f*(boxCol.offset.y + boxCol.size.y), -1);
		positions[4] = positions[0];
		outline.positionCount = positions.Length;
        outline.startWidth = 0.05F;
        outline.endWidth = 0.05F;
		outline.useWorldSpace = false;
		outline.SetPositions(positions);
	}

	void OnTriggerEnter2D(Collider2D other) {
		Destroy(this.gameObject);
	}
}
