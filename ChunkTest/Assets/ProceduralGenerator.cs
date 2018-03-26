using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour {
    private static readonly int MAP_SIZE = 10;
    private int chunkSize;
    private Vector3 startOrigin;
    public SeedGenerator Seed;

    //Maps offset => chunk
    private Dictionary<Vector3, Chunk> chunks = new Dictionary<Vector3, Chunk>();

    /**
	 * Initalises the procedual generator
	 * @param int size The Chunk size
	 * @param SeedGenerator seed The seed generator
	 * @return Vector3 Returns the starting origin
	 */
    public Vector3 initalise(int size, SeedGenerator seed) {
        this.Seed = seed;
        Random.InitState(seed.ChunkSeed);

        float originX = Random.Range(320, 960);
        originX -= originX % size;

        //TODO: Water update has trouble when this is set to 0
        //Likely due to inter-chunk math failing on float boundaries
        float originY = 0.0f;

        float originZ = Random.Range(320, 960);
        originX -= originZ % size;

        startOrigin = new Vector3(originX, originY, originZ);

        this.chunkSize = size;

        return startOrigin;
    }

    private void storeChunk(Chunk chunk) {
        Vector3 offset = chunk.getOffset();

        chunks[offset] = chunk;
    }

    private bool chunkExists(Vector3 offset) {
        bool exists = chunks.ContainsKey(offset);

        if (exists == false) {
            //Debug.Log ("Offset " + offset.ToString () + " null");
        }

        return exists;
    }

    public Chunk getChunk(Vector3 position) {
        if (chunks.ContainsKey(position) == false) {
            return null;
        }

        return chunks[position];
    }


    private class ChunkPosition {
        public Vector3 position { get; set; }
        public float distance { get; set; }

        public ChunkPosition(Vector3 position, float distance) {
            this.position = position;
            this.distance = distance;
        }

    }


    ChunkPosition closestNullChunk(Vector3 target) {
        ChunkPosition cp = null;
        Vector3 chunkPos = new Vector3(0, 0, 0);

        // Create the surface
        for (int x = -MAP_SIZE; x <= MAP_SIZE; x++) {
            for (int y = -2; y <= 1; y++) {
                for (int z = -MAP_SIZE; z <= MAP_SIZE; z++) {

                    chunkPos.Set(target.x + (chunkSize * x), target.y + (chunkSize * y), target.z + (chunkSize * z));

                    if (chunkExists(chunkPos) == false) {
                        //Chunk is null at this position, compare its distance to the current closest chunk

                        float dist = Vector3.Distance(target, chunkPos);

                        if (cp == null) {
                            cp = new ChunkPosition(chunkPos, dist);
                        } else if (dist < cp.distance) {
                            cp.distance = dist;
                            cp.position = chunkPos;
                        }

                    }

                }
            }
        }

        return cp;
    }

    /**
	 * Generates a new random map around a point
	 * @param Vector3 offset The offset position
	 */
    public bool generateMap(Vector3 playerPos) {
        ChunkPosition cp = closestNullChunk(playerPos);

        if (cp == null) {
            return false;
        }

        Chunk chunk = new Chunk(cp.position, this);
        storeChunk(chunk);
        return true;

        /**

		Random.InitState (Seed.MineralSeed);
		for (int offsetY = 0; offsetY >= -1; offsetY--) {
			//Generate mineral layer
			Vector3 mineralVec = new Vector3 (chunkPos.x, chunkPos.y + (offsetY * Chunk.CHUNK_SIZE), chunkPos.z);

			Dictionary<Mineral.Type, Vector3[]> minerals = this.calculateMinerals ((int)mineralVec.y);

			if (chunkExists (mineralVec) == false) {
				Chunk earth = new Chunk(mineralVec, this, true);
				earth.GenMinerals (minerals);

				storeChunk (earth);
				return true;
			}
		}

		*/
    }

    /**
	 * Second pass fixes holes in the map.
	 */
    public void secondPass() {
        Vector3 adjacentChunkPos = new Vector3(0, 0, 0);
        Vector3 currentChunkPos = new Vector3(0, 0, 0);
        Chunk chunk;
        bool checkNorth = false, checkSouth = false, checkEast = false, checkWest = false;

        foreach (Chunk entry in chunks.Values) {
            if (entry.secondPass) {
                continue;
            }
            checkNorth = false;
            checkSouth = false;
            checkEast = false;
            checkWest = false;
            currentChunkPos = entry.getOffset();
            //If chunk next to this one exists, fix the holes facing that side
            //If the chunk does not exist, just skip
            adjacentChunkPos.Set(currentChunkPos.x, currentChunkPos.y, currentChunkPos.z + chunkSize);
            if (chunkExists(adjacentChunkPos)) {
                checkNorth = true;
            }

            adjacentChunkPos.Set(currentChunkPos.x, currentChunkPos.y, currentChunkPos.z - chunkSize);
            if (chunkExists(adjacentChunkPos)) {
                checkSouth = true;
            }

            adjacentChunkPos.Set(currentChunkPos.x + chunkSize, currentChunkPos.y, currentChunkPos.z);
            if (chunkExists(adjacentChunkPos)) {
                checkEast = true;
            }

            adjacentChunkPos.Set(currentChunkPos.x - chunkSize, currentChunkPos.y, currentChunkPos.z);
            if (chunkExists(adjacentChunkPos)) {
                checkWest = true;
            }

            entry.fillHoles(checkNorth, checkSouth, checkEast, checkWest);
        }
    }

    Vector3 deleteChunk = Vector3.zero;
    bool deleteChunkSet = false;

    public bool garbageCollect(Vector3 playerPos) {

        foreach (Vector3 otherChunk in chunks.Keys) {
            float dist = Vector3.Distance(playerPos, otherChunk);

            if (dist > chunkSize * MAP_SIZE * 2) {
                chunks[otherChunk].Delete();

                deleteChunk = otherChunk;
                deleteChunkSet = true;
                break;
            }
        }

        if (deleteChunkSet == true) {
            chunks.Remove(deleteChunk);
            deleteChunkSet = false;
            return true;
        }

        return false;

    }

    public void waterProcess(Vector3 playerPos) {
        foreach (Vector3 otherChunk in chunks.Keys) {
            Chunk chunk = chunks[otherChunk];
            chunk.waterProcess();
        }
    }

    private Dictionary<Mineral.Type, Vector3[]> calculateMinerals(int y) {
        Dictionary<Mineral.Type, Vector3[]> minerals = new Dictionary<Mineral.Type, Vector3[]>();
        //Debug.Log (y);
        // Generate coal
        if (y < Mineral.getSpawnLayer(Mineral.Type.Coal)) {
            if (Mineral.hasSpawnChance(Mineral.Type.Coal)) {
                Vector3[] positions = this.generateMineralPositions(Mineral.getRandomSize(Mineral.Type.Coal));

                minerals.Add(Mineral.Type.Coal, positions);
            }
        }
        if (y < Mineral.getSpawnLayer(Mineral.Type.Iron)) {
            if (Mineral.hasSpawnChance(Mineral.Type.Iron)) {
                Vector3[] positions = this.generateMineralPositions(Mineral.getRandomSize(Mineral.Type.Iron));

                minerals.Add(Mineral.Type.Iron, positions);
            }
        }

        return minerals;
    }

    private Vector3[] generateMineralPositions(int size) {
        Vector3[] positions = new Vector3[size];

        positions[0] = new Vector3(Random.Range(1, this.chunkSize), Random.Range(1, this.chunkSize), Random.Range(1, this.chunkSize));


        for (int i = 1; i < size; i++) {
            int rx = (int)positions[i - 1].x + Random.Range(-1, 1);
            int ry = (int)positions[i - 1].y + Random.Range(-1, 1);
            int rz = (int)positions[i - 1].z + Random.Range(-1, 1);

            rx = rx < 0 ? rx * -1 : rx;
            ry = ry < 0 ? ry * -1 : ry;
            rz = rz < 0 ? rz * -1 : rz;

            positions[i] = new Vector3(rx, ry, rz);
        }

        return positions;
    }
}