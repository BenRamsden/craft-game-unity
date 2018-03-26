using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

    public GameObject gameObject { get; }
    public enum Behaviour { Human, Bot };

    private WorldGenerator wg;
    private ProceduralGenerator pg;

    public Player(WorldGenerator wg, ProceduralGenerator pg, string resourceString, Vector3 position, Behaviour behaviour) {
        this.wg = wg;
        this.pg = pg;

        gameObject = wg.CreatePlayer(resourceString, position);

        if (behaviour.Equals(Behaviour.Human)) {
            gameObject.AddComponent<PlayerMove>();
            PlayerInteraction pi = gameObject.AddComponent<PlayerInteraction>();
            pi.pg = pg;
            gameObject.AddComponent<Inventory>();
        } else if (behaviour.Equals(Behaviour.Bot)) {
            BotMove bm = gameObject.AddComponent<BotMove>();
            bm.pg = pg;
        }

    }
}
