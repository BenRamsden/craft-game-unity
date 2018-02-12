using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
	const int CHUNK_WIDTH = 5;
	Chunk[,] currentFOV = new Chunk[CHUNK_WIDTH,CHUNK_WIDTH];

	// Use this for initialization
	void Start () {
		for (int x = 0; x < CHUNK_WIDTH; x++) {
			for (int z = 0; z < CHUNK_WIDTH; z++) {
				currentFOV [x,z] = new Chunk(16*x,16*z);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
