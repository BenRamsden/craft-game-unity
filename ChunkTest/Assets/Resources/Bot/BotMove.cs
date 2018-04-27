using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMove : MonoBehaviour {
	public ProceduralGenerator pg;
	public Rigidbody rb;
	private  Animator animator;
	public bool isGrounded;

	public readonly Vector3 jump = new Vector3(0.0f, 5.0f, 0.0f);
	private float rotY,rotYHead, rotX = 0.0f;
	public float mouseSensitivity = 1000.0f;
	public float moveSpeed = 5.0F;

	private AudioClip jumpSound;
	private AudioClip grassWalkSound;
	private AudioClip swimSound;
	private AudioClip walkSound;
	private AudioSource audioSource;

	private bool isInWater = false;

	public static float x_AxisRotateClamp = 80.0f;
    public static float y_AxisRotateClamp = 10.0f;

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;

		jumpSound = (AudioClip)Resources.Load ("Sounds/jump");
		grassWalkSound = (AudioClip)Resources.Load ("Sounds/walk_grass");
		swimSound = (AudioClip)Resources.Load ("Sounds/swim");
		audioSource = gameObject.AddComponent<AudioSource> ();
	}

	static Vector3 leftCollider = new Vector3 (-1.5f, -0.5f, 0.0f);
	static Vector3 frontCollider = new Vector3(0.0f,-0.5f,0.0f);
	static Vector3 rightCollider = new Vector3 (1.5f, -0.5f, 0.0f);

	// FixedUpdate is called once per tick
	void FixedUpdate () {
		if (avoidCollision (frontCollider)) {
			jumpPlayer ();
		}

		float moveX = 0;// Input.GetAxis ("Horizontal");
		float moveZ = 0.2f;// Input.GetAxis ("Vertical");

		movePlayer (moveX, moveZ);
	}

	// Update is called once per frame
	void Update () {
        // Rotating player with mouse
		float mouseX = 0;// Input.GetAxis("Mouse X");
		float mouseY = 0;// -Input.GetAxis("Mouse Y"); // "-" because otherwise it is inverted up and down 
        
		moveCamera (mouseX, mouseY);
	}

	/// <summary>
	/// The basis of the bot AI, avoids colliding with the terrain.
	/// </summary>
	/// <returns><c>true</c>, if collision can be avoided, <c>false</c> otherwise.</returns>
	/// <param name="collider">Collider.</param>
	private bool avoidCollision(Vector3 collider) {
		Vector3 forward = transform.forward;

		Vector3 botPos = this.gameObject.transform.position;
		botPos += collider;
		botPos += forward * 3.0f;

		Vector3 chunkPos = HelperMethods.worldPositionToChunkPosition (botPos);
		Vector3 blockPos = HelperMethods.vectorDifference (chunkPos,botPos);

		Chunk chunk = pg.getChunk (chunkPos);

		if (chunk == null)
			return false;

		Block block = chunk.getBlock (blockPos);

		if (block == null)
			return false;

		if (block.resourceString == "WaterBlock")
			return false;

		return true;
	}

	/// <summary>
	/// Causes the player to jump.
	/// </summary>
	private void jumpPlayer() {
		if (isGrounded){
			rb.AddForce(jump, ForceMode.Impulse);
			isGrounded = false;

			animator.SetBool ("isJumping", true);

			audioSource.PlayOneShot (jumpSound);
		}
	}

	/// <summary>
	/// Moves the bot.
	/// </summary>
	/// <param name="moveX">Move x.</param>
	/// <param name="moveZ">Move z.</param>
	void movePlayer(float moveX, float moveZ) {
		//float move
		Vector3 movement = new Vector3 (moveX, 0.0f, moveZ);

		//use delta time to make it consistent rotation, fps doesnt matter
		rb.transform.Translate(movement * moveSpeed * Time.deltaTime);

		//check for which type of movement for appropiate animation
		if (moveX != 0 || moveZ != 0) {
			animator.SetBool ("isMoving", true);
		} else {
			animator.SetBool ("isMoving", false);
		}
	}

	/// <summary>
	/// Moves the camera.
	/// </summary>
	/// <param name="mouseX">Mouse x.</param>
	/// <param name="mouseY">Mouse y.</param>
	void moveCamera(float mouseX, float mouseY) {
		rotYHead += mouseX * mouseSensitivity * Time.deltaTime;
		rotX += mouseY * mouseSensitivity * Time.deltaTime;
		//stop the player being able to look up/down too much
		rotX = Mathf.Clamp(rotX, -x_AxisRotateClamp, x_AxisRotateClamp);
		//rotYHead = Mathf.Clamp(rotY, -y_AxisRotateClamp, y_AxisRotateClamp);

		if (avoidCollision (leftCollider)) {
			rotYHead += 45;
		} else if (avoidCollision (rightCollider)) {
			rotYHead -= 45;
		}

		//set up rotations for the torso and head. allow head to look up and down but not torso.
		Quaternion rotationHead = Quaternion.Euler(rotX, rotYHead, 0.0f);
		transform.GetChild(0).rotation = rotationHead;

		float diff = rotYHead - rotY;

		if(diff > y_AxisRotateClamp || diff < y_AxisRotateClamp)
		{
			rotY += diff / 3;
		}

		Quaternion rotationTorso = Quaternion.Euler(0.0f, rotY, 0.0f);
		transform.rotation = rotationTorso;
	}

	/// <summary>
	/// Raises the collision enter event.
	/// </summary>
	/// <param name="collision">Collision.</param>
	public void OnCollisionEnter(Collision collision) {
		// Get the walk sound from the collided block
		walkSound = collision.gameObject.GetComponent<BlockProperties> ().PlayerWalkSound;
	}
		
	/// <summary>
	/// Raises the collision stay event.
	/// </summary>
	public void OnCollisionStay() {
		isGrounded = true;
		animator.SetBool ("isJumping", false);
	}

	/// <summary>
	/// Raises the trigger stay event.
	/// </summary>
	/// <param name="trigger">Trigger.</param>
	public void OnTriggerStay(Collider trigger) {
		if (trigger.gameObject.name == "WaterBlock")
			isInWater = true;
	}

	/// <summary>
	/// Raises the trigger exit event.
	/// </summary>
	/// <param name="trigger">Trigger.</param>
	public void OnTriggerExit(Collider trigger) {
		if (trigger.gameObject.name == "WaterBlock")
			isInWater = false;
	}
}
