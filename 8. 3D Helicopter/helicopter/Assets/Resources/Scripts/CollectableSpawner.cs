using UnityEngine;
using System.Collections;

public class CollectableSpawner : MonoBehaviour {

	[System.Serializable]
    public struct Spawnable
    {
        public GameObject prefab;
        [Range(1, 10)] public int spawnPriority;
    }
    public Spawnable[] spawnables;

	/// <summary>
	/// Get a random Prefab from spawnables array considering each item spawnPriority.
	/// </summary>
	/// <returns>Returns a prefab. Null in case of error</returns>
	private GameObject getRandomSpawnable()
	{
		int totalPriority = 0;
		int count = 0;
		foreach (var spawnable in spawnables)
			totalPriority += spawnable.spawnPriority;

		int rand = Random.Range(1, totalPriority + 1);

		foreach (var spawnable in spawnables)
		{
			count += spawnable.spawnPriority;
			if (rand <= count)
				return spawnable.prefab;
		}
		return null;
	}

    // Use this for initialization
    void Start () {

		// infinite coin spawning function, asynchronous
		StartCoroutine(SpawnCollectables());
	}

	// Update is called once per frame
	void Update () {

	}

	IEnumerator SpawnCollectables() {
		while (true) {

			// number of coins we could spawn vertically
			int collectablesThisRow = Random.Range(1, 4);

			// instantiate all coins in this row separated by some random amount of space
			// Here the code from choosing between the spawnable options
			for (int i = 0; i < collectablesThisRow; i++) {
				Instantiate(getRandomSpawnable(), new Vector3(26, Random.Range(-10, 10), 10), Quaternion.identity);
			}

			// pause 1-5 seconds until the next coin spawns
			yield return new WaitForSeconds(Random.Range(1, 5));
		}
	}
}
