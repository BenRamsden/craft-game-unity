using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
	private const int CHUNK_SIZE = 16;

	private GameObject player;

	private ProceduralGenerator generator;

	// Use this for initialization
	void Start ()
    {
		generator = new ProceduralGenerator ();

		Vector3 origin = generator.initalise (CHUNK_SIZE, 0);

		player = (GameObject)Instantiate (Resources.Load("PlayerTorso"), new Vector3 (origin.x + 10, origin.y + 18, origin.z + 10), Quaternion.identity);

		generator.generateMap (origin);
	}
	
	// Update is called once per frame
	void Update ()
    {
		Vector3 playerPos = player.transform.position;

		float originX = playerPos.x - playerPos.x % CHUNK_SIZE;
		float originY = playerPos.y - playerPos.y % CHUNK_SIZE;
		float originZ = playerPos.z - playerPos.z % CHUNK_SIZE;


	}
}
