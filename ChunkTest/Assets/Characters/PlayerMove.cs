using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public ProceduralGenerator pg;
    public Player.Behaviour behaviour;

    public Rigidbody rb;
    private Animator animator;
    public bool isGrounded;

    public readonly Vector3 jump = new Vector3(0.0f, 10.0f, 0.0f);

    public float mouseSensitivity = 200.0f;
    public float moveSpeed = 20.0F;

    private AudioClip jumpSound;
    private AudioClip grassWalkSound;
    private AudioClip swimSound;
    private AudioClip walkSound;
    private AudioSource audioSource;

    private bool isInWater = false;

    private const float x_AxisRotateClamp = 80.0f;
    private const float y_AxisRotateClamp = 20.0f;

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

        if (behaviour.Equals(Player.Behaviour.Human)) {
            GameObject head = transform.GetChild(0).gameObject;
            head.AddComponent<Camera>();
            head.AddComponent<AudioListener>();
        }
    }

    void Update() {
        if (behaviour.Equals(Player.Behaviour.Human)) {
            HumanUpdate();
        } else if (behaviour.Equals(Player.Behaviour.Bot)) {
            BotUpdate();
        }
    }

    /** Moving player using unity physics */
    void FixedUpdate() {
        if (behaviour.Equals(Player.Behaviour.Human)) {
            HumanFixedUpdate();
        } else if (behaviour.Equals(Player.Behaviour.Bot)) {
            BotFixedUpdate();
        }
    }

    void HumanFixedUpdate() {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        //float move
        Vector3 movement = new Vector3(moveX, 0.0f, moveZ);

        //use delta time to make it consistent rotation, fps doesnt matter
        rb.transform.Translate(movement * moveSpeed * Time.deltaTime);

        //check for which type of movement for appropiate animation
        if (moveX != 0 || moveZ != 0) {
            animator.SetBool("isMoving", true);
        } else {
            animator.SetBool("isMoving", false);
        }
    }

    void BotFixedUpdate() {
        if (avoidCollision(frontCollider)) {
            jumpPlayer();
        }

        float moveX = 0;// Input.GetAxis ("Horizontal");
        float moveZ = 0.2f;// Input.GetAxis ("Vertical");

        MovePlayer(moveX, moveZ);
    }

    void HumanUpdate() {
        // Rotating player with mouse
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y"); // "-" because otherwise it is inverted up and down
        MoveCamera(mouseX, mouseY);

#if false        
        rotYHead += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -x_AxisRotateClamp, x_AxisRotateClamp);

        //set up rotations for the torso and head. allow head to look up and down but not torso.



        float diff = rotYHead - rotY;

        if (diff > y_AxisRotateClamp || diff < y_AxisRotateClamp) {
            rotY += diff / 3;
        }
        Quaternion rotationHead = Quaternion.Euler(rotX, rotYHead, 0.0f);
        Quaternion rotationTorso = Quaternion.Euler(0.0f, rotY, 0.0f);
        transform.rotation = rotationTorso;
        transform.GetChild(0).rotation = rotationHead; 
#endif

        //player jumping code
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            rb.AddForce(jump, ForceMode.Impulse);
            isGrounded = false;

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

        // If a key is pressed, play a sound
        if (Input.anyKey && !audioSource.isPlaying) {
            if (isInWater)
                audioSource.PlayOneShot(swimSound);
            else
                audioSource.PlayOneShot(walkSound);
        }
    }

    void BotUpdate() {
        // Rotating player with mouse
        float mouseX = 0;// Input.GetAxis("Mouse X");
        float mouseY = 0;// -Input.GetAxis("Mouse Y"); // "-" because otherwise it is inverted up and down 

        MoveCamera(mouseX, mouseY);
    }

    void OnCollisionStay() {
        isGrounded = true;
        animator.SetBool("isJumping", false);
    }

    void OnCollisionEnter(Collision collision) {
        // Get the walk sound from the collided block
        walkSound = collision.gameObject.GetComponent<BlockProperties>().PlayerWalkSound;
    }

    /**
	 * Water blocks are triggers, check whether we are in water
	 */

    void OnTriggerStay(Collider trigger) {
        if (trigger.gameObject.name == "WaterBlock")
            isInWater = true;
    }

    void OnTriggerExit(Collider trigger) {
        if (trigger.gameObject.name == "WaterBlock")
            isInWater = false;
    }

    /* Methods below imported from BotMove */

    bool avoidCollision(Vector3 collider) {
        Vector3 forward = transform.forward;

        Vector3 botPos = this.gameObject.transform.position;
        botPos += collider;
        botPos += forward * 3.0f;

        /*if (cube == null) {
			cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
			cube.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);

			Physics.IgnoreCollision (cube.GetComponent<Collider> (), GetComponent<Collider> ());
		}
			
		cube.transform.position = botPos;*/

        Vector3 chunkPos = HelperMethods.worldPositionToChunkPosition(botPos);
        Vector3 blockPos = HelperMethods.vectorDifference(chunkPos, botPos);

        Chunk chunk = pg.getChunk(chunkPos);

        if (chunk == null)
            return false;

        Block block = chunk.getBlock(blockPos);

        if (block == null)
            return false;

        if (block.resourceString == "WaterBlock")
            return false;

        return true;
    }

    void jumpPlayer() {
        if (isGrounded) {
            rb.AddForce(jump, ForceMode.Impulse);
            isGrounded = false;

            animator.SetBool("isJumping", true);

            audioSource.PlayOneShot(jumpSound);
        }
    }

    void MovePlayer(float moveX, float moveZ) {
        //float move
        Vector3 movement = new Vector3(moveX, 0.0f, moveZ);

        //use delta time to make it consistent rotation, fps doesnt matter
        rb.transform.Translate(movement * moveSpeed * Time.deltaTime);

        //check for which type of movement for appropiate animation
        if (moveX != 0 || moveZ != 0) {
            animator.SetBool("isMoving", true);
        } else {
            animator.SetBool("isMoving", false);
        }
    }

    private float rotY, rotYHead, rotX = 0.0f;
    void MoveCamera(float mouseX, float mouseY) {
        rotYHead += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;
        //stop the player being able to look up/down too much
        rotX = Mathf.Clamp(rotX, -x_AxisRotateClamp, x_AxisRotateClamp);

        if (behaviour.Equals(Player.Behaviour.Bot)) {
            if (avoidCollision(leftCollider)) {
                rotYHead += 45;
            } else if (avoidCollision(rightCollider)) {
                rotYHead -= 45;
            }
        }


        //set up rotations for the torso and head. allow head to look up and down but not torso.

        float diff = rotYHead - rotY;
        if (diff > y_AxisRotateClamp || diff < y_AxisRotateClamp) {
            rotY += diff / 3;
        }
        Quaternion rotationHead = Quaternion.Euler(rotX, rotYHead, 0.0f);

        Quaternion rotationTorso = Quaternion.Euler(0.0f, rotY, 0.0f);
        transform.rotation = rotationTorso;
        transform.GetChild(0).rotation = rotationHead;
    }

}
