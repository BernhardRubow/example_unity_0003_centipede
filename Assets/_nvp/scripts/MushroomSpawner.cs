using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomSpawner : MonoBehaviour {

	// +++ public fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++  
	public int mushroomPercentage;  
	public int width;
	public int height;
	public GameObject[] mushroomPrefabs;
	public GameObject parent;
	



	// +++ editor fields ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	// +++ private fields +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++    
    // +++ unity callbacks ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void Start () {
		Map.width = width;
		Map.height = height;
		Map.offsetX = -width / 2;
		Map.offsetY = -height / 2;
		Map.grid = new bool[width,height];
		for(int y = 4; y < height-5; y++){
			for(int x = 0; x < width; x++){
				var rnd = Random.value;
				if(rnd < mushroomPercentage /100f) SpawnMushroom(x, y);
			}
		}
	}
    
    
    
    
    // +++ event handler ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	// +++ class methods ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	void SpawnMushroom(int x, int y){
		// get random mushroom prefab
		int index = Random.Range(0, mushroomPrefabs.Length);
		GameObject mushroomPrefab = mushroomPrefabs[index];

		// spawn mushroom prefab
		GameObject m = Instantiate(
			mushroomPrefab, 
			new Vector3(x + Map.offsetX + 0.5f, y + Map.offsetY + 0.5f, 0), 
			Quaternion.identity,
			parent.transform);

		// place mushroom on map
		Map.grid[x, y] = true;
	}
}

public static class Map{
	public static bool[,] grid;
    internal static int offsetX;
    internal static int offsetY;
    internal static int width;
    internal static int height;

	public static bool EvaluteTurn(Vector3 pos, Vector3 dir){

		// calculate point to project on grid
		var bodyPartWidth = 0.5f;
		var offset = bodyPartWidth * dir.x ;
		var evaluationX = pos.x + offset;

		// get integer from point
		var x = Mathf.FloorToInt(evaluationX);
		var y = Mathf.FloorToInt(pos.y);

		// project this calculated integer point to the grid
		// which is offset to the left by half of its width
		var mapX = x - Map.offsetX;
		var mapY = y - Map.offsetY;

		// if point exeeds the boundaries of the grid alway make a turn
		if(mapX < 0 || mapX > Map.width -1) return true;

		// if code reached this point, the point lies in the grid
		// so check, if there is a obstacle in that location
		return Map.grid[mapX, mapY];
	}
}
