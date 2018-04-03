using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CraftingHandler{

	public static Item craftItem(Container matrix){
		Block block = new Block();
		Debug.Log ("1");
		if (matrix.getItem (0) != null){
			Debug.Log ("2");
			Debug.Log (matrix.getItem (0).resourceString);
			if(matrix.getItem(0).resourceString == "LogBlock"){
				Debug.Log ("3");
				block.resourceString = "WoodBlock";
				return block;
			}
		}
		return null;
	}
}
