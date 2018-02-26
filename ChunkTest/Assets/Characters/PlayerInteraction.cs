using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
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
        Vector3 worldPosition, posOfChunk, posOfBlock;
		if (isLeftMouseDown) {
			if (Physics.Raycast (transform.position, transform.forward, out hit, 5)) {
                if (hit.collider.gameObject.CompareTag("GrassBlock")) {
                    worldPosition = hit.collider.gameObject.GetComponent<Transform>().position;
                    posOfBlock = new Vector3(worldPosition.x % 16.0f, worldPosition.y % 16.0f, worldPosition.z % 16.0f);
                    posOfChunk = worldPosition - (posOfBlock);

                    Chunk currentChunk = GameObject.Find("World").GetComponent<WorldGenerator>().getPGenerator().getChunk(posOfChunk);
                    currentBlock = currentChunk.getBlock(posOfBlock);
                }
			}
		}else {
            currentBlock = null;
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
