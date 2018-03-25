using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

	public GameObject gameObject { get; }
	private WorldGenerator wg;

	public Player(WorldGenerator wg) {
		this.wg = wg;
		gameObject = wg.CreatePlayer ();
		gameObject.AddComponent<PlayerMove> ();
		gameObject.AddComponent<PlayerInteraction> ();
		gameObject.AddComponent<Inventory> ();
	}
}
