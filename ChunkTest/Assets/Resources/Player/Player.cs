using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    public GameObject gameObject { get; }
    public enum Behaviour { Human, Bot };

    private WorldGenerator wg;
    private ProceduralGenerator pg;

	/// <summary>
	/// Initializes a new instance of the <see cref="Player"/> class.
	/// </summary>
	/// <param name="wg">World Generator.</param>
	/// <param name="pg">Procedural Generator.</param>
	/// <param name="resourceString">Resource string.</param>
	/// <param name="position">Position.</param>
	/// <param name="behaviour">Behaviour.</param>
    public Player(WorldGenerator wg, ProceduralGenerator pg, string resourceString, Vector3 position, Behaviour behaviour) {
        this.wg = wg;
        this.pg = pg;

        gameObject = wg.CreatePlayer(resourceString, position);
		if (behaviour == Behaviour.Human)
			gameObject.name = "player";

        if (behaviour.Equals(Behaviour.Human)) {
            PlayerMove pm = gameObject.AddComponent<PlayerMove>();
            pm.behaviour = Behaviour.Human;
            pm.pg = pg;
            PlayerInteraction pi = gameObject.AddComponent<PlayerInteraction>();
            pi.pg = pg;
            gameObject.AddComponent<Inventory>();
        } else if (behaviour.Equals(Behaviour.Bot)) {
            PlayerMove pm = gameObject.AddComponent<PlayerMove>();
            pm.behaviour = Behaviour.Bot;
            pm.pg = pg;
        }
    }
}
