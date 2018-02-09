using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	const int CHUNK_WIDTH = 16;
	Color bedrock_color = new Color (0, 0, 0);
	Color mud_color = new Color (0.65f, 0.15f, 0.15f);
	Color grass_color = Color.green;

	// Use this for initialization
	void Start () {
		for (int x = 0; x < CHUNK_WIDTH; x++)
		{
			for (int y = 0; y < CHUNK_WIDTH; y++)
			{
				for (int z = 0; z < CHUNK_WIDTH; z++)
				{
					Color c;
					if (y <= 3)
						c = bedrock_color;
					else if (y <= 6)
						c = mud_color;
					else
						c = grass_color;

					GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
					cube.transform.position = new Vector3 (x, y, z);
					cube.GetComponent<Renderer> ().material.color = c;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
