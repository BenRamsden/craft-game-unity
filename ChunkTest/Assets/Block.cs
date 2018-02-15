using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    GameObject thisBody;
	bool isMineable, isPickupable, isBreakable, isTraversable;
	int blockHealth, blockDamage;
    int worldX, worldY, worldZ;
    int chunkX, chunkY, chunkZ;
    string blockType;

    /**draw() 
     * Draws the physical representation of a Block in the world.
     */
    public void draw()
    {
        thisBody = GameObject.Instantiate(Resources.Load(blockType), new Vector3(worldX, worldY, worldZ), Quaternion.identity) as GameObject;
    }

    /**setPosition(int,int,int)
     * Sets the Block's knowledge of its position in the world.
     * @param row - the x coordinate of where the Block is in the world.
     * @param layer - the y coordinate of where the Block is in the world.
     * @param column - the z coordinate of where the Block isin the world.
     */
    public void setPosition(int row, int layer, int column)
    {
        worldX = row;
        worldY = layer;
        worldZ = column;
	}

    /**setChunkPosition(int,int,int)
     * Sets the Block's knowledge of its position in its Chunk.
     * @param row - the x coordinate of where the Block is in its Chunk.
     * @param layer - the y coordinate of where the Block is in its Chunk.
     * @param column - the z coordinate of where the Block is in its Chunk.
     */
    public void setChunkPosition(int posX, int posY, int posZ)
    {
        chunkX = posX;
        chunkY = posY;
        chunkZ = posZ;
    }

    /**getChunkX()
     * @return chunkX - the x coordinate of where the Block is in its Chunk.
     */
    public int getChunkX()
    {
        return chunkX;
    }

    /**getChunkY()
     * @return chunkZ - the z coordinate of where the Block is in its Chunk.
     */
    public int getChunkZ()
    {
        return chunkZ;
    }

    /**setBlockType(string)
     * Sets the type of Block.
     * @param blockType - the type of prefab this Block should draw.
     */
    public void setBlockType(string blockType)
    {
        this.blockType = blockType;
	}

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	}
}
