using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
	private const int MAP_SIZE = 5;

	private int chunkSize;

	private Vector3Int startOrigin;

	/**
	 * Initalises the procedual generator
	 * @param int size The Chunk size
	 * @param int seed The seed value for the random generator
	 * @return Vector3Int Returns the starting origin
	 */
	public Vector3Int initalise(int size, int seed = 0)
	{
		Random.InitState (seed);

		startOrigin = new Vector3Int(Random.Range(320, 960), Random.Range(size * 2, size * 5), Random.Range (320, 960));

		this.chunkSize = size;

		return startOrigin;
	}

	/**
	 * Generates a new random map around a point
	 * @param Vector3Int offset The offset position
	 */
	public void generateMap(Vector3Int offset)
	{
		// Create the surface
		for (int x = -MAP_SIZE; x < MAP_SIZE; x++)
		{
			for (int z = -MAP_SIZE; z < MAP_SIZE; z++)
			{
				Chunk surface = new Chunk(new Vector3Int(offset.x + (this.chunkSize * x), offset.y, offset.z + (this.chunkSize * z)));

				for (int y = 1; y < 5; y++)
				{
					Dictionary<Mineral.Type, Vector3Int[]> minerals = this.calculateMinerals (offset.y - (this.chunkSize * y));

					Chunk earth = new Chunk(new Vector3Int(offset.x + (this.chunkSize * x), offset.y - (this.chunkSize * y), offset.z + (this.chunkSize * z)), true, minerals);
				}
			}
		}
	}

	private Dictionary<Mineral.Type, Vector3Int[]> calculateMinerals(int y)
	{
		Dictionary<Mineral.Type, Vector3Int[]> minerals = new Dictionary<Mineral.Type, Vector3Int[]> ();
		Debug.Log (y);
		// Generate coal
		if (y < Mineral.getSpawnLayer (Mineral.Type.Coal))
		{
			if (Mineral.hasSpawnChance (Mineral.Type.Coal))
			{
				Vector3Int[] positions = this.generateMineralPositions (Mineral.getRandomSize (Mineral.Type.Coal));

				minerals.Add (Mineral.Type.Coal, positions);
			}
		}
		if (y < Mineral.getSpawnLayer (Mineral.Type.Iron))
		{
			if (Mineral.hasSpawnChance(Mineral.Type.Iron))
			{
				Vector3Int[] positions = this.generateMineralPositions (Mineral.getRandomSize(Mineral.Type.Iron));

				minerals.Add (Mineral.Type.Iron, positions);
			}
		}

		return minerals;
	}

	private Vector3Int[] generateMineralPositions(int size)
	{
		Vector3Int[] positions = new Vector3Int[size];

		positions[0] = new Vector3Int (Random.Range (1, this.chunkSize), Random.Range (1, this.chunkSize), Random.Range (1, this.chunkSize));


		for (int i = 1; i < size; i++)
		{
			int rx = positions[i - 1].x + Random.Range (-1, 1);
			int ry = positions[i - 1].y + Random.Range (-1, 1);
			int rz = positions[i - 1].z + Random.Range (-1, 1);

			rx = rx < 0 ? rx * -1 : rx;
			ry = ry < 0 ? ry * -1 : ry;
			rz = rz < 0 ? rz * -1 : rz;

			positions [i] = new Vector3Int (rx, ry, rz);
		}

		return positions;
	}
}