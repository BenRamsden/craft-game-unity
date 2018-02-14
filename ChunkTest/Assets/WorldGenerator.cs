using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
	const int WORLD_SIZE = 5;
	Chunk[,] currentFOV = new Chunk[WORLD_SIZE, WORLD_SIZE];

	// Use this for initialization
	void Start ()
    {
        //Draw the world
		for (int x = 0; x < WORLD_SIZE; x++)
        {
			for (int z = 0; z < WORLD_SIZE; z++)
            {
				currentFOV [x,z] = new Chunk(16*x,16*z);
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
    {
	}
}
