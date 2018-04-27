using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Recipe;

public class CraftingHandler {
	public Recipe.Recipe[] craftingRecipes;
	int numberOfRecipes;

	/// <summary>
	/// Initialise the crafting recipes.
	/// </summary>
	public bool initialise() {
		var recipes = Resources.Load("Crafting/recipes") as TextAsset;
		if (recipes == null)
			return false;

		Recipe.Recipes tmp = JsonUtility.FromJson<Recipe.Recipes>(recipes.text);
		craftingRecipes = tmp.recipes;

		numberOfRecipes = craftingRecipes.Length;
		return true;
	}

	/// <summary>
	/// Crafts the item from the recipe input into the crafting UI.
	/// </summary>
	/// <returns>The item.</returns>
	/// <param name="matrix">Container reference.</param>
	public Item[] craftItem(Container matrix) {
		Block[] blocks = null;
		bool[] possible = new bool[numberOfRecipes];
		for(int k = 0; k < numberOfRecipes; k++){
			possible [k] = false;
		}

		Recipe.Recipe currentRecipe;
		for (int i = 0; i < numberOfRecipes; i++) {
			//This Recipe
			currentRecipe = craftingRecipes[i];
			for(int j = 0; j < 9; j++){
				if (matrix.getItem (j) == null && currentRecipe.recipe[j] != "DefaultItem") {
					possible [i] = false;
					break;
				}

				if(matrix.getItem(j) != null){
					if (currentRecipe.recipe[j] == matrix.getItem (j).resourceString) {
						possible [i] = true;
					} else {
						possible [i] = false;
						break;
					}
				}
			}
		}

		for(int l = 0; l < numberOfRecipes; l++){
			if (possible [l]) {
				blocks = new Block[craftingRecipes[l].amount];
				for(int m = 0; m < blocks.Length; m++){
					blocks[m] = new Block();
					blocks[m].resourceString = craftingRecipes[l].name;
				}
				//Consume resources
				for (int n = 0; n < 9; n++) {
					if (craftingRecipes [l].recipe [n] != "DefaultItem") {
						matrix.removeItem(n);
					}
				}
				break;
			}
		}
		return blocks;
	}
}
