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

					Vector3[] vectors = Vector3[6];
					vectors [0] = new Vector3 (posOfBlock.x - 1.0f, posOfBlock.y, posOfBlock.z);
					vectors [1] = new Vector3 (posOfBlock.x + 1.0f, posOfBlock.y, posOfBlock.z);
					vectors [2] = new Vector3 (posOfBlock.x, posOfBlock.y - 1.0f, posOfBlock.z);
					vectors [3] = new Vector3 (posOfBlock.x, posOfBlock.y + 1.0f, posOfBlock.z);
					vectors [4] = new Vector3 (posOfBlock.x, posOfBlock.y, posOfBlock.z - 1.0f);
					vectors [5] = new Vector3 (posOfBlock.x, posOfBlock.y, posOfBlock.z + 1.0f);


					Block adjacentBlock;
					for(int i = 0; i < 6; i++){
						if (adjacentBlock = currentChunk.getBlock (vectors [i]) != null) 
						{
							if (adjacentBlock != null) 
							{
								adjacentBlock.draw();
							}
						} 
						else
						{
							posOfChunk = new Vector3(
								(vectors[i].x<0&&vectors[i].x<15)? posOfChunk.x-16.0f:posOfChunk.x+16.0f,
								(vectors[i].y<0&&vectors[i].y<15)? posOfChunk.y-16.0f:posOfChunk.y+16.0f,
								(vectors[i].z<0&&vectors[i].z<15)? posOfChunk.z-16.0f:posOfChunk.z+16.0f);
							currentChunk = GameObject.Find("World").GetComponent<WorldGenerator>().getPGenerator().getChunk(posOfChunk);

						}

					}
                }
            }
		}
	}
}
