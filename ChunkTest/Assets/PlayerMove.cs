using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	public Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;
	}

	/** Moving player using unity physics */
	public float moveSpeed = 10.0F;
	void FixedUpdate () {
		float moveX = Input.GetAxis ("Horizontal");
		float moveZ = Input.GetAxis ("Vertical");
		//float move
		Vector3 movement = new Vector3 (moveX, 0.0f, moveZ);
		//use delta time to make it consistent rotation, fps doesnt matter
		rb.transform.Translate(movement * moveSpeed * Time.deltaTime);
	}


	/** Rotating player with mouse*/
	private float mouseX, mouseY;
	public static float x_AxisRotateClamp = 80.0f;
	public float mouseSensitivity = 1000.0f;
	private float rotY = 0.0f; // rotation on the up/y axis
	private float rotX = 0.0f; // rotation on the right/x axis
	void Update () {
		mouseX = Input.GetAxis("Mouse X");
		mouseY = -Input.GetAxis("Mouse Y"); // "-" because it is inverted up and down 
		rotY += mouseX * mouseSensitivity * Time.deltaTime;
		rotX += mouseY * mouseSensitivity * Time.deltaTime;
		//stop the player being able to look up/down too much
		rotX = Mathf.Clamp(rotX, -x_AxisRotateClamp, x_AxisRotateClamp);
		rb.transform.rotation= Quaternion.Euler(rotX, rotY, 0.0f);
	}



}
