using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CraftingHandler{

	public static Item craftItem(Container matrix){
		Block block = new Block();
		if (matrix.getItem (0) != null){
			if(matrix.getItem(0).resourceString == "LogBlock"){
				block.resourceString = "WoodBlock";
				return block;
			}
		}
		return null;
	}
}
