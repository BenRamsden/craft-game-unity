using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
	private const int MAP_SIZE = 2;

	private const bool DEBUG_DISABLE_MINERAL = true;

	private int chunkSize;

	private Vector3 startOrigin;


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

		//Modulo aligns the block origin with 0,0,0 at increments of size

		float originX = Random.Range (320, 960);
		originX -= originX % size;

		float originY = Random.Range (size * 2, size * 5);
		originY -= originY % size;

		float originZ = Random.Range (320, 960);
		originX -= originZ % size;

		startOrigin = new Vector3(originX, originY, originZ);

		this.chunkSize = size;

		return startOrigin;
	}

	private void storeChunk(Chunk chunk) {
		Vector3 offset = chunk.getOffset ();

		chunks [offset] = chunk;
	}

	private bool chunkExists(Vector3 offset) {
		bool exists = chunks.ContainsKey (offset);

		if (exists == false) {
			//Debug.Log ("Offset " + offset.ToString () + " null");
		}

		return exists;
	}

	public Chunk getChunk(Vector3 position){
		if(chunks.ContainsKey(position) == false) {
			return null;
		}

		return chunks[position];
	}

	/**
	 * Generates a new random map around a point
	 * @param Vector3 offset The offset position
	 */
	public void generateMap(Vector3 playerPos)
	{
		// Create the surface
		for (int x = -MAP_SIZE; x < MAP_SIZE; x++)
		{
			for (int z = -MAP_SIZE; z < MAP_SIZE; z++)
			{
				Vector3 surfaceVec = new Vector3 (playerPos.x + (this.chunkSize * x), playerPos.y, playerPos.z + (this.chunkSize * z));

				if (chunkExists (surfaceVec) == false) {
					Chunk surface = new Chunk(surfaceVec,this);

					storeChunk (surface);
					return;
				}

				if (DEBUG_DISABLE_MINERAL) {
					continue;
				}

				for (int y = 1; y < 5; y++)
				{
					Dictionary<Mineral.Type, Vector3[]> minerals = this.calculateMinerals ((int)playerPos.y - (this.chunkSize * y));

					Vector3 earthVec = new Vector3 (playerPos.x + (this.chunkSize * x), playerPos.y - (this.chunkSize * y), playerPos.z + (this.chunkSize * z));

					if (chunkExists (earthVec) == false) {
						Chunk earth = new Chunk(earthVec, this, true, minerals);

						storeChunk (earth);
						return;
					}

				}
			}
		}

	}

	Vector3 deleteChunk = Vector3.zero;
	bool deleteChunkSet = false;

	public void garbageCollect(Vector3 playerPos) {

		foreach (Vector3 otherChunk in chunks.Keys) {
			float dist = Vector3.Distance (playerPos, otherChunk);

			if (dist > chunkSize * MAP_SIZE * 1.5) {
				chunks [otherChunk].Delete ();

				deleteChunk = otherChunk;
				deleteChunkSet = true;
				break;
			}
		}

		if (deleteChunkSet == true) {
			chunks.Remove (deleteChunk);
			deleteChunkSet = false;
		}

	}

    public void waterProcess(Vector3 playerPos)
    {
        foreach (Vector3 otherChunk in chunks.Keys)
        {
            chunks[otherChunk].waterProcess();
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