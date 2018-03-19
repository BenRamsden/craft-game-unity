using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Item{
	private BlockProperties bp;
	Vector3 chunkPosition;

	public void damageBlock(int inputDamage) {
		bp.blockHealth -= inputDamage;
		Debug.Log (bp.blockHealth);
	}

    public void dropSelf(){
        Transform thisTransform = thisBody.transform;
        thisTransform.localScale -= new Vector3(0.75f, 0.75f, 0.75f);
        thisTransform.rotation = Random.rotation;
        thisBody.AddComponent<Rigidbody>();
        thisBody.GetComponent<Rigidbody>().AddForce(thisTransform.forward * 1.0f);
    }
		
    /**draw() 
     * Draws the physical representation of a Block in the world.
     */
    public void draw()
    {
		base.draw();
		if (thisBody != null) {
			bp = thisBody.GetComponent<BlockProperties> ();
		}
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

	public Vector3 getPosition() {
		return worldPosition;
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

    public BlockProperties getProperties() {
        return bp;
    }

    /**getChunkX()
     * @return chunkX - the x coordinate of where the Block is in its Chunk.
     */
    public float getChunkX()
    {
		return chunkPosition.x;
    }

    /**getChunkY()
     * @return chunkZ - the z coordinate of where the Block is in its Chunk.
     */
    public float getChunkZ()
    {
		return chunkPosition.z;
    }
}
