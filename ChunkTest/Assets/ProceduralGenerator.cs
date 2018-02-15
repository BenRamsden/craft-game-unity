using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
	private const int MAP_SIZE = 5;

	/**
	 * Generates a new random world
	 * @param int size The Chunk size
	 * @param int seed The seed value for the random generator
	 */
	public void generateWorld(int size, int seed = 0)
	{
		Random.InitState (seed);

		// The start height for the first chunk
		int startHeight = Random.Range(size * 2, size * 4);
		//int startHeight = 0;
		generateMap (new Vector3Int(0, startHeight, 0), size);

		GameObject player = (GameObject)Instantiate (Resources.Load("Torso"), new Vector3 (size * 2, startHeight, size * 2), Quaternion.identity);
	}

	private void generateMap(Vector3Int offset, int size)
	{
		// Create the surface
		for (int x = -2; x < 2; x++)
		{
			for (int z = -2; z < 2; z++)
			{
				// Randomise the height of the new chunk, height increase between 0 & 3, cliffs could be higher
				int height = Random.Range(1, 3);

				Chunk surface = new Chunk(new Vector3Int(offset.x + (size * x), offset.y, offset.z + (size * z)));

				for (int y = 1; y < 3; y++)
				{
					Chunk earth = new Chunk(new Vector3Int(offset.x + (size * x), offset.y - (size * y), offset.z + (size * z)), true);
				}
			}
		}
	}
}