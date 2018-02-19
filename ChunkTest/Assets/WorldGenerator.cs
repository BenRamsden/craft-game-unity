using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
		ProceduralGenerator generator = new ProceduralGenerator ();
		Vector3 origin = generator.initalise (16, 0);

		GameObject player = (GameObject)Instantiate (Resources.Load("Torso"), new Vector3 (origin.x + 10, origin.y + 18, origin.z + 10), Quaternion.identity);

		generator.generateMap (origin);
	}
	
	// Update is called once per frame
	void Update ()
    {
	}
}
