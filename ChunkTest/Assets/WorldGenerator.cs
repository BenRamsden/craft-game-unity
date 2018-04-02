using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {
    private static readonly int GENERATE_SPEED = 5;

    private static int CHUNK_SIZE = Chunk.CHUNK_SIZE;
    private static bool ENABLE_MENU = true;

    private Player player;
    private Player bot;

    private ProceduralGenerator generator;

    private SeedGenerator seed;

    private Vector3 origin;

	private GameObject inventoryMenu;
    private GameObject canvas;
    private GameObject camera;


    // Use this for initialization
    void Start() {
        generator = new ProceduralGenerator();

        seed = new SeedGenerator("a totally random seed", 7);

        origin = generator.initalise(CHUNK_SIZE, seed);

        if (ENABLE_MENU) {
			inventoryMenu = GameObject.Find ("inventoryMenu");
			inventoryMenu.SetActive(false);

            canvas = (GameObject)Instantiate(Resources.Load("Menu/Canvas"), new Vector3(0, 0, 0), Quaternion.identity);

            camera = (GameObject)Instantiate(Resources.Load("Menu/Camera"), new Vector3(origin.x + 0, origin.y + 20, origin.z + 60), Quaternion.LookRotation(new Vector3(0.0f, -0.3f, -1.0f)));
        } else {
            InitPlayer();
        }

        //while (generator.generateMap (origin) == true) {
        //Loading
        //}
    }

    public void InitPlayer() {
        if (ENABLE_MENU) {
			inventoryMenu.SetActive (true);
            Destroy(canvas);
            Destroy(camera);
        }

        bot = new Player(this, generator, "Player/Player_Steve", new Vector3(origin.x, origin.y + 20, origin.z + 45), Player.Behaviour.Bot);

        player = new Player(this, generator, "Player/Player_Steve", new Vector3(origin.x+5, origin.y + 20, origin.z + 35), Player.Behaviour.Human);
		player.gameObject.name = "Main_Character";
    }

    public GameObject CreatePlayer(string resourceString, Vector3 position) {
        GameObject gameObject = (GameObject)Instantiate(Resources.Load(resourceString), position, Quaternion.identity);
        return gameObject;
    }

    Vector3 getCenterChunkPos() {
        Vector3 centerChunk;

        if (player != null) {
            centerChunk = player.gameObject.transform.position;
        } else {
            centerChunk = origin;
        }

        return HelperMethods.worldPositionToChunkPosition(centerChunk);
    }

    public ProceduralGenerator getPGenerator() {
        return generator;
    }

    // Update is called once per frame
    void Update() {
        Vector3 centerChunk = getCenterChunkPos();

        while (generator.garbageCollect(centerChunk)) {

        }

        int generated = 0;
        while (generator.generateMap(centerChunk)) {
            if (generated++ == GENERATE_SPEED) {
                break;
            }
        }

        generator.secondPass();

        //generator.waterProcess(centerChunk);
    }
}
