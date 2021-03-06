﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public ProceduralGenerator pg;
    public Player.Behaviour behaviour;
    public Rigidbody rb;
    private Animator animator;

    public readonly Vector3 jump = new Vector3(0.0f, 1f, 0.0f);

    public float mouseSensitivity = 100.0f;
	public float moveSpeed = 13.0f;

    private AudioClip jumpSound;
    private AudioClip grassWalkSound;
    private AudioClip swimSound;
    private AudioClip walkSound;
    private AudioSource audioSource;

    private bool isInWater = false;

    private const float x_AxisRotateClamp = 80.0f;
    private const float y_AxisRotateClamp = 15.0f;

    static Vector3 leftCollider = new Vector3(-1.5f, -0.5f, 0.0f);
    static Vector3 frontCollider = new Vector3(0.0f, -0.5f, 0.0f);
    static Vector3 rightCollider = new Vector3(1.5f, -0.5f, 0.0f);

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

        jumpSound = (AudioClip)Resources.Load("Sounds/jump");
        grassWalkSound = (AudioClip)Resources.Load("Sounds/walk_grass");
        swimSound = (AudioClip)Resources.Load("Sounds/swim");
        audioSource = gameObject.AddComponent<AudioSource>();

		if (behaviour.Equals (Player.Behaviour.Human)) {
			Camera cam = transform.GetComponentInChildren<Camera> ();
			cam.enabled = true;
		} else {
			AudioListener al = transform.GetComponentInChildren<AudioListener> ();
			al.enabled = false;
		}
    }

	// Update is called once per frame
    void Update() {
        if (behaviour.Equals(Player.Behaviour.Human)) {
            PlayerUpdate();
        } else if (behaviour.Equals(Player.Behaviour.Bot)) {
            //BotUpdate();
        }
    }

	// FixedUpdate is called once per tick
    void FixedUpdate() {
        if (behaviour.Equals(Player.Behaviour.Human)) {
            PlayerFixedUpdate();
        } else if (behaviour.Equals(Player.Behaviour.Bot)) {
            BotFixedUpdate();
        }
    }

	/// <summary>
	/// Handles player movement updates.
	/// </summary>
    private void PlayerFixedUpdate() {
        rb.transform.Translate(movement * moveSpeed * Time.deltaTime);
        if (IsGrounded()) {
            movement.y = 0;
        }
    }

	Vector3 collisionRayTransform = new Vector3 (0.0f, -1.0f, 0.0f);
	Quaternion leftAngle = Quaternion.AngleAxis (-15, new Vector3 (0, 1, 0));
	Quaternion rightAngle = Quaternion.AngleAxis (15, new Vector3 (0, 1, 0));

	RaycastHit hit;

	/// <summary>
	/// Handles bot movement updates.
	/// </summary>
    private void BotFixedUpdate() {
		float moveX = 0;// Input.GetAxis ("Horizontal");
		float moveZ = 0.2f;// Input.GetAxis ("Vertical");

		MovePlayer(moveX, moveZ);

		Vector3 rayLeft = leftAngle * transform.forward;
		Vector3 rayCenter = transform.forward;
		Vector3 rayRight = rightAngle * transform.forward;

		Vector3 from = rb.transform.position + collisionRayTransform;

		if (Physics.Raycast (from, rayLeft, out hit, 3.0f)) {
			MoveCamera (1, 0);
		}

		if (Physics.Raycast (from, rayRight, out hit, 3.0f)) {
			MoveCamera (-1, 0);
		}

		if (Physics.Raycast (from, rayCenter, out hit, 1.5f)) {
			rb.AddForce(Vector3.up*2.0f, ForceMode.Impulse);
		}
    }

    float moveX;
    float moveZ;
    Vector3 movement = Vector3.zero;

	/// <summary>
	/// Handles how to move the player model based on user input.
	/// </summary>
    public void PlayerUpdate() {
		if (GetComponent<Inventory> ().isToggled) {
			Cursor.lockState = CursorLockMode.None;
			return;
		} else {
			Cursor.lockState = CursorLockMode.Locked;
		}

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        movement.x = moveX;
        movement.z = moveZ;

        // Rotating player with mouse
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y"); // "-" because otherwise it is inverted up and down
        MoveCamera(mouseX, mouseY);

        // Player jumping code
        if (IsGrounded()) {
            animator.SetBool("isJumping", false);
            if (Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }
        }

        // Check for which type of movement for appropiate animation
        if (moveX != 0 || moveZ != 0) {
            animator.SetBool("isMoving", true);
        } else {
            animator.SetBool("isMoving", false);
        }

        // If a key is pressed, play a sound
        if (Input.anyKey && !audioSource.isPlaying) {
            if (isInWater)
                audioSource.PlayOneShot(swimSound);
            else
                audioSource.PlayOneShot(walkSound);
        }
    }

	/// <summary>
	/// Determines whether the player is grounded.
	/// </summary>
	/// <returns><c>true</c> if this instance is grounded; otherwise, <c>false</c>.</returns>
    public bool IsGrounded() {
        RaycastHit hit;
        if (Physics.Raycast(rb.transform.position, -Vector3.up, out hit, 1.15f)) {
            return true;
        }
        return false;
    }

	/// <summary>
	/// Listens for collisions with blocks and plays the corresponding sound.
	/// </summary>
	/// <param name="collision">Collision.</param>
    public void OnCollisionEnter(Collision collision) {
        walkSound = collision.gameObject.GetComponent<BlockProperties>().PlayerWalkSound;
    }

	/// <summary>
	/// Checks whether player has entered water.
	/// </summary>
	/// <param name="trigger">Trigger.</param>
    public void OnTriggerStay(Collider trigger) {
        if (trigger.gameObject.name == "WaterBlock")
            isInWater = true;
    }

	/// <summary>
	/// Checks whether player has exited water.
	/// </summary>
	/// <param name="trigger">Trigger.</param>
    public void OnTriggerExit(Collider trigger) {
        if (trigger.gameObject.name == "WaterBlock")
            isInWater = false;
    }

	/// <summary>
	/// Handle the player jumping.
	/// </summary>
    public void Jump() {
        movement.y = jump.y;
        //rb.AddForce(jump, ForceMode.Impulse);
        animator.SetBool("isJumping", true);

        if (Random.Range(0, 2) == 0) {
            animator.SetBool("alternateJumpLeg", false);
        } else {
            animator.SetBool("alternateJumpLeg", true);
        }
        if (Random.Range(0, 2) == 0) {
            animator.SetBool("alternateJumpArm", false);
        } else {
            animator.SetBool("alternateJumpArm", true);
        }
        audioSource.PlayOneShot(jumpSound);
    }

	/// <summary>
	/// Moves the player on the x-z plane.
	/// </summary>
	/// <param name="moveX">Move x.</param>
	/// <param name="moveZ">Move z.</param>
    void MovePlayer(float moveX, float moveZ) {
        //float move
        Vector3 movement = new Vector3(moveX, 0.0f, moveZ);

        rb.transform.Translate(movement * moveSpeed * Time.deltaTime);

        //check for which type of movement for appropiate animation
        if (moveX != 0 || moveZ != 0) {
            animator.SetBool("isMoving", true);
        } else {
            animator.SetBool("isMoving", false);
        }
    }

    private float rotY, rotYHead, rotX = 0.0f;

	/// <summary>
	/// Moves the camera.
	/// </summary>
	/// <param name="mouseX">Mouse x.</param>
	/// <param name="mouseY">Mouse y.</param>
    void MoveCamera(float mouseX, float mouseY) {
        rotYHead += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;
        //stop the player being able to look up/down too much
        rotX = Mathf.Clamp(rotX, -x_AxisRotateClamp, x_AxisRotateClamp);

        //set up rotations for the torso and head. allow head to look up and down but not torso.
        float diff = (rotYHead - rotY);
        if (diff != y_AxisRotateClamp) {
            rotY += diff / 2;
        }
        Quaternion rotationHead = Quaternion.Euler(rotX, rotYHead, 0.0f);

        Quaternion rotationTorso = Quaternion.Euler(0.0f, rotY, 0.0f);
        transform.rotation = rotationTorso;
        transform.GetChild(0).rotation = rotationHead;
    }

}
