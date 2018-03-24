using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk{
	public static int CHUNK_SIZE = 8;

	//blocks[,,] is a 3D array in the form [x, y, z]
	private Block[,,] blocks = new Block[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];
	private Vector3 worldOffset;
	private ProceduralGenerator generator;
	public bool secondPass { get; set; }

	public Chunk(Vector3 worldOffset, ProceduralGenerator generator, bool isBelowSurface = false)
	{
		this.worldOffset = worldOffset;
		this.generator = generator;
		secondPass = false;

		if (isBelowSurface) {
			return;
		}

		for (int x = 0; x < CHUNK_SIZE; x++){
			for (int y = 0; y < CHUNK_SIZE; y++){
				for (int z = 0; z < CHUNK_SIZE; z++){
					//Calculate absolute position of block (world space)
					int worldX = (int)worldOffset.x + x;
					int worldY = (int)worldOffset.y + y;
					int worldZ = (int)worldOffset.z + z;

					//Generate a scaled X and Z for input into PerlinNoise function
					float perlinX = ((float)worldX) / CHUNK_SIZE;
					float perlinZ = ((float)worldZ) / CHUNK_SIZE;

					float perlinTerrainType = Mathf.PerlinNoise (perlinX / 30, perlinZ / 30);

					float perlinTerrain = Mathf.PerlinNoise (perlinX / 10, perlinZ / 10) * 2; //perlin between 0.0 and 1.0
					perlinTerrain = Mathf.Pow (perlinTerrain, 3);

					int perlinY = (int) (Mathf.PerlinNoise (perlinX/3, perlinZ/3) * CHUNK_SIZE * perlinTerrain);

					string blockType = null;

					const float WATER_TYPE = 0.25f;

					if (perlinTerrainType < WATER_TYPE) {
						//int seaDescent = (int) (perlinTerrainType * 3.0f);
						perlinY *= 0;	

						blockType = "WaterBlock";
					} else if (perlinTerrainType < 0.4f) {
						float islandEdge = perlinTerrainType - WATER_TYPE;
						const float tweenDistance = 0.02f;
						const float tweenScale = 1.0f / tweenDistance;

						if (islandEdge < tweenDistance) {
							perlinY = (int) (perlinY * islandEdge * tweenScale);
						}

						blockType = "FastGrass";
					} else {
						blockType = "FastDirt";
					}

					perlinY = (perlinY + y) - (int)worldOffset.y;

					if (perlinY >= 0 && perlinY < CHUNK_SIZE) {
						CreateBlock(blockType, x, perlinY, z);
					}
				}
			}
		}

		//TREE_GEN
		for (int x = 0; x < CHUNK_SIZE; x++){
			for (int z = 0; z < CHUNK_SIZE; z++){
				//DESCEND
				for (int y = CHUNK_SIZE-1; y >= 0; y--){
					int worldY = (int)worldOffset.y + y;

					if (blocks [x, y, z] != null){
						if (Random.Range (0, 1000) < 1.0f){
							CreateBlockInOtherChunk ("LogBlock", x, y+1, z);
							CreateBlockInOtherChunk ("LogBlock", x, y+2, z);
							CreateBlockInOtherChunk ("LogBlock", x, y+3, z);
							CreateBlockInOtherChunk ("LeafBlock", x, y+4, z);
						}
						break;
					}
				}
			}
		}

		//WATER_GEN
		/*
		for (int x = 0; x < CHUNK_SIZE; x++)
		{
			for (int z = 0; z < CHUNK_SIZE; z++)
			{
				//DESCEND
				for (int y = CHUNK_SIZE-1; y >= 0; y--)
				{
					int worldY = (int)worldOffset.y + y;

					if (blocks [x, y, z] != null) {
						break;  //quit descending this y, as hit ground
					}

					if (worldY > 7 && worldY < 9) {
						CreateBlock ("WaterBlock", x, y, z);
					}
				}
			}
		}
		*/
		drawChunk();
	}

    public Block CreateBlock(string blockType, int chunkX, int chunkY, int chunkZ)
    {
        int worldX = (int)worldOffset.x + chunkX;
        int worldY = (int)worldOffset.y + chunkY;
        int worldZ = (int)worldOffset.z + chunkZ;

        Block tempBlock = new Block();
		tempBlock.resourceString = blockType;
        tempBlock.setPosition(worldX, worldY, worldZ);
        tempBlock.setChunkPosition(chunkX, chunkY, chunkZ);
        blocks[chunkX,chunkY,chunkZ] = tempBlock;
        return tempBlock;
    }

	public Block CreateBlockInOtherChunk(string blockType, int chunkX, int chunkY, int chunkZ) 
	{
		if (isInBlocksBounds (chunkX, chunkY, chunkZ)) {
			return CreateBlock (blockType, chunkX, chunkY, chunkZ);
		}

		Vector3 worldPosition = new Vector3 (worldOffset.x + chunkX, worldOffset.y + chunkY, worldOffset.z + chunkZ);
		Vector3 chunkPosition = HelperMethods.worldPositionToChunkPosition (worldPosition);
		Vector3 blockPosition = HelperMethods.vectorDifference (worldPosition, chunkPosition);

		Chunk chunk = generator.getChunk (chunkPosition);

		if (chunk == null) {
			//Debug.Log ("Cant create block, chunk doesnt exist "+chunkPosition);
			return null;
		}

		Block block = chunk.getBlock (blockPosition);

		if (block != null) {
			//Debug.Log ("Cant create block, block already there");
			return null;
		}

		return chunk.CreateBlock ("LeafBlock", (int)blockPosition.x, (int)blockPosition.y, (int)blockPosition.z);
	}

	public void GenMinerals(Dictionary<Mineral.Type, Vector3[]> minerals) 
	{
		foreach (KeyValuePair<Mineral.Type,Vector3[]> pair in minerals) {
			Vector3[] mineralPosition = pair.Value;

			for (int mineralIndex = 0; mineralIndex < mineralPosition.Length; mineralIndex++)
			{
				Block block = CreateBlock (Mineral.getBlock (Mineral.Type.Coal), (int)mineralPosition [mineralIndex].x, (int)mineralPosition [mineralIndex].y, (int)mineralPosition [mineralIndex].z);
				block.draw ();
			}
		}
	}

	public void Delete()
	{
		for (int x = 0; x < CHUNK_SIZE; x++) {
			for (int y = 0; y < CHUNK_SIZE; y++) {
				for (int z = 0; z < CHUNK_SIZE; z++) {

					if (blocks [x, y, z] != null) {
						blocks [x, y, z].Delete ();
						blocks [x, y, z] = null;
					}

				}
			}
		}

	}

	public Vector3 getOffset(){
		return this.worldOffset;
	}

	public Block getBlock(Vector3 position)
	{
		int x = (int)position.x;
		int y = (int)position.y;
		int z = (int)position.z;

		if (!isInBlocksBounds (x, y, z)) {
			return null;
		}
			        
		if (blocks [x, y, z] == null) {
			return null;
		}

		return blocks[x,y,z];
	}

    public void removeBlock(Vector3 position){
        blocks[(int)position.x, (int)position.y, (int)position.z] = null;
    }

	private bool nullOrWater(int x, int y, int z) {
		if (isInBlocksBounds (x, y, z) == false) {
			return false;
		}

		Block block = blocks [x, y, z];

		if (block == null) {
			return true;
		}

		if (block.resourceString == "WaterBlock") {
			return true;
		}

		return false;
	}

	private bool horizontalNull(int x, int y, int z) {
		bool leftNull = nullOrWater(x - 1, y, z);
		bool rightNull = nullOrWater(x + 1, y, z);
		bool backNull = nullOrWater(x, y, z - 1);
		bool frontNull = nullOrWater(x, y, z + 1);

		return leftNull || rightNull || backNull || frontNull;
	}
		
	/**drawChunk()
     * Renders the visible blocks based on the current state of the chunk.
     * Currently this means rendering any block that touches a null "air" Block.
     */
	private void drawChunk(){
		Block blockToDraw;

		for (int x = 0; x < CHUNK_SIZE; x++) {
			for (int z = 0; z < CHUNK_SIZE; z++) {
				bool inGround = false;

				for (int y = CHUNK_SIZE - 1; y >= 0; y--) {
					if (blocks [x, y, z] == null) {
						continue;
					}

					blockToDraw = blocks [x, y, z];

					if (inGround == false) {
						blockToDraw.draw ();
						inGround = true;
					} else if (horizontalNull (x, y, z)) {
						blockToDraw.draw ();
					} else {
						break;
					}
				}
			}
		}
	}


	public void fillHoles(bool checkNorth,bool checkSouth, bool checkEast,bool checkWest){
		Block blockToDraw;
		Vector3 chunkPos = new Vector3(0,0,0), blockPos = new Vector3(0,0,0);
		secondPass = true;

		for (int x = 0; x < CHUNK_SIZE; x++){
			for (int z = 0; z < CHUNK_SIZE; z++){
				//DESCEND
				for (int y = CHUNK_SIZE-1; y >= 0; y--){
					if (blocks [x, y, z] == null) {
						continue;
					}
					blockToDraw = blocks [x, y, z];
					if (checkNorth){
						if(!isInBlocksBounds (x, y, z + 1)){
							chunkPos.Set (worldOffset.x, worldOffset.y, worldOffset.z + CHUNK_SIZE);
							blockPos.Set (x, y, 0);
							if (generator.getChunk (chunkPos).getBlock (blockPos) == null) {
								blockToDraw.draw ();
							}
						} else if(nullOrWater (x, y, z + 1)){
							blockToDraw.draw ();
						}
					}

					if (checkSouth) {
						if(!isInBlocksBounds (x, y, z - 1)){
							chunkPos.Set (worldOffset.x, worldOffset.y, worldOffset.z - CHUNK_SIZE);
							blockPos.Set (x, y, CHUNK_SIZE-1);
							if (generator.getChunk (chunkPos).getBlock (blockPos) == null) {
								blockToDraw.draw ();
							}
						} else if(nullOrWater (x, y, z - 1)){
							blockToDraw.draw ();
						}
					}

					if (checkEast) {
						if(!isInBlocksBounds (x + 1, y, z)){
							chunkPos.Set (worldOffset.x + CHUNK_SIZE, worldOffset.y, worldOffset.z);
							blockPos.Set (0, y, z);
							if (generator.getChunk (chunkPos).getBlock (blockPos) == null) {
								blockToDraw.draw ();
							}
						} else if(nullOrWater (x + 1, y, z)){
							blockToDraw.draw ();
						}
					}

					if (checkWest) {
						if(!isInBlocksBounds (x - 1, y, z)){
							chunkPos.Set (worldOffset.x - CHUNK_SIZE, worldOffset.y, worldOffset.z);
							blockPos.Set (CHUNK_SIZE-1, y, z);
							if (generator.getChunk (chunkPos).getBlock (blockPos) == null) {
								blockToDraw.draw ();
							}
						} else if(nullOrWater (x - 1, y, z)){
							blockToDraw.draw ();
						}
					}
				}
			}
		}
	}

    bool isInBlocksBounds(int x, int y, int z){
        return x >= 0 && x < CHUNK_SIZE && y >= 0 && y < CHUNK_SIZE && z >= 0 && z < CHUNK_SIZE;
    }

    public bool waterProcess()
    {
        //WATER_GEN
        for (int x = 0; x < CHUNK_SIZE; x++)
        {
            for (int z = 0; z < CHUNK_SIZE; z++)
            {
                //DESCEND
                for (int y = CHUNK_SIZE - 1; y >= 0; y--)
                {
                    
                    if(blocks[x,y,z] != null && blocks[x,y,z].resourceString == "WaterBlock")
                    {
						bool or = false;

						or = or || waterProcessBlock (x + 1, y, z);
						or = or || waterProcessBlock (x - 1, y, z);
						or = or || waterProcessBlock (x, y, z + 1);
						or = or || waterProcessBlock (x, y, z - 1);
						or = or || waterProcessBlock (x, y - 1, z);

						if (or == true) {
							return true;
						}
					}

                }
            }
        }

		return false;
    }

	bool waterProcessBlock(int x2, int y2, int z2) {
		if (isInBlocksBounds (x2, y2, z2)) {
			if(blocks[x2,y2,z2] == null)
			{
				Block block = CreateBlock("WaterBlock", x2, y2, z2);
				block.draw();
				return true;
			}
		} else {
			int world_x = (int)worldOffset.x + x2;
			int world_y = (int)worldOffset.y + y2;
			int world_z = (int)worldOffset.z + z2;

			Vector3 worldPos = new Vector3 (world_x,world_y,world_z);

			Vector3 chunkPosition = HelperMethods.worldPositionToChunkPosition (worldPos);

			Chunk otherChunk = generator.getChunk (chunkPosition);

			if (otherChunk == null) {
				return false;
			}

			Vector3 chunkIndex = HelperMethods.vectorDifference (worldPos, chunkPosition);
			int chunkX = (int)chunkIndex.x;
			int chunkY = (int)chunkIndex.y;
			int chunkZ = (int)chunkIndex.z;

			if (isInBlocksBounds (chunkX, chunkY, chunkZ) == false) {
				return false;
			}

			Block otherBlock = otherChunk.blocks [chunkX, chunkY, chunkZ];

			if (otherBlock == null) {
				Block block = otherChunk.CreateBlock("WaterBlock", chunkX, chunkY, chunkZ);
				block.draw();
				return true;
			}

		}

		return false;
	}
}
