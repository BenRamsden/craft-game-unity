using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
	Rigidbody rb;
	GameObject world;
	RaycastHit hit;
	bool isLeftMouseDown;
	Block currentBlock;
	private  Animator animator;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator> ();


	}

	void FixedUpdate() {
		if (isLeftMouseDown) {
			if (Physics.Raycast (transform.position, transform.forward, out hit, 5)) {
				if (hit.collider.gameObject.CompareTag ("GrassBlock")) {
					Vector3 worldPosition = hit.collider.gameObject.GetComponent<Transform> ().position;
					Vector3 moduloVector = new Vector3 (worldPosition.x % 16.0f, worldPosition.y % 16.0f, worldPosition.z % 16.0f);
					Vector3 positionOfChunk = worldPosition - (moduloVector);
					Vector3 positionOfBlockInChunk = moduloVector;

					Chunk currentChunk = GameObject.Find ("World").GetComponent<WorldGenerator> ().getPGenerator ().getChunk (positionOfChunk);

					currentBlock = currentChunk.getBlock (positionOfBlockInChunk);

					Debug.Log ("There is something in front of the object!");
				}
			} else {
				currentBlock = null;
			}
		}

	}

	// Update is called once per frame
	void Update () {
		isLeftMouseDown = Input.GetMouseButtonDown(0);
		if (isLeftMouseDown) {
			animator.ResetTrigger("Interact");
			animator.SetTrigger("Interact");
			if (currentBlock != null) {
				currentBlock.damageBlock (10);
			}
		}
	}
}
