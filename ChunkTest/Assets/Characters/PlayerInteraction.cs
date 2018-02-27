using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
	RaycastHit hit;
	bool isLeftMouseDown;
	Block currentBlock;
	private  Animator animator;
	Rigidbody rb;
    Vector3 worldPosition, posOfChunk, posOfBlock;
    Chunk currentChunk;

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator> ();
	}

	void FixedUpdate() {
		if (isLeftMouseDown) {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
            {
                if (hit.collider.gameObject.CompareTag("GrassBlock"))
                {
                    worldPosition = hit.collider.gameObject.GetComponent<Transform>().position;
                    posOfBlock = new Vector3(worldPosition.x % 16.0f, worldPosition.y % 16.0f, worldPosition.z % 16.0f);
                    posOfChunk = worldPosition - (posOfBlock);

                    currentChunk = GameObject.Find("World").GetComponent<WorldGenerator>().getPGenerator().getChunk(posOfChunk);
                    currentBlock = currentChunk.getBlock(posOfBlock);
                }
            }
            else {
                currentBlock = null;
            }
		}
    }

	// Update is called once per frame
	void Update () {
		isLeftMouseDown = Input.GetMouseButtonDown(0);
		if(isLeftMouseDown){
			animator.ResetTrigger("Interact");
			animator.SetTrigger("Interact");
            if (currentBlock != null) {
                currentBlock.damageBlock(10);
                if (currentBlock.getProperties().blockHealth <= 0) {
                    currentBlock.dropSelf();
                    currentChunk.removeBlock(posOfBlock);

                    Block adjacentBlock = currentChunk.getBlock(new Vector3(posOfBlock.x - 1.0f, posOfBlock.y, posOfBlock.z));
                    if (adjacentBlock != null) {
                        adjacentBlock.draw();
                    }
                    adjacentBlock = currentChunk.getBlock(new Vector3(posOfBlock.x-1.0f,posOfBlock.y,posOfBlock.z));
                    if (adjacentBlock != null)
                    {
                        adjacentBlock.draw();
                    }
                    if (adjacentBlock != null)
                    {
                        adjacentBlock.draw();
                    }
                    adjacentBlock = currentChunk.getBlock(new Vector3(posOfBlock.x+1.0f, posOfBlock.y, posOfBlock.z));
                    if (adjacentBlock != null)
                    {
                        adjacentBlock.draw();
                    }
                    adjacentBlock = currentChunk.getBlock(new Vector3(posOfBlock.x, posOfBlock.y-1.0f, posOfBlock.z));
                    if (adjacentBlock != null)
                    {
                        adjacentBlock.draw();
                    }
                    adjacentBlock = currentChunk.getBlock(new Vector3(posOfBlock.x, posOfBlock.y+1.0f, posOfBlock.z));
                    if (adjacentBlock != null)
                    {
                        adjacentBlock.draw();
                    }
                    adjacentBlock = currentChunk.getBlock(new Vector3(posOfBlock.x, posOfBlock.y, posOfBlock.z-1.0f));
                    if (adjacentBlock != null)
                    {
                        adjacentBlock.draw();
                    }
                    adjacentBlock = currentChunk.getBlock(new Vector3(posOfBlock.x, posOfBlock.y, posOfBlock.z+1.0f));
                    if (adjacentBlock != null)
                    {
                        adjacentBlock.draw();
                    }
                }
            }
		}
	}
}
