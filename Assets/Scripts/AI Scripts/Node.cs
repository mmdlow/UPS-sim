﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Node : IHeapItem<Node> {

	public Vector3 worldPosition;
	public int gCost;
	public int hCost;
	public Node parent;
	int heapIndex;

	public Node(Vector3 worldPos) {
		worldPosition = worldPos;
	}

	public float GetX() {
		return worldPosition.x;
	}

	public float GetY() {
		return worldPosition.y;
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo(Node nodeToCompare) {
		int compare = fCost.CompareTo(nodeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}
}
