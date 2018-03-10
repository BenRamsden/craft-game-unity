using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
	RaycastHit hit;
	bool isLeftMouseDown;
	Block currentBlock;
	private  Animator animator;
	Rigidbody rb;
    Vector3 worldPosition, posOfChunk, posOfBlock;
    Chunk currentChunk;
    GameObject currentObject;
	Camera camera;
	LineRenderer targetLine;
	Vector3 rayOrigin;

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator> ();
		camera = GetComponentInChildren<Camera>();
		targetLine = GetComponent<LineRenderer>();
	}

	void FixedUpdate() {
		if (camera == null) {
			return;
		}

		rayOrigin = camera.ViewportToWorldPoint (new Vector3(.5f,.5f,0));
		//targetLine.SetPosition (0, rayOrigin);
		if (Physics.Raycast (rayOrigin, camera.transform.forward, out hit, 100)) {
			//targetLine.SetPosition (1, hit.point);
			currentObject = hit.collider.gameObject;
		} 
		//else {
			//targetLine.SetPosition (1, rayOrigin + (camera.transform.forward * 100));
		//}
    }

	// Update is called once per frame
	void Update () {
		if (currentObject == null) {
			return;
		}

		worldPosition = currentObject.GetComponent<Transform>().position;
		posOfChunk = HelperMethods.worldPositionToChunkPosition (worldPosition);
		posOfBlock = HelperMethods.vectorDifference (worldPosition, posOfChunk);

		currentChunk = GameObject.Find("World").GetComponent<WorldGenerator>().getPGenerator().getChunk(posOfChunk);
		currentBlock = currentChunk.getBlock(posOfBlock);

		isLeftMouseDown = Input.GetMouseButtonDown(0);
        if (isLeftMouseDown)
        {
            animator.ResetTrigger("Interact");
            animator.SetTrigger("Interact");
            if (currentBlock != null)
            {
                currentBlock.damageBlock(100);
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
                        if (currentChunk.getBlock(vectors [i]) != null){
                            adjacentBlock = currentChunk.getBlock(vectors[i]);
                            if (adjacentBlock != null){
								adjacentBlock.draw();
							}
						}
					}
                }
            }
        }
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.GetComponent<Rigidbody>() != null) {
			Block newBlock = new Block();
			newBlock.BlockType = col.gameObject.tag;
			GetComponent<Inventory>().addBlock(newBlock);
			GetComponent<Inventory>().setUI();
			Destroy(col.gameObject);
		}
	}
}
