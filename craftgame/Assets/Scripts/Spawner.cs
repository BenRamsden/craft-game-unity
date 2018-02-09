using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		for (int x = 0; x < 8; x++)
		{
			for (int y = 0; y < 8; y++)
			{
				for (int z = 0; z < 8; z++)
				{
					Color c;
					if (y <= 3)
						c = new Color (0, 0, 0);
					else if (y <= 6)
						c = new Color (0.65f, 0.15f, 0.15f);
					else
						c = Color.green;

					GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
					cube.transform.position = new Vector3 (x + 10, y + 10, z + 10);
					cube.GetComponent<Renderer> ().material.color = c;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
