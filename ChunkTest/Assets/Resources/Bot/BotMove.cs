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
	public float moveSpeed = 10.0F;

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

	/** Moving player using unity physics */
	void FixedUpdate () {
		float moveX = Random.Range(-1,1);// Input.GetAxis ("Horizontal");
		float moveZ = Random.Range(-1,1);// Input.GetAxis ("Vertical");

		movePlayer (moveX, moveZ);
	}

	void Update () {
        // Rotating player with mouse
		float mouseX = Random.Range(-1,1);// Input.GetAxis("Mouse X");
		float mouseY = Random.Range(-1,1);// -Input.GetAxis("Mouse Y"); // "-" because otherwise it is inverted up and down 
        
		moveCamera (mouseX, mouseY);
	}

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

	void moveCamera(float mouseX, float mouseY) {
		rotYHead += mouseX * mouseSensitivity * Time.deltaTime;
		rotX += mouseY * mouseSensitivity * Time.deltaTime;
		//stop the player being able to look up/down too much
		rotX = Mathf.Clamp(rotX, -x_AxisRotateClamp, x_AxisRotateClamp);
		//rotYHead = Mathf.Clamp(rotY, -y_AxisRotateClamp, y_AxisRotateClamp);

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

	void OnCollisionEnter(Collision collision)
	{
		// Get the walk sound from the collided block
		walkSound = collision.gameObject.GetComponent<BlockProperties> ().PlayerWalkSound;
	}
		
	void OnCollisionStay()
	{
		isGrounded = true;
		animator.SetBool ("isJumping", false);
	}

	/**
	 * Water blocks are triggers, check whether we are in water
	 */

	void OnTriggerStay(Collider trigger)
	{
		if (trigger.gameObject.name == "WaterBlock")
			isInWater = true;
	}

	void OnTriggerExit(Collider trigger)
	{
		if (trigger.gameObject.name == "WaterBlock")
			isInWater = false;
	}
}
