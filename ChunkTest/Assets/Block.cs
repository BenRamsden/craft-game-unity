using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
    GameObject thisBody;

	//static int deletePrints = 0; 	//what is this? 
	BlockProperties bp;
	Vector3 worldPosition;
	Vector3 chunkPosition;

	public string BlockType { get; set; }

	public void damageBlock(int inputDamage) {
		if (false) {
		}
	}

	public void Delete() {
		if (thisBody != null) {
			GameObject.Destroy (thisBody);
			thisBody = null;
		}
	}
		
    /**draw() 
     * Draws the physical representation of a Block in the world.
     */
    public void draw()
    {
		thisBody = GameObject.Instantiate(Resources.Load(BlockType), worldPosition , Quaternion.identity) as GameObject;
		bp = thisBody.GetComponent<BlockProperties> ();

    }

	/**setPosition(int,int,int)
     * Sets the Block's knowledge of its position in the world.
     * @param row - the x coordinate of where the Block is in the world.
     * @param layer - the y coordinate of where the Block is in the world.
     * @param column - the z coordinate of where the Block isin the world.
     */
	public void setPosition(int row, int layer, int column)
	{
		worldPosition = new Vector3 (row, layer, column);
	}

    /**setChunkPosition(int,int,int)
     * Sets the Block's knowledge of its position in its Chunk.
     * @param row - the x coordinate of where the Block is in its Chunk.
     * @param layer - the y coordinate of where the Block is in its Chunk.
     * @param column - the z coordinate of where the Block is in its Chunk.
     */
    public void setChunkPosition(int posX, int posY, int posZ) {
		chunkPosition = new Vector3 (posX, posY, posZ);
    }

    /**getChunkX()
     * @return chunkX - the x coordinate of where the Block is in its Chunk.
     */
    public int getChunkX()
    {
		return chunkPosition.x;
    }

    /**getChunkY()
     * @return chunkZ - the z coordinate of where the Block is in its Chunk.
     */
    public int getChunkZ()
    {
		return chunkPosition.z;
    }
}
