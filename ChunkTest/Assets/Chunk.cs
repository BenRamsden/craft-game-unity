using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
	public static int CHUNK_SIZE = 16;

	//blocks[,,] is a 3D array in the form [x, y, z]
	private Block[,,] blocks = new Block[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];
    private int highestPoint = 0;
	private Vector3 worldOffset;
	private ProceduralGenerator generator;

    public Block CreateBlock(string blockType, int chunkX, int chunkY, int chunkZ)
    {
        int worldX = (int)worldOffset.x + chunkX;
        int worldY = (int)worldOffset.y + chunkY;
        int worldZ = (int)worldOffset.z + chunkZ;

        Block tempBlock = new Block();
        tempBlock.BlockType = blockType;
        tempBlock.setPosition(worldX, worldY, worldZ);
        tempBlock.setChunkPosition(chunkX, chunkY, chunkZ);
        blocks[chunkX,chunkY,chunkZ] = tempBlock;
        return tempBlock;
    }

	public Chunk(Vector3 worldOffset, ProceduralGenerator generator, bool isBelowSurface = false){
		this.worldOffset = worldOffset;
		this.generator = generator;

        /* --- START SECTION --- */
        /*This section intialises the chunk*/

		if (isBelowSurface) {
			return;
		}

		for (int x = 0; x < CHUNK_SIZE; x++)
        {
			for (int y = 0; y < CHUNK_SIZE - (!isBelowSurface ? 4 : 0); y++)
            {
				for (int z = 0; z < CHUNK_SIZE; z++)
                {
                    //Calculate absolute position of block (world space)
					int worldX = (int)worldOffset.x + x;
					int worldZ = (int)worldOffset.z + z;

					//Generate a scaled X and Z for input into PerlinNoise function
					float perlinX = ((float)worldX) / CHUNK_SIZE;
					float perlinZ = ((float)worldZ) / CHUNK_SIZE;

					//Generate the PerlinNoise value, offset the block's height by this
					int perlinY = (int) (y + Mathf.PerlinNoise (perlinX, perlinZ) * 5);
					
					if (perlinY < 0 || perlinY > CHUNK_SIZE-1) {
						Debug.Log ("Cannot insert chunk into block at index " + perlinY + " continuing");
						continue;
					}

                    string blockType = null;

					if (y <= 4)
					{
                        blockType = "StoneBlock";
					}
					else if (y <= 10)
					{
                        blockType = "SoilBlock";
					}
					else
					{
                        blockType = "GrassBlock";
					}

                    highestPoint = (perlinY > highestPoint) ? perlinY : highestPoint;

                    CreateBlock(blockType, x, perlinY, z);
                }
			}
		}

		//WATER_GEN
		for (int x = 0; x < CHUNK_SIZE; x++)
		{
			for (int z = 0; z < CHUNK_SIZE; z++)
			{
				//DESCEND
				for (int y = CHUNK_SIZE-1; y >= 0; y--)
				{

					if (blocks [x, y, z] != null) {
						break;  //quit descending this y, as hit ground
					}

					if (y < 14 && Random.Range(0,1000) < 1.0f) {
                        CreateBlock("WaterBlock", x, y, z);
					}
				}
			}
		}
			
	    /* --- END SECTION --- */
		drawChunk();
	}

	public void GenMinerals(Dictionary<Mineral.Type, Vector3[]> minerals) {
		foreach (KeyValuePair<Mineral.Type,Vector3[]> pair in minerals) {
			Vector3[] mineralPosition = pair.Value;

			for (int mineralIndex = 0; mineralIndex < mineralPosition.Length; mineralIndex++)
			{
				Block block = CreateBlock (Mineral.getBlock (Mineral.Type.Coal), (int)mineralPosition [mineralIndex].x, (int)mineralPosition [mineralIndex].y, (int)mineralPosition [mineralIndex].z);
				block.draw ();
			}
		}
	}

	public void Delete() {
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

	public Vector3 getOffset() {
		return this.worldOffset;
	}

	public Block getBlock(Vector3 position){
		return blocks [(int)position.x, (int)position.y, (int)position.z];
	}

    /**drawChunk()
     * Renders the visible blocks based on the current state of the chunk.
     * Currently this means rendering any block that touches a null "air" Block.
     */
	private bool nullOrWater(Block block) {
		return block == null || block.BlockType == "WaterBlock";
	}

	private bool surroundedByNull(int x, int y, int z) {
		return nullOrWater (blocks [x + 1, y, z]) ||
		nullOrWater (blocks [x - 1, y, z]) ||
		nullOrWater (blocks [x, y + 1, z]) ||
		nullOrWater (blocks [x, y - 1, z]) ||
		nullOrWater (blocks [x, y, z + 1]) ||
		nullOrWater (blocks [x, y, z - 1]);
	}

	private void drawChunk()
	{
		Block blockToDraw;

		//Iterate through each layer from the highest point downwards (y-axis)
		for (int y = highestPoint; y >= 0; y--)
		{
			//Iterate through each row (x-axis)
			for (int x = 0; x < CHUNK_SIZE; x++)
			{
				//Iterate through each column (z-axis)
				for (int z = 0; z < CHUNK_SIZE; z++)
				{
					if (blocks [x, y, z] != null)
					{
						blockToDraw = blocks [x, y, z];

                        //If statement to handle Blocks at the edge of a chunk
                        if (x == 0 || x == CHUNK_SIZE-1 ||
                            y == 0 || y == CHUNK_SIZE-1 ||
                            z == 0 || z == CHUNK_SIZE-1)
                        {
//							if (blockToDraw.BlockType == "StoneBlock")
//								continue;
                            blockToDraw.draw();
                        }
                        //If statement to handle Blocks within a chunk
						else if(surroundedByNull(x,y,z))
                        {
//							if (blockToDraw.BlockType == "StoneBlock")
//								continue;
                            blockToDraw.draw();
                        }
					}
				}
			}
		}
	}

    bool isInBlocksBounds(int x, int y, int z)
    {
        return x >= 0 && x < CHUNK_SIZE && y >= 0 && y < CHUNK_SIZE && z >= 0 && z < CHUNK_SIZE;
    }

    public void waterProcess()
    {
        //WATER_GEN
        for (int x = 0; x < CHUNK_SIZE; x++)
        {
            for (int z = 0; z < CHUNK_SIZE; z++)
            {
                //DESCEND
                for (int y = CHUNK_SIZE - 1; y >= 0; y--)
                {
                    
                    if(blocks[x,y,z] != null && blocks[x,y,z].BlockType == "WaterBlock")
                    {
                        for(int x2 = x-1; x2 <= x+1; x2++)
                        {
                            for(int z2 = z-1; z2 <= z+1; z2++)
                            {
                                for(int y2 = y-1; y2 <= y; y2++)
                                {
									int world_x = (int)worldOffset.x + x2;
									int world_y = (int)worldOffset.y + y2;
									int world_z = (int)worldOffset.z + z2;

									if (isInBlocksBounds (x2, y2, z2)) {
										if(blocks[x2,y2,z2] == null)
										{
                                            Block block = CreateBlock("WaterBlock", x2, y2, z2);
                                            block.draw();
										}
									} else {
										Vector3 worldPos = new Vector3 (world_x,world_y,world_z);

										Vector3 chunkPosition = HelperMethods.worldPositionToChunkPosition (worldPos);
		
										Chunk otherChunk = generator.getChunk (chunkPosition);

										if (otherChunk == null) {
											continue;
										}

										Vector3 chunkIndex = HelperMethods.vectorDifference (worldPos, chunkPosition);
										int chunkX = (int)chunkIndex.x;
										int chunkY = (int)chunkIndex.y;
										int chunkZ = (int)chunkIndex.z;

										if (isInBlocksBounds (chunkX, chunkY, chunkZ) == false) {
											continue;
										}

										Block otherBlock = otherChunk.blocks [chunkX, chunkY, chunkZ];

										if (otherBlock == null) {
                                            Block block = otherChunk.CreateBlock("WaterBlock", chunkX, chunkY, chunkZ);
                                            block.draw();
										}
										
									}

                                    
                                }
                            }
                        }
                    }

                }
            }
        }
    }

    // Use this for initialization
    void Start()
	{	
	}

	// Update is called once per frame
	void Update()
	{
	}
}
