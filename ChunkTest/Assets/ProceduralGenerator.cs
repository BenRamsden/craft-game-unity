using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
	private const int MAP_SIZE = 1;

	private int chunkSize;

	private Vector3 startOrigin;

	private static bool DEBUG_DISABLE_MINERAL = true;

	//Maps offset => chunk
	private Dictionary<Vector3,Chunk> chunks = new Dictionary<Vector3,Chunk> ();

	/**
	 * Initalises the procedual generator
	 * @param int size The Chunk size
	 * @param int seed The seed value for the random generator
	 * @return Vector3 Returns the starting origin
	 */
	public Vector3 initalise(int size, int seed = 0)
	{
		Random.InitState (seed);

		startOrigin = new Vector3(Random.Range(320, 960), Random.Range(size * 2, size * 5), Random.Range (320, 960));

		this.chunkSize = size;

		return startOrigin;
	}

	private void storeChunk(Chunk chunk) {
		Vector3 offset = chunk.getOffset ();

		chunks [offset] = chunk;
	}

	/**
	 * Generates a new random map around a point
	 * @param Vector3 offset The offset position
	 */
	public void generateMap(Vector3 offset)
	{
		// Create the surface
		for (int x = -MAP_SIZE; x < MAP_SIZE; x++)
		{
			for (int z = -MAP_SIZE; z < MAP_SIZE; z++)
			{
				Chunk surface = new Chunk(new Vector3(offset.x + (this.chunkSize * x), offset.y, offset.z + (this.chunkSize * z)));

				storeChunk (surface);

				if (DEBUG_DISABLE_MINERAL) {
					continue;
				}

				for (int y = 1; y < 5; y++)
				{
					Dictionary<Mineral.Type, Vector3[]> minerals = this.calculateMinerals ((int)offset.y - (this.chunkSize * y));

					Chunk earth = new Chunk(new Vector3(offset.x + (this.chunkSize * x), offset.y - (this.chunkSize * y), offset.z + (this.chunkSize * z)), true, minerals);
				
					storeChunk (earth);
				}
			}
		}
	}

	private Dictionary<Mineral.Type, Vector3[]> calculateMinerals(int y)
	{
		Dictionary<Mineral.Type, Vector3[]> minerals = new Dictionary<Mineral.Type, Vector3[]> ();
		Debug.Log (y);
		// Generate coal
		if (y < Mineral.getSpawnLayer (Mineral.Type.Coal))
		{
			if (Mineral.hasSpawnChance (Mineral.Type.Coal))
			{
				Vector3[] positions = this.generateMineralPositions (Mineral.getRandomSize (Mineral.Type.Coal));

				minerals.Add (Mineral.Type.Coal, positions);
			}
		}
		if (y < Mineral.getSpawnLayer (Mineral.Type.Iron))
		{
			if (Mineral.hasSpawnChance(Mineral.Type.Iron))
			{
				Vector3[] positions = this.generateMineralPositions (Mineral.getRandomSize(Mineral.Type.Iron));

				minerals.Add (Mineral.Type.Iron, positions);
			}
		}

		return minerals;
	}

	private Vector3[] generateMineralPositions(int size)
	{
		Vector3[] positions = new Vector3[size];

		positions[0] = new Vector3 (Random.Range (1, this.chunkSize), Random.Range (1, this.chunkSize), Random.Range (1, this.chunkSize));


		for (int i = 1; i < size; i++)
		{
			int rx = (int)positions[i - 1].x + Random.Range (-1, 1);
			int ry = (int)positions[i - 1].y + Random.Range (-1, 1);
			int rz = (int)positions[i - 1].z + Random.Range (-1, 1);

			rx = rx < 0 ? rx * -1 : rx;
			ry = ry < 0 ? ry * -1 : ry;
			rz = rz < 0 ? rz * -1 : rz;

			positions [i] = new Vector3 (rx, ry, rz);
		}

		return positions;
	}
}