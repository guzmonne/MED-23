using UnityEngine;
using System.Collections;
 
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
 
public class CharacterControls : MonoBehaviour {
 
	public float speed = 10.0f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public bool canJump = true;
	public float jumpHeight = 2.0f;
	public bool grounded = false;
	public float direction;
	
	private Animator animator;
	private float motorSpeed; 
	public bool isJumping = false;
	private bool turnedAround = false;
 
	void Awake () {
	    animator = GetComponent<Animator>();
		rigidbody.freezeRotation = true;
	    rigidbody.useGravity = false;
	}
	
	void Update() {
		animator.SetBool("isGrounded", grounded);
	}
 
	void FixedUpdate () {
		//animator.SetBool("isGrounded", grounded);
	    //if (grounded) {
	        // Calculate how fast we should be moving
			// Check if the player is moving to the left or to the right
	        Vector3 targetVelocity = new Vector3(0, 0, Input.GetAxis("Horizontal"));
	        targetVelocity = transform.TransformDirection(targetVelocity);
	        targetVelocity *= speed;
 
	        // Apply a force that attempts to reach our target velocity
	        Vector3 velocity = rigidbody.velocity;
	        Vector3 velocityChange = (targetVelocity - velocity);
	        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
	        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
	        velocityChange.y = 0;
	        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
 
	        // Jump
	        if (canJump && Input.GetButton("Jump")) {
	            rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
				animator.SetBool("isJumping", true);
				isJumping = true;
	        }
	    //}
 
	    // We apply gravity manually for more tuning control
	    rigidbody.AddForce(new Vector3 (0, -gravity * rigidbody.mass, 0));
		
		if(isJumping && grounded){
			animator.SetBool("isJumping", false);
			isJumping = false;
			Debug.Log("I Should be grounded");
		}
		
		if(isJumping) 
			grounded = false;
		/*
		if(Mathf.Sign(direction * rigidbody.velocity.z) == -1 ){
			direction *= -1;
			//transform.Rotate(Vector3.up * 180);
		}
		
		if(rigidbody.velocity.z != 0){
			transform.eulerAngles = (rigidbody.velocity.z < 0) ? Vector3.up * 180: Vector3.zero;
		}
		*/
		
		direction = rigidbody.velocity.z;
		
		motorSpeed = rigidbody.velocity.z;
		animator.SetFloat("Speed", Mathf.Abs(motorSpeed));		
	}
 
	void OnCollisionStay () {
	    
		grounded = true;    
	}
 
	float CalculateJumpVerticalSpeed () {
	    // From the jump height and gravity we deduce the upwards speed 
	    // for the character to reach at the apex.
	    return Mathf.Sqrt(2 * jumpHeight * gravity);
	}
}