using UnityEngine;
using System.Collections;

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
				if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
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

	void OnDrawGizmos(){
	
	}

}
