﻿using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour 
{
	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count(5,9);
	public Count foodCount = new Count(1,5);
	public GameObject exit;
	public GameObject[] floorTitles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;

	private Transform boardHolder;
	private List<Vector3> gridPosition = new List<Vector3>();

	void InitialiseList()
	{
		gridPosition.Clear ();

		for (int x = 1; x < columns -1; x++) {
			for (int y = 1; y < rows -1; y++) {
				gridPosition.Add (new Vector3(x,y,0.0f));
			}
		}
	}

	void BoardSetup()
	{
		boardHolder = new GameObject ("Board").transform;

		for (int x = -1; x < columns +1; x++) {
			for (int y = -1; y < rows +1; y++) {
				GameObject toInstantiate = floorTitles[Random.Range(0, floorTitles.Length)];
				if (x == -1 || x == columns || y == -1 || y == rows) {
					toInstantiate = outerWallTiles[Random.Range(0,outerWallTiles.Length)]; 
				}
				GameObject instance = Instantiate(toInstantiate, new Vector3(x,y,0.0f), Quaternion.identity) as GameObject;

				instance.transform.SetParent(boardHolder);
			}
		}
	}

	Vector3 RandomPosition()
	{
		int randomIndex = Random.Range (0, gridPosition.Count);
		Vector3 randomPosition = gridPosition [randomIndex];
		gridPosition.RemoveAt (randomIndex);

		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum + 1);

		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition();
			GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	public void SetupScene(int level)
	{
		BoardSetup ();
		InitialiseList ();
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
		LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
		int ennemyCount = (int)Math.Log(level, 2f);
		LayoutObjectAtRandom (enemyTiles, ennemyCount, ennemyCount);
		Instantiate(exit, new Vector3(columns -1, rows -1, 0.0f), Quaternion.identity);
	}
}
