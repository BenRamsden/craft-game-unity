using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockProperties : MonoBehaviour {

	int blockHealth = 100;
	bool isMineable, isPickupable, isBreakable, isTraversable;
	int chunkX, chunkY, chunkZ;





	public void setChunkPosition(int posX, int posY, int posZ)
	{
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

}
