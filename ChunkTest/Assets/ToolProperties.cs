using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolProperties : MonoBehaviour {
    string toolType;
    int toolQualityRating;
    int toolHealth = 100;

    /**initialise(string, int)
     * This function should be called directly after instantiating a tool in the world to set 
     * what type of tool it is. We can then use the toolQualityRating and toolHealth properties
     * to calculate how much damage a tool deals to a block/NPC/object.
     * 
     * @param toolType - the type of tool "pickaxe, sword, bucket, etc."
     * @param toolQualityRating - the material the tool is made of "iron, gold, diamond, etc."
     */
    public void initialise(string toolType, int toolQualityRating) {
        this.toolType = toolType;
        this.toolQualityRating = toolQualityRating;
    }

    //Setters
    public void wearTool(int damage){
        toolHealth -= damage;
    }

    //Getters
    public int getQualityRating() {
        return toolQualityRating;
    }
    public string getToolType(){
        return toolType;
    }
    public int getHealth() {
        return toolHealth;
    }
}
