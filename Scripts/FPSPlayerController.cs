using UnityEngine;
using System.Collections;

public class FPSPlayerController : Entity {
	// Animator variables
	private float speed;
	
	// Components
	private Animator animator;
	//private CharacterMotor motor;
	private CharacterController controller;
	private CapsuleCollider pill;
	public GameObject bloodSplat;

	// Helper variables;
	public bool flipCharacter = true;
	private bool isGoingBack = true;
	private bool sliding;
	private Vector3 directionVector;
	private float yRotation;
	private float zRotation;
	
	// Capsule Variables
	private Vector3 pillCenter;
	private float pillRadious;
	private float pillHeight;
	
	/*void Awake(){

	}*/
	
	// Use this for initialization
	void Start () {
		Debug.Log(motor.movement.velocity.x.ToString());
		// Get the needed components
		animator = GetComponent<Animator>();
		//motor = GetComponent<CharacterMotor>();
		controller = GetComponent<CharacterController>();
		
		// Turn the character around if initialized in the other direction
		if(flipCharacter)	
			TurnAround();
		// Initialize pill location variables
		pillCenter = controller.center;
		pillRadious = controller.radius;
		pillHeight = controller.height;
	}
	
	// Update is called once per frame
	void Update () {
		// Don't let the player move in the x direction
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		// Get if the player is moving in the horizontal axis
		directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
		// Call the animator functions to enable the character movement
		AnimatorRunning();
		AnimatorJumping();
		AnimatorSliding();
				
		// Rotate the character acording to the platform angle and the character face direction
		FaceDirectionAndRotation();
	}
//***********************************************************************************************//
//										COLIDER FUNCTIONS
//***********************************************************************************************//	
	private void OnControllerColliderHit(ControllerColliderHit hit){
		if(hit.gameObject.name == "Circular Saw v1"){
			TakeDamage(10);
			Destroy(Instantiate(bloodSplat, hit.point, Quaternion.identity), 0.5f);
		}
	}
//***********************************************************************************************//
//								ORIENTATION AND ROTATION FUNCTIONS
//***********************************************************************************************//
	// This function changes the direction which the character is moving
	private void FaceDirectionAndRotation(){
		zRotation = (motor.movingPlatform.activePlatform == null) ? 0 : motor.movingPlatform.activePlatform.transform.eulerAngles.z;
		if(directionVector.x < 0){
			yRotation = 0;
			isGoingBack = true;
		} else if (directionVector.x > 0){
			yRotation = 180;
			zRotation = -zRotation;
			isGoingBack = false;
		} else {
			if(isGoingBack){
				yRotation = 0;
			} else {
				yRotation = 180;
				zRotation = -zRotation;
			}
		}
		zRotation = (motor.grounded) ? zRotation : 0;
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x,
											 yRotation,
											 zRotation);
	}
	
	private void TurnAround(){
		if(isGoingBack)
			transform.eulerAngles = Vector3.up * 180;
		isGoingBack = false;
	}
//***********************************************************************************************//
//										ANIMATOR FUNCTIONS
//***********************************************************************************************//
	private void AnimatorRunning(){
		// Set the Speed parameter for the movement animation
		speed = motor.movement.velocity.x;
		animator.SetFloat("Speed", Mathf.Abs(speed));
	}
	
	private void AnimatorJumping(){
		// Set the Jump parameter for the Jumping animation
		// Must be on the ground and can't be sliding
		if(motor.grounded == false && !sliding){
			animator.SetBool("isJumping", true);
		} else {
			animator.SetBool("isJumping", false);
		}
	}
	
	private void AnimatorSliding(){
		// Set the Jump parameter for the Sliding animation
		// Also we need to change the Character Controller parameters in order
		// to go under objects.
		if(sliding){
			if (Mathf.Abs(speed) < 0.25f){
				sliding = false;
				animator.SetBool("isSliding", false);
				ResetPills();
			}
		}
		if(Input.GetButtonDown("Slide")){
			sliding = true;
			animator.SetBool("isSliding", true);
			ResizePills(new Vector3(-0.010f, -0.035f, 0), 0.035f, 0);
		}
	}
//***********************************************************************************************//
//										HELPER FUNCTIONS
//***********************************************************************************************//
	// Set the radious, height and center of the controller and the pill collider
	private void ResizePills(Vector3 center, float radious, float height){
		controller.center = center;
		controller.radius = radious;
		controller.height = height;
	}
	// Reset the locations of the pill and the controller
	private void ResetPills(){
		ResizePills(pillCenter, pillRadious, pillHeight);
	}
}