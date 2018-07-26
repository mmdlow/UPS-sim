using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
	TileGrid grid;

	void Awake() {
		grid = GetComponent<TileGrid>();
	}

	public void FindPath(PathRequest request, Action<PathResult> callback) {

		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		Node startNode = grid.NodeFromWorldPoint(request.pathStart);
		Node targetNode = grid.NodeFromWorldPoint(request.pathEnd);
		startNode.parent = startNode; // NEW!!!!!

		//if (startNode == null || targetNode == null) Debug.LogWarning("Node does not exist");

		Heap<Node> openSet = new Heap<Node>(grid.MaxSize); // Set to be evaluated
		HashSet<Node> closedSet = new HashSet<Node>(); // Set already evaluated
		openSet.Add(startNode);

		while (openSet.Count > 0) {
			Node current = openSet.RemoveFirst();
			closedSet.Add(current);
			if (current == targetNode) {
				pathSuccess = true;
				break; // We finish
			}
			List<Node> neighbours = grid.GetNeighbours(current);
			foreach (Node neighbour in neighbours) {
				if (closedSet.Contains(neighbour)) continue;
				// If new path to neighbour is shorter OR neighbour not in openSet
				int newDist = current.gCost + GetDistance(current, neighbour);
				if (newDist < neighbour.gCost || !openSet.Contains(neighbour)) {
					neighbour.gCost = newDist;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = current;
					if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
					else openSet.UpdateItem(neighbour); // NEW!!!!!
				}
			}
		}
		if (pathSuccess) {
			waypoints = RetracePath(startNode, targetNode);
			pathSuccess = waypoints.Length > 0;
		}
		callback(new PathResult(waypoints, pathSuccess, request.callback));
	}

	Vector3[] RetracePath(Node start, Node end) {
		List<Node> path = new List<Node>();
		path.Add(start);
		Node current = end;

		while (current != start) {
			path.Add(current);
			current = current.parent;
		}
		Vector3[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints);
		return waypoints;
	}

	// Only retain waypoints where the path actually changes directions
	Vector3[] SimplifyPath(List<Node> path) {
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 dirOld = Vector2.zero; // Store direction of last waypoint
		for (int i = 1; i < path.Count; i++) {
			Vector2 dirNew = new Vector2(path[i - 1].GetX() - path[i].GetX(),
										path[i - 1].GetY() - path[i].GetY());
			if (dirNew != dirOld) waypoints.Add(path[i].worldPosition);
			dirOld = dirNew;
		}
		if(path.Count > 0) waypoints.Add(path[path.Count - 1].worldPosition); // Add last node, even if same dir
		return waypoints.ToArray();
	}

	int GetDistance(Node a, Node b) {
		int distX = Mathf.Abs(Mathf.RoundToInt(a.GetX() - b.GetX()));
		int distY = Mathf.Abs(Mathf.RoundToInt(a.GetY() - b.GetY()));

		if (distX > distY) return 2 * distY + distX - distY;
		return 2 * distX + distY - distX;
	}
}
