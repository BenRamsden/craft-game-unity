using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
	Rigidbody rb;
	GameObject world;
	RaycastHit hit;


	// Use this for initialization
	void Start () {
		bool isLeftMouseDown = Input.GetMouseButtonDown(0);
		rb = GetComponent<Rigidbody>();
		//world = GameObject.Find("World");

		//chunk = world.findChunk (currentPosition);
		//Block = Chunk.findblock (currentposition);

	}

	void FixedUpdate() {
		
		if (Physics.Raycast (transform.position, transform.forward, out hit, 100)) {
			if (hit.collider.gameObject.CompareTag ("GrassBlock")) {
				Debug.Log("There is something in front of the object!");
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
