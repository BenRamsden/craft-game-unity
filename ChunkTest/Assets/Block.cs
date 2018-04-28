using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Item {
    private BlockProperties bp;
    Vector3 chunkPosition;

	/// <summary>
	/// Damages the block.
	/// </summary>
	/// <param name="inputDamage">Input damage.</param>
    public void damageBlock(int inputDamage) {
        bp.blockHealth -= inputDamage;
        //Debug.Log(bp.blockHealth);
    }

	/// <summary>
	/// Drops a collectable version of this object the world.
	/// </summary>
    public void dropSelf() {
        Transform thisTransform = thisBody.transform;
        thisTransform.localScale -= new Vector3(0.75f, 0.75f, 0.75f);
        thisTransform.rotation = Random.rotation;
        thisBody.AddComponent<Rigidbody>();
        thisBody.GetComponent<Rigidbody>().AddForce(thisTransform.forward * 1.0f);
    }

    /// <summary>
    /// Draw this instance.
    /// </summary>
    public void draw() {
        base.draw();
        if (thisBody != null) {
            bp = thisBody.GetComponent<BlockProperties>();
        }
    }

	/// <summary>
	/// Sets the Block's knowledge of its position in the world.
	/// </summary>
	/// <param name="row">Row.</param>
	/// <param name="layer">Layer.</param>
	/// <param name="column">Column.</param>
    public void setPosition(int row, int layer, int column) {
        worldPosition = new Vector3(row, layer, column);
    }

	/// <summary>
	/// Gets the position.
	/// </summary>
	/// <returns>The position.</returns>
    public Vector3 getPosition() {
        return worldPosition;
    }

	/// <summary>
	/// Sets the Block's knowledge of its position in its Chunk.
	/// </summary>
	/// <param name="posX">Position x.</param>
	/// <param name="posY">Position y.</param>
	/// <param name="posZ">Position z.</param>
    public void setChunkPosition(int posX, int posY, int posZ) {
        chunkPosition = new Vector3(posX, posY, posZ);
    }

	/// <summary>
	/// Gets the properties.
	/// </summary>
	/// <returns>The properties.</returns>
    public BlockProperties getProperties() {
        return bp;
    }

	/// <summary>
	/// Gets the x coordinate of where the Block is in its Chunk.
	/// </summary>
	/// <returns>The chunk x.</returns>
    public float getChunkX() {
        return chunkPosition.x;
    }

	/// <summary>
	/// Gets the z coordinate of where the Block is in its Chunk.
	/// </summary>
	/// <returns>The chunk x.</returns>
    public float getChunkZ() {
        return chunkPosition.z;
    }

	/// <summary>
	/// Delete this instance.
	/// </summary>
	public void Delete(){
		base.Delete();
	}
}
