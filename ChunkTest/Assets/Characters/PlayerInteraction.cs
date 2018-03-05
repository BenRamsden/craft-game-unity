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
    GameObject currentObject, previousObject;

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator> ();
	}

	void FixedUpdate() {
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
        {
            currentObject = hit.collider.gameObject;
            if (currentObject != null) {
                //hovering
            }

            if (currentObject.CompareTag("GrassBlock"))
            {
                worldPosition = currentObject.GetComponent<Transform>().position;
                posOfBlock = new Vector3(worldPosition.x % 16.0f, worldPosition.y % 16.0f, worldPosition.z % 16.0f);
                posOfChunk = worldPosition - (posOfBlock);

                currentChunk = GameObject.Find("World").GetComponent<WorldGenerator>().getPGenerator().getChunk(posOfChunk);
                currentBlock = currentChunk.getBlock(posOfBlock);
            }
            else
            {
                currentBlock = null;
            }
        }
    }

	// Update is called once per frame
	void Update () {
		isLeftMouseDown = Input.GetMouseButtonDown(0);
        if (isLeftMouseDown)
        {
            animator.ResetTrigger("Interact");
            animator.SetTrigger("Interact");
            if (currentBlock != null)
            {
                currentBlock.damageBlock(10);
                if (currentBlock.getProperties().blockHealth <= 0)
                {
                    currentBlock.dropSelf();
                    currentChunk.removeBlock(posOfBlock);

					Vector3[] vectors = new Vector3[6];
					vectors [0] = new Vector3 (posOfBlock.x - 1.0f, posOfBlock.y, posOfBlock.z);
					vectors [1] = new Vector3 (posOfBlock.x + 1.0f, posOfBlock.y, posOfBlock.z);
					vectors [2] = new Vector3 (posOfBlock.x, posOfBlock.y - 1.0f, posOfBlock.z);
					vectors [3] = new Vector3 (posOfBlock.x, posOfBlock.y + 1.0f, posOfBlock.z);
					vectors [4] = new Vector3 (posOfBlock.x, posOfBlock.y, posOfBlock.z - 1.0f);
					vectors [5] = new Vector3 (posOfBlock.x, posOfBlock.y, posOfBlock.z + 1.0f);


					Block adjacentBlock;
					for(int i = 0; i < 6; i++){
                        Debug.Log("Vector.x = " + vectors[i].x + ", Vector.y = " + vectors[i].y + ", Vector.z = " + vectors[i].z);

                        if (currentChunk.getBlock(vectors [i]) != null){
                            adjacentBlock = currentChunk.getBlock(vectors[i]);
                            if (adjacentBlock != null){
								adjacentBlock.draw();
							}
						}else{
                            if (vectors[i].x < 0){
                                posOfChunk = new Vector3(posOfChunk.x - 16.0f, posOfChunk.y, posOfChunk.z);
                                vectors[i].x += 16;
                            }else if(vectors[i].x > 15){
                                posOfChunk = new Vector3(posOfChunk.x + 16.0f, posOfChunk.y, posOfChunk.z);
                                vectors[i].x -= 16;
                            }

                            if (vectors[i].y < 0){
                                posOfChunk = new Vector3(posOfChunk.x, posOfChunk.y - 16.0f, posOfChunk.z);
                                vectors[i].y += 16;
                            }else if (vectors[i].y > 15){
                                posOfChunk = new Vector3(posOfChunk.x, posOfChunk.y + 16.0f, posOfChunk.z);
                                vectors[i].y -= 16;
                            }

                            if (vectors[i].z < 0){
                                posOfChunk = new Vector3(posOfChunk.x, posOfChunk.y, posOfChunk.z - 16.0f);
                                vectors[i].z += 16;
                            }else if (vectors[i].z > 15){
                                posOfChunk = new Vector3(posOfChunk.x, posOfChunk.y, posOfChunk.z + 16.0f);
                                vectors[i].z -= 16;
                            }
							currentChunk = GameObject.Find("World").GetComponent<WorldGenerator>().getPGenerator().getChunk(posOfChunk);
                            adjacentBlock = currentChunk.getBlock(vectors[i]);

                            if (currentChunk.getBlock(vectors[i]) != null){
                                adjacentBlock = currentChunk.getBlock(vectors[i]);
                                if (adjacentBlock != null)
                                {
                                    adjacentBlock.draw();
                                }
                            }
                        }
					}
                }
            }
        }
	}

    private void onTriggerEnter(Collider other)
    {
        Block newBlock = new Block();
        newBlock.BlockType = other.tag;
        GetComponent<Inventory>().addBlock(newBlock);
        GetComponent<Inventory>().setUI();
        Destroy(other.gameObject);
    }
}
