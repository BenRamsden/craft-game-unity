using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
	public ProceduralGenerator pg;

	RaycastHit hit;
	bool isLeftMouseDown, isRightMouseClicked;
	Block currentBlock;
	private  Animator animator;
	Rigidbody rb;
    Vector3 worldPosition, posOfChunk, posOfBlock;
    Chunk currentChunk;
    GameObject currentObject;
	Inventory inventory;
	Camera camera;
	Vector3 rayOrigin;
	int timer = 10, currentActiveItem = 6;

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator> ();
		camera = GetComponentInChildren<Camera>();
		inventory = GetComponent<Inventory>();
	}

	void FixedUpdate() {
		if (camera == null) {
			return;
		}

		rayOrigin = camera.ViewportToWorldPoint (new Vector3(.5f,.5f,0));
		if (Physics.Raycast (rayOrigin, camera.transform.forward, out hit, 100)) {
			currentObject = hit.collider.gameObject;
		} 
    }

	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			if (currentObject != null) {
				isLeftMouseDown = Input.GetMouseButton(0);
				isRightMouseClicked = Input.GetMouseButtonDown(1);

				worldPosition = currentObject.GetComponent<Transform>().position;
				posOfChunk = HelperMethods.worldPositionToChunkPosition (worldPosition);
				posOfBlock = HelperMethods.vectorDifference (worldPosition, posOfChunk);

				currentChunk = GameObject.Find("World").GetComponent<WorldGenerator>().getPGenerator().getChunk(posOfChunk);
				currentBlock = currentChunk.getBlock(posOfBlock);

				if (isLeftMouseDown && timer < 1){
					interactWithBlock();
					timer = 10;
				}
				timer = (timer < 1)? 0: --timer;

				if(isRightMouseClicked){
					placeBlock();
				}
			}

			if (Input.GetKey (KeyCode.Keypad1) || Input.GetKey (KeyCode.Alpha1)) {
				currentActiveItem = 1;
			}else if(Input.GetKey (KeyCode.Keypad2) || Input.GetKey (KeyCode.Alpha2)){
				currentActiveItem = 2;
			}else if(Input.GetKey (KeyCode.Keypad3) || Input.GetKey (KeyCode.Alpha3)){
				currentActiveItem = 3;
			}else if(Input.GetKey (KeyCode.Keypad4) || Input.GetKey (KeyCode.Alpha4)){
				currentActiveItem = 4;
			}else if(Input.GetKey (KeyCode.Keypad5) || Input.GetKey (KeyCode.Alpha5)){
				currentActiveItem = 5;
			}else if(Input.GetKey (KeyCode.Keypad6) || Input.GetKey (KeyCode.Alpha6)){
				currentActiveItem = 6;
			}
			inventory.setActiveList (currentActiveItem - 1);
			inventory.setUI();
		}
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.GetComponent<Rigidbody>() != null) {
			Block newBlock = new Block();
			newBlock.BlockType = col.gameObject.tag;
			if (inventory.addBlock (newBlock)) {
				inventory.setUI();
				Destroy(col.gameObject);
			} else {
				Debug.Log ("Inventory is full.");
			}
		}
	}


	private void interactWithBlock(){
		animator.ResetTrigger("Interact");
		animator.SetTrigger("Interact");
		if (currentBlock != null)
		{
			currentBlock.damageBlock(100);
			if (currentBlock.getProperties().blockHealth <= 0)
			{
				Vector3 deletedPos = currentBlock.getPosition ();
				currentBlock.dropSelf();
				currentChunk.removeBlock(posOfBlock);

				drawAdjacentBlock (deletedPos.x-1, deletedPos.y, deletedPos.z);
				drawAdjacentBlock (deletedPos.x+1, deletedPos.y, deletedPos.z);
				drawAdjacentBlock (deletedPos.x, deletedPos.y-1, deletedPos.z);
				drawAdjacentBlock (deletedPos.x, deletedPos.y+1, deletedPos.z);
				drawAdjacentBlock (deletedPos.x, deletedPos.y, deletedPos.z-1);
				drawAdjacentBlock (deletedPos.x, deletedPos.y, deletedPos.z+1);
			}
		}
	}

	private void drawAdjacentBlock(float x, float y, float z) {
		Vector3 deletedPos = new Vector3 (x, y, z);
		Vector3 chunkPos = HelperMethods.worldPositionToChunkPosition (deletedPos);
		Vector3 blockPos = HelperMethods.vectorDifference (chunkPos,deletedPos);

		Chunk chunk = pg.getChunk (chunkPos);

		if (chunk == null)
			return;
					
		Block block = chunk.getBlock (blockPos);

		if (block == null)
			return;

		block.draw ();
	}

	private void placeBlock(){
		animator.ResetTrigger("Interact");
		animator.SetTrigger("Interact");
		if(currentBlock != null){
			String blockType;
			if ((blockType = inventory.removeBlock()) != null){
				Block tempBlock = currentChunk.CreateBlock(blockType, (int)posOfBlock.x, (int)posOfBlock.y+1, (int)posOfBlock.z);
				tempBlock.draw();
				inventory.setUI();
			}
		}
	}

}
