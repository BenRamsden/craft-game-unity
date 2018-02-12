using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk{
	private const int CHUNK_SIZE = 16;
	//blocks is a 3D array in the form [x, y, z], as such each layer is
	//up the y axis like floors of a building. and layerIsAir therefore
	//is a direct reference such that: blocks[-,x,-] = layersIsAir[x]
	private Block[,,] blocks = new Block[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];
	private bool[] layerIsAir = new bool[CHUNK_SIZE];
	private int offsetX, offsetZ;

	public Chunk(int offsetX, int offsetZ){
		this.offsetX = offsetX;
		this.offsetZ = offsetZ;
	/*START SECTION*/
		/*This section intialises the chunk, in future this will
		 *be done by the procedural generation algorithm.*/
		for(int i = 0; i < CHUNK_SIZE; i++)
		{
			layerIsAir[i] = (i > 2) ? true : false;
		}
		Block tempBlock;
		for(int j = 0; j < CHUNK_SIZE; j++)
		{
			for(int k = 0; k < CHUNK_SIZE; k++)
			{
				tempBlock = new Block();
				tempBlock.renderColor (Color.gray);
				blocks [k, 0, j] = tempBlock;
				tempBlock = new Block();
				tempBlock.renderColor (Color.green);
				blocks [k, 1, j] = tempBlock;
			}
		}
	/*END SECTION*/
		drawChunk();
	}

	//Renders the visible blocks based on the current state of the chunk.
	//Currently this means rendering any block that touches a null "air"
	//block.
	private void drawChunk()
	{
		Block blockToDraw;
		//Iterate through each layer (y-axis)
		for (int l = CHUNK_SIZE-1; l >= 0; l--)
		{
			//If the current layer is completely air, continue to the next
			//layer down, there is no point iterating through that layer as
			//there is nothing to draw.
			if(layerIsAir[l]) continue;
			//Iterate through each row (x-axis)
			for (int r = 0; r < CHUNK_SIZE; r++)
			{
				//Iterate through each column (z-axis)
				for (int c = 0; c < CHUNK_SIZE; c++)
				{
					if (blocks [r, l, c] != null)
					{
						blockToDraw = blocks [r, l, c];
						blockToDraw.setPosition(offsetX+r,l,offsetZ+c);
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
