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

	private GameObject objectivePanel;

	private Game game;


    // Use this for initialization
    void Start() {
		CreateGenerator ();

		objectivePanel = GameObject.Find ("ObjectiveCanvas");
		objectivePanel.SetActive (false);

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

	public void StartGame(string seed = "", bool creative = true)
	{
		if (!creative)
		{
			Game.Instance.LoadCampaign ("scrapmetal");
			objectivePanel.SetActive (true);

			ObjectiveManager.Instance.NextObjective ();
		}

		CreateGenerator (seed);

		InitPlayer ();
	}

	private void CreateGenerator(string seed = "")
	{
		generator = new ProceduralGenerator();

		if (seed == "")
		{
			// Generate a random seed
			int randomSeed = (int)(System.DateTime.UtcNow - new System.DateTime (1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
			Random.InitState (randomSeed);
			randomSeed *= Random.Range (-1000, 1000);
			randomSeed += Random.Range ((int)-Mathf.Sqrt (Mathf.Sqrt (randomSeed)), (int)Mathf.Sqrt (Mathf.Sqrt (randomSeed * Random.Range (2, 5))));

			this.seed = new SeedGenerator (randomSeed, randomSeed.ToString ().Length / 2);
		}
		else
		{
			// If the seed length < 7, increase it. (It makes the generator better)
			if (seed.Length < 7)
				seed = seed + seed + seed + seed + seed;
			this.seed = new SeedGenerator (seed, seed.Length / 2);
		}

		origin = generator.initalise(CHUNK_SIZE, this.seed);
	}

    public void InitPlayer() {
        if (ENABLE_MENU) {
			inventoryMenu.SetActive (true);
            Destroy(canvas);
            Destroy(camera);
        }

        bot = new Player(this, generator, "Player/Player_Steve", new Vector3(origin.x, origin.y + 20, origin.z + 80), Player.Behaviour.Bot);

        player = new Player(this, generator, "Player/Player_Steve", new Vector3(origin.x+5, origin.y + 20, origin.z + 75), Player.Behaviour.Human);
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

		// Temp for detecting object destruction objective event
		if (bot != null)
		if (bot.gameObject == null)
			ObjectiveManager.Instance.ObjectiveCheck ("object_destroy", "Player_Steve(Clone)", 1);

        //generator.waterProcess(centerChunk);
    }
}