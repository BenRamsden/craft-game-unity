using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recipe{
	[System.Serializable]
	public class Recipe{
		public string name;
		public int amount;
		public string[] recipe;
	}

	[System.Serializable]
	public class Recipes{
		public Recipe[] recipes;
	}
}
