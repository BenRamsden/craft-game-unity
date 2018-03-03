using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
	private static int CHUNK_SIZE = Chunk.CHUNK_SIZE;

	private GameObject player;

	private ProceduralGenerator generator;

	// Use this for initialization
	void Start ()
    {
		generator = new ProceduralGenerator ();

		Vector3 origin = generator.initalise (CHUNK_SIZE, 0);

		player = (GameObject)Instantiate (Resources.Load("Steve/PlayerTorso"), new Vector3 (origin.x + 10, origin.y + 18, origin.z + 10), Quaternion.identity);
	}
		
	Vector3 getPlayerPosition() {
		Vector3 playerPos = player.transform.position;

		playerPos.y -= CHUNK_SIZE / 2;

		return HelperMethods.worldPositionToChunkPosition (playerPos);
	}

	public ProceduralGenerator getPGenerator(){
		return generator;
	}
	
	// Update is called once per frame
	void Update ()
    {
		Vector3 playerPos = getPlayerPosition ();

		generator.generateMap(playerPos);

		generator.garbageCollect(playerPos);

        generator.waterProcess(playerPos);
	}
}
