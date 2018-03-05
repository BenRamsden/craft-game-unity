using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
	private static int CHUNK_SIZE = Chunk.CHUNK_SIZE;
	private static bool ENABLE_MENU = true;

	private GameObject player;

	private ProceduralGenerator generator;

	private SeedGenerator seed;

	private Vector3 origin;

	private GameObject canvas;
	private GameObject camera;

	// Use this for initialization
	void Start ()
    {
		generator = new ProceduralGenerator ();

		seed = new SeedGenerator ("a totally random seed", 7);

		origin = generator.initalise (CHUNK_SIZE, seed);

		if (ENABLE_MENU) {
			canvas = (GameObject)Instantiate (Resources.Load ("Menu/Canvas"), new Vector3 (0, 0, 0), Quaternion.identity);

			camera = (GameObject)Instantiate (Resources.Load ("Menu/Camera"), new Vector3 (0, 20, 25), Quaternion.LookRotation (new Vector3 (0.0f, -0.3f, -1.0f)));
		} else {
			InitPlayer ();
		}
			
		while (generator.generateMap (origin) == true) {
			//Loading
		}
	}

	public void InitPlayer() {
		if (ENABLE_MENU) {
			Destroy (canvas);
			Destroy (camera);
		}
			
		player = (GameObject)Instantiate (Resources.Load("Steve/PlayerTorso"), new Vector3 (origin.x, origin.y+20, origin.z), Quaternion.identity);
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
