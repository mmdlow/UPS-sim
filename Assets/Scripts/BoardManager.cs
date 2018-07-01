using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour 
{
	[Serializable]
	public class Count 
	{
		public int min;
		public int max;
		public Count(int min, int max) 
		{
			this.min = min;
			this.max = max;
		}
	}

	private Count waypointCount = new Count(1000, 1500);
	public GameObject ground;
	public GameObject[] roadTiles;
	public GameObject[] waypointTiles;

	private List<Vector3Int> reachablePositions = new List<Vector3Int>();

	void InitReachablePositions()
	{
		reachablePositions.Clear();
		Tilemap groundTilemap = ground.transform.Find("Tilemap-ground").gameObject.GetComponent(typeof(Tilemap)) as Tilemap;
		Tilemap buildingTilemap = ground.transform.Find("Tilemap-buildings-base").gameObject.GetComponent(typeof(Tilemap)) as Tilemap;
        TileBase[] groundTilesArray = groundTilemap.GetTilesBlock(groundTilemap.cellBounds);
        TileBase[] buildingTilesArray = buildingTilemap.GetTilesBlock(buildingTilemap.cellBounds);
		for (int x=groundTilemap.origin.x; x<groundTilemap.size.x; x++) {
			for (int y=groundTilemap.origin.y; y<groundTilemap.size.y; y++) {
				TileBase tile = groundTilemap.GetTile(new Vector3Int(x, y, 0));
				if (tile != null && tile.name.StartsWith("road")) {
					reachablePositions.Add(new Vector3Int(x, y, 0));
				}
			}
		}
	}

    //RandomPosition returns a random position from our list reachablePositions.
	Vector3 GetRandomPosition () {
		//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List reachablePositions.
		int randomIndex = Random.Range (0, reachablePositions.Count);
		
		//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List reachablePositions.
		Vector3 randomPosition = reachablePositions[randomIndex];
		
		//Remove the entry at randomIndex from the list so that it can't be re-used.
		reachablePositions.RemoveAt(randomIndex);
		
		//Return the randomly selected Vector3 position.
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max) {
		int objectCount = Random.Range(min, max + 1);
		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = GetRandomPosition();
			GameObject tile = tileArray[Random.Range(0, tileArray.Length)];
			Instantiate(tile, randomPosition, Quaternion.identity);
		}
	}
	public void SetupScene() {
		// Reset reachablePositions
		InitReachablePositions();

		// Instantiate a random number of waypointTiles based on min and max
		LayoutObjectAtRandom(waypointTiles, waypointCount.min, waypointCount.max);
	}
}
