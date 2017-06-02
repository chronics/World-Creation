using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WorldGenerator : MonoBehaviour {

	//variables
	int[,] map; 											// create a 2D array called map
	public int width, height; 								// the size of the maps X and Y axis

	public string seed; 											// hold word or number for the seed (posibly being replaced with random seed)
	public bool useRandomSeed; 								// bool for program to create a seed rather than using a held seed

	[Range(0,100)]											// the range of the fillpercent 
	public int randomFillPercent; 							// the fill of the map


	// Use this for initialization
	void Start () {
		CreateMap ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			CreateMap ();
		}
	}

	void CreateMap() {
		map = new int[width, height]; 						// create a new map using the width and hight for the X and Y values

		RandomMapFill ();

		for (int i = 0; i < 5; i++) {
			Smoothing ();
		}

		ProcessMap ();

		int borderSize = 1;
		int[,] borderedMap = new int[width + borderSize * 2,height + borderSize * 2];

		for (int x = 0; x < borderedMap.GetLength(0); x ++) {
			for (int y = 0; y < borderedMap.GetLength(1); y ++) {
				if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize) {
					borderedMap[x,y] = map[x-borderSize,y-borderSize];
				}
				else {
					borderedMap[x,y] =1;
				}
			}
		}

		MeshGenerator meshGen = GetComponent<MeshGenerator>();
		meshGen.GenerateMesh(map, 1);

	}

	void RandomMapFill() {
		if (useRandomSeed) { 								// if useRandomSeed is true
			seed = Time.time.ToString ();					// get the time and convert to a string and store as seed
		}

		System.Random pseRandNumGen = new System.Random(seed.GetHashCode()); //get seed string and convert to hash code

		for (int x = 0; x < width; x++){
			for (int y = 0; y < height; y++){
				if (x == 0 || x == width-1 || y == 0 || y == height-1){
					map[x,y] = 1;
				}
				else{
					map [x, y] = (pseRandNumGen.Next (0, 100) < randomFillPercent) ? 1 : 0;
				}
			}
		}

	}

	int GetSurroundingWallcount(int gridX, int gridY){		//find the boarder of the map and fill with tiles creaing a border

		int wallCount = 0;

		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++){
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++){
				if (IsInMap(neighbourX, neighbourY)) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += map [neighbourX, neighbourY];
					}
				} 

				else {
					wallCount++;
				}
			}
		}
		return wallCount;
	}

	void Smoothing() {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int neighbouringWallTiles = GetSurroundingWallcount (x, y);

				if (neighbouringWallTiles > 4) 				//if nieghbouring tile is > 4 fill with tile
					map [x, y] = 1; 
				else if ( neighbouringWallTiles < 4) 		//if nieghbouring tile is < 4 dont fill with tile
					map[x,y] = 0;
			}
		}
	}

	struct Coord{
		public int tileX;
		public int tileY;

		public Coord(int x, int y){
			tileX = x;
			tileY = y;
		}
	}

	List<Coord> GetTiles( int startX, int startY){
		List<Coord> tiles = new List<Coord> ();
		int[,] mapFlags = new int[width, height];
		int tileType = map [startX, startY];

		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (new Coord (startX, startY));
		mapFlags [startX, startY] = 1;

		while (queue.Count > 0) {
			Coord tile = queue.Dequeue ();
			tiles.Add (tile);

			for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
				for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
					if (IsInMap (x, y) && (x == tile.tileX || y == tile.tileY)) {
						if (mapFlags [x, y] == 0 && map [x, y] == tileType) {
							mapFlags [x, y] = 1;
							queue.Enqueue(new Coord(x,y));
						}
					}
				}
			}
		}
		return tiles;
	}

	bool IsInMap(int x, int y) {
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	List<List<Coord>> GetRegions(int tileType) {
		List<List<Coord>> regions = new List<List<Coord>> ();
		int[,] mapFlags = new int[width,height];

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (mapFlags[x,y] == 0 && map[x,y] == tileType) {
					List<Coord> newRegion = GetTiles(x,y);
					regions.Add(newRegion);

					foreach (Coord tile in newRegion) {
						mapFlags[tile.tileX, tile.tileY] = 1;
					}
				}
			}
		}

		return regions;
	}
			
	void ProcessMap() {
		List<List<Coord>> wallRegions = GetRegions (1);
		int wallThresholdSize = 50;

		foreach (List<Coord> wallRegion in wallRegions) {
			if (wallRegion.Count < wallThresholdSize) {
				foreach (Coord tile in wallRegion) {
					map[tile.tileX,tile.tileY] = 0;
				}
			}
		}

		List<List<Coord>> roomRegions = GetRegions (0);
		int roomThresholdSize = 50;

		foreach (List<Coord> roomRegion in roomRegions) {
			if (roomRegion.Count < roomThresholdSize) {
				foreach (Coord tile in roomRegion) {
					map[tile.tileX,tile.tileY] = 1;
				}
			}
		}
	}

	void OnDrawGizmos(){
	
	}

}
