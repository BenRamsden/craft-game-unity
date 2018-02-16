using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	public Rigidbody rb;
	private  Animator animator;
	public bool isGrounded;

	public Vector3 jump;

	private float mouseX, mouseY, rotY, rotX, moveX, moveZ = 0.0f;
	public float mouseSensitivity = 1000.0f;
	public float moveSpeed = 10.0F;

	public static float x_AxisRotateClamp = 80.0f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;
		jump = new Vector3 (0.0f, 5.0f, 0.0f);
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
	}

	/** Rotating player with mouse*/

	void Update () {
		mouseX = Input.GetAxis("Mouse X");
		mouseY = -Input.GetAxis("Mouse Y"); // "-" because it is inverted up and down 
		rotY += mouseX * mouseSensitivity * Time.deltaTime;
		//rotX += mouseY * mouseSensitivity * Time.deltaTime;
		//stop the player being able to look up/down too much
		rotX = Mathf.Clamp(rotX, -x_AxisRotateClamp, x_AxisRotateClamp);
		rb.transform.rotation= Quaternion.Euler(rotX, rotY, 0.0f);

		if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
			rb.AddForce(jump, ForceMode.Impulse);
			isGrounded = false;
		}
	}
}
