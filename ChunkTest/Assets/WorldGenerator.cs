using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
	private static int CHUNK_SIZE = Chunk.CHUNK_SIZE;

	private GameObject player;

	private ProceduralGenerator generator;

	private SeedGenerator seed;

	private Vector3 origin;

	// Use this for initialization
	void Start ()
    {
		generator = new ProceduralGenerator ();

		seed = new SeedGenerator ("a totally random seed", 7);

		origin = generator.initalise (CHUNK_SIZE, seed);


		while (generator.generateMap (origin) == true) {
			//Loading
		}
	}

	public void InitPlayer() {
		player = (GameObject)Instantiate (Resources.Load("Steve/PlayerTorso"), new Vector3 (origin.x + 10, origin.y + 18, origin.z + 10), Quaternion.identity);
	}
		
	Vector3 getPlayerPosition() {
		if (player == null) {
			return origin;
		}

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
