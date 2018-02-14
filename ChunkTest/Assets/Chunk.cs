using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
	private const int CHUNK_SIZE = 16;

	//blocks[,,] is a 3D array in the form [x, y, z]
	private Block[,,] blocks = new Block[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];
    private int highestPoint = 0;
	private int offsetX, offsetZ;

	public Chunk(int offsetX, int offsetZ){
		this.offsetX = offsetX;
		this.offsetZ = offsetZ;

        /* --- START SECTION --- */
        /*This section intialises the chunk*/

        Block tempBlock;
        int world_x, world_y, world_z;
        float perlinX, perlinY, perlinZ;

		for (int x = 0; x < CHUNK_SIZE; x++)
        {
			for (int y = 0; y < CHUNK_SIZE; y++)
            {
				for (int z = 0; z < CHUNK_SIZE; z++)
                {
                    //Calculate absolute position of block (world space)
                    world_x = offsetX + x;
                    world_z = offsetZ + z;

					//Generate a scaled X and Z for input into PerlinNoise function
					perlinX = ((float)world_x) / CHUNK_SIZE;
					perlinZ = ((float)world_z) / CHUNK_SIZE;

					//Generate the PerlinNoise value, offset the block's height by this
					perlinY = Mathf.PerlinNoise (perlinX, perlinZ);
                    world_y = y + (int)(perlinY * 5);

					//Debug.Log ("world_x:" + world_x + " world_z:" + world_z + " = " + world_y);
					if (world_y < 0 || world_y > CHUNK_SIZE-1)
                    {
						Debug.Log ("Cannot insert chunk into block at index " + world_y + " continuing");
						continue;
					}

					if (y <= 0)
                    {
						tempBlock = new Block();
						tempBlock.setBlockType("StoneBlock");
					}
                    else if (y <= 1)
                    {
						tempBlock = new Block();
						tempBlock.setBlockType("GrassBlock");
					}
                    else
                    {
						continue;
					}

                    highestPoint = (world_y > highestPoint) ? world_y : highestPoint;

					tempBlock.setPosition(world_x, world_y, world_z);
                    tempBlock.setChunkPosition(offsetX, 0, offsetZ);
					blocks [x, world_y, z] = tempBlock;

                    //Test to fill in all layers below grass with stone (can be deleted when necessary)
                    if (y == 0)
                    {
                        for (int i = 0; i < world_y; i++)
                        {
                            tempBlock = new Block();
                            tempBlock.setBlockType("StoneBlock");
                            tempBlock.setPosition(world_x, i, world_z);
                            tempBlock.setChunkPosition(offsetX, 0, offsetZ);
                            blocks[x, i, z] = tempBlock;
                        }
                    }
                }
			}
		}
	    /* --- END SECTION --- */
		drawChunk();
	}

    /**drawChunk()
     * Renders the visible blocks based on the current state of the chunk.
     * Currently this means rendering any block that touches a null "air" Block.
     */
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
                            blockToDraw.draw();
                        }
                        //If statement to handle Blocks within a chunk
                        else if(blocks[x + 1 , y, z] == null || blocks[x - 1, y, z] == null ||
                                blocks[x, y + 1, z] == null  || blocks[x, y - 1, z] == null ||
                                blocks[x, y, z + 1] == null  || blocks[x, y, z - 1] == null)
                        {
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
