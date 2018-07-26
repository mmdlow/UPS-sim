using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGrid : MonoBehaviour {

	private static List<Vector3> roadPositions;
	private static List<Vector3> pavementPositions;
	public bool displayGridGizmos;
	public Tilemap roads;
	public Vector3 worldSize;
	public float tileLength, offset;
	Node[,] grid;
	int gridSizeX, gridSizeY, lowerX, upperX, lowerY, upperY;

	void Awake () {
		roads = GameObject.Find("Ground").transform.Find("Tilemap-road").gameObject.GetComponent<Tilemap>();
		worldSize = roads.size;
		tileLength = 1f;
		offset = tileLength / 2;
		gridSizeX = Mathf.RoundToInt(worldSize.x);
		gridSizeY = Mathf.RoundToInt(worldSize.y);
		lowerX = roads.origin.x;
		lowerY = roads.origin.y;
		upperX = gridSizeX + lowerX;
		upperY = gridSizeY + lowerY;
		CreateGrid();
	}

	void CreateGrid() {
		roadPositions = new List<Vector3>();
		grid = new Node[gridSizeX, gridSizeY];
		for (int x = lowerX; x < upperX; x++) {
			for (int y = lowerY; y < upperY; y++) {
				TileBase tile = roads.GetTile(new Vector3Int(x, y, 0));
				if (tile != null) {
					Vector3 location = new Vector3(x + offset, y + offset, 0);
					grid[x - lowerX, y - lowerY] = new Node(location);
					roadPositions.Add(location);
				}
			}
		}
	}

	public Vector3 GetRandomRoadPosition() {
		int randomIndex = Random.Range(0, roadPositions.Count);
		return roadPositions[randomIndex];
	}

	public int MaxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}

	public List<Node> GetNeighbours(Node node) {
		// Find out where node is in grid
		List<Node> neighbours = new List<Node>();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0 ||
				x == -1 && y == -1 ||
				x == -1 && y == 1 ||
				x == 1 && y == -1 ||
				x == 1 && y == 1) continue; // Back at node itself, skip
				int checkX = Mathf.RoundToInt(node.GetX() + x - offset);
				int checkY = Mathf.RoundToInt(node.GetY() + y - offset);
				if (checkX >= lowerX && checkX < upperX &&
					checkY >= lowerY && checkY < upperY) {
					// node withing tilemap bounds, add to List
					if (grid[checkX - lowerX, checkY - lowerY] == null) continue;
					neighbours.Add(grid[checkX - lowerX, checkY - lowerY]);
				}
			}
		}
		return neighbours;
	}

	public Node NodeFromWorldPoint(Vector3 worldPos) {
		int x = Mathf.RoundToInt(Mathf.Floor(worldPos.x));
		int y = Mathf.RoundToInt(Mathf.Floor(worldPos.y));
		foreach (Node n in grid) {
			return grid[x - lowerX, y - lowerY];
		}
		return null;
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, new Vector3(worldSize.x, worldSize.y, 1));
		if (grid != null && displayGridGizmos) {
			foreach (Node n in grid) {
				if (n == null) continue;
				Gizmos.color = Color.white;
				Gizmos.DrawCube(new Vector3(n.GetX(),
											n.GetY(),
											n.worldPosition.z),
											Vector3.one * (tileLength-.1f));
			}
		}
	}
}
