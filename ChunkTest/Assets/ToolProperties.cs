using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolProperties : MonoBehaviour {
    string toolType;
    int toolQualityRating;
    int toolHealth = 100;

	/// <summary>
	/// Initialise the specified toolType and toolQualityRating.
	/// </summary>
	/// <param name="toolType">Tool type.</param>
	/// <param name="toolQualityRating">Tool quality rating.</param>
    public void initialise(string toolType, int toolQualityRating) {
        this.toolType = toolType;
        this.toolQualityRating = toolQualityRating;
    }

    /// <summary>
    /// Deals wear damage the tool.
    /// </summary>
    /// <param name="damage">Damage.</param>
    public void wearTool(int damage){
        toolHealth -= damage;
    }

    /// <summary>
    /// Gets the quality rating.
    /// </summary>
    /// <returns>The quality rating.</returns>
    public int getQualityRating() {
        return toolQualityRating;
    }

	/// <summary>
	/// Gets the type of the tool.
	/// </summary>
	/// <returns>The tool type.</returns>
    public string getToolType(){
        return toolType;
    }

	/// <summary>
	/// Gets the current tool health.
	/// </summary>
	/// <returns>The health.</returns>
    public int getHealth() {
        return toolHealth;
    }
}
