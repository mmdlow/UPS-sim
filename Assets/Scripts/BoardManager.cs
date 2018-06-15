using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

	public int columns = 8;
	public int rows = 8;
	public Count buildingCount = new Count(5, 10);
	public Count roadCount = new Count(1, 5);

	public GameObject[] roadTiles;
	public GameObject[] buildingTiles;

	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3>();

	void InitializeList()
	{
		gridPositions.Clear();
		// Sets up a playable area
		for (int x = 1; x < columns - 1; x++) 
		{
			for (int y = 1; y < rows - 1; y++) 
			{
				gridPositions.Add(new Vector3(x, y, 0f));
			}
		}
	}

	// Sets up a wall around the playable area
	void BoardSetup ()
	{
		boardHolder = new GameObject("Board").transform;
		for (int x = -1; x < columns + 1; x++) 
		{
			for (int y = -1; y < rows + 1; y++)
			{
				GameObject toInstantiate = buildingTiles[Random.Range(0, buildingTiles.Length)];
				if (x == -1 || x == columns || y == -1 || y == rows) 
				{
					// TODO: change floorTiles here to other tiles if you want custom outerwalls
					toInstantiate = buildingTiles[Random.Range(0, buildingTiles.Length)];
				}
                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
				GameObject instance = Instantiate (toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
				instance.transform.SetParent(boardHolder);
			}
		}
	}
        
    //RandomPosition returns a random position from our list gridPositions.
	Vector3 RandomPosition ()
	{
		//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
		int randomIndex = Random.Range (0, gridPositions.Count);
		
		//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
		Vector3 randomPosition = gridPositions[randomIndex];
		
		//Remove the entry at randomIndex from the list so that it can't be re-used.
		gridPositions.RemoveAt (randomIndex);
		
		//Return the randomly selected Vector3 position.
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
	{
		int objectCount = Random.Range(min, max + 1);
		for (int i = 0; i < objectCount; i++) 
		{
			Vector3 randomPosition = RandomPosition();
			GameObject tile = tileArray[Random.Range(0, tileArray.Length)];
			Instantiate(tile, randomPosition, Quaternion.identity);
		}
	}
	public void SetupScene()
	{
		// Creates outer walls and floor
		BoardSetup();

		// Reset gridpositions
		InitializeList();

		// Instantiate a random number of buildingTiles based on min and max
		LayoutObjectAtRandom(buildingTiles, buildingCount.min, buildingCount.max);

	}
}
