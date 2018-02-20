using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
	private const int CHUNK_SIZE = 16;

	//blocks[,,] is a 3D array in the form [x, y, z]
	private Block[,,] blocks = new Block[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];
    private int highestPoint = 0;
	private Vector3 offset;

	public Chunk(Vector3 offset, bool isBelowSurface = false, Dictionary<Mineral.Type, Vector3[]> minerals = null){
		this.offset = offset;

        /* --- START SECTION --- */
        /*This section intialises the chunk*/

        Block tempBlock;
        int world_x, world_y, world_z;
        float perlinX, perlinY, perlinZ;

		int world_yNoOffset;

		for (int x = 0; x < CHUNK_SIZE; x++)
        {
			for (int y = 0; y < CHUNK_SIZE - (!isBelowSurface ? 4 : 0); y++)
            {
				for (int z = 0; z < CHUNK_SIZE; z++)
                {
                    //Calculate absolute position of block (world space)
					world_x = (int)offset.x + x;
					world_z = (int)offset.z + z;

					//Generate a scaled X and Z for input into PerlinNoise function
					perlinX = ((float)world_x) / CHUNK_SIZE;
					perlinZ = ((float)world_z) / CHUNK_SIZE;

					//Generate the PerlinNoise value, offset the block's height by this
					perlinY = Mathf.PerlinNoise (perlinX, perlinZ);

					if (isBelowSurface)
						perlinY = 0;

					world_y = y + (int)(perlinY * 5);
					world_yNoOffset = (int)offset.y + world_y;
						
					//Debug.Log ("world_x:" + world_x + " world_z:" + world_z + " = " + world_y);
					if (world_y < 0 || world_y > CHUNK_SIZE-1)
                    {
						Debug.Log ("Cannot insert chunk into block at index " + world_y + " continuing");
						continue;
					}

					tempBlock = new Block ();

					if (isBelowSurface)
					{
						tempBlock.BlockType ="StoneBlock";
					}
					else
					{
						if (y <= 4)
						{
							tempBlock.BlockType = "StoneBlock";
						}
						else if (y <= 10)
						{
							tempBlock.BlockType = "SoilBlock";
						}
						else
						{
							tempBlock.BlockType = "GrassBlock";
						}
					}

                    highestPoint = (world_y > highestPoint) ? world_y : highestPoint;

					tempBlock.setPosition(world_x, world_yNoOffset, world_z);
					tempBlock.setChunkPosition((int)offset.x, (int)offset.y, (int)offset.z);
					blocks [x, world_y, z] = tempBlock;

					if (y <= 4 && !isBelowSurface)
					{
						for (int i = (int)offset.y; i < world_yNoOffset; i++)
						{
							tempBlock = new Block();
							tempBlock.BlockType = "StoneBlock";
							tempBlock.setPosition(world_x, i, world_z);
							tempBlock.setChunkPosition((int)offset.x, 0, (int)offset.z);
							blocks[x, i - (int)offset.y, z] = tempBlock;
						}
					}
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

					if (y < 14) {
						world_x = (int)offset.x + x;
						world_y = (int)offset.y + y;
						world_z = (int)offset.z + z;

						tempBlock = new Block ();
						tempBlock.BlockType = "WaterBlock";
						tempBlock.setPosition (world_x, world_y, world_z);
						tempBlock.setChunkPosition (x, y, z);
						blocks [x, y, z] = tempBlock;
					}
				}
			}
		}

		if (minerals != null)
		{
			Vector3[] mineralPosition;
			if(minerals.ContainsKey(Mineral.Type.Coal))
			{
				mineralPosition= minerals [Mineral.Type.Coal];

				for (int mineralIndex = 0; mineralIndex < mineralPosition.Length; mineralIndex++)
				{
					blocks [(int)mineralPosition [mineralIndex].x, (int)mineralPosition [mineralIndex].y, (int)mineralPosition [mineralIndex].z].BlockType = Mineral.getBlock(Mineral.Type.Coal);
				}
			}

			if (minerals.ContainsKey (Mineral.Type.Iron))
			{
				mineralPosition = minerals [Mineral.Type.Iron];

				for (int mineralIndex = 0; mineralIndex < mineralPosition.Length; mineralIndex++)
				{
					blocks [(int)mineralPosition [mineralIndex].x, (int)mineralPosition [mineralIndex].y, (int)mineralPosition [mineralIndex].z].BlockType = Mineral.getBlock (Mineral.Type.Iron);
				}
			}
		}
		
	    /* --- END SECTION --- */
		drawChunk();
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
		return this.offset;
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

	// Use this for initialization
	void Start()
	{	
	}

	// Update is called once per frame
	void Update()
	{
	}
}
