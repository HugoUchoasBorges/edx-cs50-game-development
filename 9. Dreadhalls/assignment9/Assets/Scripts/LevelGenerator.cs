using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	public GameObject floorPrefab;
	public GameObject wallPrefab;
	public GameObject ceilingPrefab;

	public GameObject characterController;

	public GameObject floorParent;
	public GameObject wallsParent;

	// allows us to see the maze generation from the scene view
	public bool generateRoof = true;

    // Range 
    [SerializeField] [Range(0, 4)] private int minFloorToDestroy = 1;
    [SerializeField] [Range(4, 50)] private int maxFloorToDestroy = 4;

    // number of times we want to "dig" in our maze
    public int tilesToRemove = 50;

	public int mazeSize;

	// spawns at the end of the maze generation
	public GameObject pickup;

	// this will determine whether we've placed the character controller
	private bool characterPlaced = false;

	// 2D array representing the map
	private bool[,] mapData;

	// we use these to dig through our maze and to spawn the pickup at the end
	private int mazeX = 4, mazeY = 1;

	// List to keep all floor items that can be destroyed
	private List<GameObject> destroyableItems;

	// Use this for initialization
	void Start () {

		// initialize map 2D array
		mapData = GenerateMazeData();

		destroyableItems = new List<GameObject>();

		// create actual maze blocks from maze boolean data
		for (int z = 0; z < mazeSize; z++) {
			for (int x = 0; x < mazeSize; x++) {

				// create floor and ceiling
				GameObject newFloor = CreateChildPrefab(floorPrefab, floorParent, x, 0, z);

				if (mapData[z, x]) {
					CreateChildPrefab(wallPrefab, wallsParent, x, 1, z);
					CreateChildPrefab(wallPrefab, wallsParent, x, 2, z);
					CreateChildPrefab(wallPrefab, wallsParent, x, 3, z);
				} else if (!characterPlaced) {
					
					// place the character controller on the first empty wall we generate
					characterController.transform.SetPositionAndRotation(
						new Vector3(x, 1, z), Quaternion.identity
					);

					// flag as placed so we never consider placing again
					characterPlaced = true;
				}
				else
				{
					// Adds the new instantiated floor to a list of destroyable Items (it isn't under a Wall and the player isn't on top of it)
					destroyableItems.Add(newFloor);
				}

				if (!mapData[z, x] && !characterPlaced)
					destroyableItems.Add(newFloor);

				if (generateRoof) {
					CreateChildPrefab(ceilingPrefab, wallsParent, x, 4, z);
				}
			}
		}

        // ASSIGNMENT -- Destroy some floorPrefabs
        int[] floorIdxToRemove = new int[(int)(Random.Range(minFloorToDestroy, maxFloorToDestroy + 1))];

        for (int i = 0; i < floorIdxToRemove.Length; i++)
		{
			int selected = (int)Random.Range(0, destroyableItems.Count - 1);

			// Ensure that won't try to destroy the same gameObject twice
			while (floorIdxToRemove.Contains(selected))
				selected = (int)Random.Range(0, destroyableItems.Count - 1);

			floorIdxToRemove[i] = selected;
		}

		DestroyFloorItems(floorIdxToRemove);

		// spawn the pickup at the end
		var myPickup = Instantiate(pickup, new Vector3(mazeX, 1, mazeY), Quaternion.identity);
		myPickup.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
	}

	/// <summary>
	/// Destroys some Floor GameObjects from destroyableItems list
	/// </summary>
	/// <param name="indexes">Vector indexes to destroy</param>
	private void DestroyFloorItems(int[] indexes)
	{
		List<GameObject> listToDestroy = new List<GameObject>();
		// Get all gameObjects to destroy and keep them in a list
		foreach (int index in indexes)
		{
			if (destroyableItems.Count <= index)
				continue;
			listToDestroy.Add(destroyableItems[index]);
		}

		// Destroy all selected gameObjects
		foreach (var item in listToDestroy)
		{
			Destroy(item);
		}
	}

	// generates the booleans determining the maze, which will be used to construct the cubes
	// actually making up the maze
	bool[,] GenerateMazeData() {
		bool[,] data = new bool[mazeSize, mazeSize];

		// initialize all walls to true
		for (int y = 0; y < mazeSize; y++) {
			for (int x = 0; x < mazeSize; x++) {
				data[y, x] = true;
			}
		}

		// counter to ensure we consume a minimum number of tiles
		int tilesConsumed = 0;

		// iterate our random crawler, clearing out walls and straying from edges
		while (tilesConsumed < tilesToRemove) {
			
			// directions we will be moving along each axis; one must always be 0
			// to avoid diagonal lines
			int xDirection = 0, yDirection = 0;

			if (Random.value < 0.5) {
				xDirection = Random.value < 0.5 ? 1 : -1;
			} else {
				yDirection = Random.value < 0.5 ? 1 : -1;
			}

			// random number of spaces to move in this line
			int numSpacesMove = (int)(Random.Range(1, mazeSize - 1));

			// move the number of spaces we just calculated, clearing tiles along the way
			for (int i = 0; i < numSpacesMove; i++) {
				mazeX = Mathf.Clamp(mazeX + xDirection, 1, mazeSize - 2);
				mazeY = Mathf.Clamp(mazeY + yDirection, 1, mazeSize - 2);

				if (data[mazeY, mazeX]) {
					data[mazeY, mazeX] = false;
					tilesConsumed++;
				}
			}
		}

		return data;
	}

	// allow us to instantiate something and immediately make it the child of this game object's
	// transform, so we can containerize everything. also allows us to avoid writing Quaternion.
	// identity all over the place, since we never spawn anything with rotation
	GameObject CreateChildPrefab(GameObject prefab, GameObject parent, int x, int y, int z) {
		var myPrefab = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
		myPrefab.transform.parent = parent.transform;
		return myPrefab;
	}
}
