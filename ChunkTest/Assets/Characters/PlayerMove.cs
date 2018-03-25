using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	public Rigidbody rb;
	private  Animator animator;
	public bool isGrounded;

	public readonly Vector3 jump = new Vector3(0.0f, 5.0f, 0.0f);

	private float mouseX, mouseY, rotY,rotYHead, rotX, moveX, moveZ = 0.0f;
	public float mouseSensitivity = 200.0f;
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
		moveX = Input.GetAxis ("Horizontal");
		moveZ = Input.GetAxis ("Vertical");
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


	void OnCollisionStay()
	{
		isGrounded = true;
		animator.SetBool ("isJumping", false);
	}


	void Update () {
        // Rotating player with mouse
        mouseX = Input.GetAxis("Mouse X");
		mouseY = -Input.GetAxis("Mouse Y"); // "-" because otherwise it is inverted up and down 
        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        //set up rotations for the torso and head. allow head to look up and down but not torso.
        Quaternion rotationHead = Quaternion.Euler(rotX, rotY, 0.0f);
		Quaternion rotationTorso = Quaternion.Euler(0.0f, rotY, 0.0f);

		transform.rotation = rotationTorso;
        transform.GetChild(0).rotation = rotationHead;

        //player jumping code
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded){
			rb.AddForce(jump, ForceMode.Impulse);
			isGrounded = false;

            animator.SetBool("isJumping", true);

            if (Random.Range(0, 2) != 0)
            {
                animator.SetBool("alternateJumpLeg", false);
            }
            else
            {
                animator.SetBool("alternateJumpLeg", true);
            }

            if (Random.Range(0, 2) != 0)
            {
                animator.SetBool("alternateJumpArm", false);
            }
            else
            {
                animator.SetBool("alternateJumpArm", true);
            }


            audioSource.PlayOneShot (jumpSound);
		}

		// If a key is pressed, play a sound
		if (Input.anyKey && !audioSource.isPlaying)
		{
			if (isInWater)
				audioSource.PlayOneShot (swimSound);
			else
				audioSource.PlayOneShot (walkSound);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		// Get the walk sound from the collided block
		walkSound = collision.gameObject.GetComponent<BlockProperties> ().PlayerWalkSound;
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
