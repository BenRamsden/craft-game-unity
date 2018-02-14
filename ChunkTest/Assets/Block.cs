using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block{
    GameObject thisBody;
	bool isMineable, isPickupable, isBreakable, isTraversable;
	int blockHealth, blockDamage;
    int worldX, worldY, worldZ;
    int chunkX, chunkY, chunkZ;
    string blockType;

    public void draw()
    {
        thisBody = GameObject.Instantiate(Resources.Load(blockType), new Vector3(worldX, worldY, worldZ), Quaternion.identity) as GameObject;
    }

	public void setPosition(int row, int layer, int column)
    {
        worldX = row;
        worldY = layer;
        worldZ = column;
	}

    public void setChunkPosition(int posX, int posY, int posZ){
        chunkX = posX;
        chunkY = posY;
        chunkZ = posZ;
    }

    public int getChunkX()
    {
        return chunkX;
    }
    public int getChunkZ()
    {
        return chunkZ;
    }

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
