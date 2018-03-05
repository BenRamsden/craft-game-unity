using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
	private const int MAP_SIZE = 2;

	private const bool DEBUG_DISABLE_MINERAL = true;

	private int chunkSize;

	private Vector3 startOrigin;

	public SeedGenerator Seed;


	//Maps offset => chunk
	private Dictionary<Vector3,Chunk> chunks = new Dictionary<Vector3,Chunk> ();

	/**
	 * Initalises the procedual generator
	 * @param int size The Chunk size
	 * @param SeedGenerator seed The seed generator
	 * @return Vector3 Returns the starting origin
	 */
	public Vector3 initalise(int size, SeedGenerator seed)
	{
		this.Seed = seed;
		Random.InitState (seed.ChunkSeed);

		float originX = Random.Range (320, 960);
		originX -= originX % size;

		//TODO: Water update has trouble when this is set to 0
		//Likely due to inter-chunk math failing on float boundaries
		float originY = Random.Range (size * -1, size * 1);
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
	public bool generateMap(Vector3 playerPos)
	{
		// Create the surface
		for (int x = -MAP_SIZE; x < MAP_SIZE; x++)
		{
			for (int z = -MAP_SIZE; z < MAP_SIZE; z++)
			{
				//Generate surface
				Vector3 surfaceVec = new Vector3 (playerPos.x + (this.chunkSize * x), playerPos.y, playerPos.z + (this.chunkSize * z));

				if (chunkExists (surfaceVec) == false) {
					Chunk surface = new Chunk(surfaceVec,this);

					storeChunk (surface);
					return true;
				}

				Random.InitState (Seed.MineralSeed);
				for (int offsetY = 0; offsetY >= -1; offsetY--) {
					//Generate mineral layer
					Vector3 mineralVec = new Vector3 (surfaceVec.x, surfaceVec.y + (offsetY * Chunk.CHUNK_SIZE), surfaceVec.z);

					Dictionary<Mineral.Type, Vector3[]> minerals = this.calculateMinerals ((int)mineralVec.y);

					if (chunkExists (mineralVec) == false) {
						Chunk earth = new Chunk(mineralVec, this, true);
						earth.GenMinerals (minerals);

						storeChunk (earth);
						return true;
					}
				}
			}
		}

		return false;

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
		//Debug.Log (y);
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