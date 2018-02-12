using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block{
	GameObject thisBody;
	Texture textureProperty;
	bool isMineable, isPickupable, isBreakable, isTraversable;
	int blockHealth, blockDamage;

	public Block(){
		thisBody = GameObject.CreatePrimitive (PrimitiveType.Cube);
	}

	public void setPosition(int row, int layer, int column){
		thisBody.transform.position = new Vector3 (row, layer, column);
	}

	public void renderColor(Color paint){
		thisBody.GetComponent<Renderer> ().material.color = paint;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
