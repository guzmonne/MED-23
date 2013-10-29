using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	// Public Character Properties
	public float speed = 10.0f;
	public float maxVelocityChange = 10.0f;
	public float gravity = 30.0f;
	public float jumpHeight = 4.0f;
	public float distanceToWall;
	public float distanceToGround;
	public float dirAndInput;
	// Private Character Properties
	private RaycastHit hit;
	private Vector3 velocity;
	private Vector3 c;
	private Vector3 originalCentre;
	private CapsuleCollider collider;
	private float originalHeight;
	private float originalRadius;
	private float colliderScale;
	private float h;
	private float r;
	private float skin = 0.07f;
	private float currentSpeed;
	public float xAxis;
	public float direction;
	// Components
	private Animator animator;
	// Player States
	public enum State {
		Standing,
		Running,
		Jumping,
		Sliding,
		Falling
	}
	public State playerState;
	
	void Awake() {
		// Set needed rigidbody properties
		//rigidbody.freezeRotation = true;
	    rigidbody.useGravity = false;
		// Grab the animator component
		animator = GetComponent<Animator>();
		// Grab the capsule collider and initialize its original values
		colliderScale = transform.localScale.z;
		collider = GetComponent<CapsuleCollider>();
		originalCentre = collider.center;
		originalHeight = collider.height;
		originalRadius = collider.radius;
		SetCollider(originalHeight, originalRadius, originalCentre);
		// Set the Character to the Falling state
		playerState = State.Falling;
	}
	
	void FixedUpdate() {
		// Grab the Player Horizontal movement
		xAxis = Input.GetAxis("Horizontal");
		switch (playerState){
		case State.Standing:
			Debug.Log("Standing");
			// Player is making the character run
			//if(direction != 0)	
			//	transform.eulerAngles = (direction < 0) ? Vector3.up * 180 : Vector3.zero;	
			//transform["Armature"].eulerAngles = (direction < 0) ? new Vector3(270, 180 ,0) : new Vector3(270, 0, 0);
			if( xAxis != 0){
				playerState = State.Running;
				break;
			}
			// Player has jumped
			if(Input.GetButton("Jump")){
				Jump();
				break;
			}
			// Set the Idle animation
			currentSpeed = 0;
			animator.SetFloat("Speed", Mathf.Abs(currentSpeed));
			break;
		case State.Running:
			Debug.Log("Running");
			if (xAxis != 0){
				direction = Mathf.Sign(xAxis);
				transform.eulerAngles = (direction < 0) ? Vector3.up * 180 : Vector3.zero;
			}
			// Check if Player has stoped the character
			if(xAxis == 0){
				Stand();
				break;
			}
			// Check if Player has jumped
			if(Input.GetButton("Jump")){
				Jump();
				break;
			}
			// Check if the Character it's not on the ground
			if(!Grounded()){
				playerState = State.Falling;
				break;
			}
			// Move the character horizontally
			CalculateHorizontalForce();
			// Set the Running animation
			animator.SetFloat("Speed", Mathf.Abs(currentSpeed));
			break;
		case State.Jumping:
			Debug.Log("Jumping");
			// Check if the Character has landed
			if(Grounded()){
				Stand();
				break;
			}
			// Check if the Character hits a wall
			if(CharacterHitsWall()){
				playerState = State.Falling;
				break;
			}
			CalculateHorizontalForce();
			// Set the Jumping animation
			animator.SetBool("isJumping", true);
			break;
		case State.Sliding:
			Debug.Log("Sliding");
			break;
		case State.Falling:
			Debug.Log("Falling");
			// Check if the Character has landed
			if(Grounded()){
				Stand();
				break;
			}
			// Set the Falling animation
			animator.SetBool("isFalling", true);
			break;
		}
		// We apply gravity manually for more tuning control
	    rigidbody.AddForce(new Vector3 (0, -gravity * rigidbody.mass, 0));
	}
//***********************************************************************************************//
//										MAIN FUNCTIONS
//***********************************************************************************************//	
	/// <summary>
	/// Calculates the horizontal force.
	/// </summary>
	private void CalculateHorizontalForce(){
        // Calculate how fast we should be moving
		// Check if the player is moving to the left or to the right
        Vector3 targetVelocity = new Vector3(0, 0, direction * xAxis);
        // Transforms direction from local space to world space. Then multiply by speed
		targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;
		// We grab the current rigidbody velocity
        velocity = rigidbody.velocity;
		// We save the velocity in z to the currentSpeed variable
		currentSpeed = rigidbody.velocity.z;
        // Apply a force that attempts to reach our target velocity
		Vector3 velocityChange = (targetVelocity - velocity);
        // Since the movement is in 2D we don't need to applied it to the 'x' axis
//		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
		// Add an instant velocity change to the rigidbody, ignoring its mass.
		rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
	}
	/// <summary>
	/// Calculates the vertical force.
	/// </summary>
	private void CalculateVerticalForce(){
		rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
	}
	/// <summary>
	/// Characters the hits wall.
	/// </summary>
	/// <returns>
	/// True if the character hits a wall.
	/// </returns>
	private bool CharacterHitsWall(){
		/*for (int i = 0; i < 2; i++){
			Vector3 rayDirection = transform.TransformDirection(Mathf.Sign(xAxis) * Vector3.forward);
			Vector3 p = transform.position;
			dirAndInput = ((direction > 0 && xAxis > 0) || (direction > 0 && xAxis < 0)) ? -1 : 1;
			float y = p.y + c.y + (i-1) * h/4;
			float z = p.z + dirAndInput * (r + skin); 
			Debug.DrawRay(new Vector3(p.x, y, z), rayDirection, Color.yellow);
			if (Physics.Raycast(transform.position + new Vector3(0, c.y + (i-1) * h/4, r + c.z + skin), transform.TransformDirection(Vector3.forward), out hit, 1.0F)){
	            	distanceToWall = hit.distance;
					if(distanceToWall < skin){
						playerState = State.Falling;
						return true;
				}
	    	}
		}
		return false; 
		*/
		for (int i = 0; i < 2; i++){
			Debug.DrawRay(new Vector3(p.x, y, z), rayDirection, Color.yellow);
			Debug.DrawRay(new Vector3(p.x, y, z), rayDirection, Color.green);
		}
	}
//***********************************************************************************************//
//										HELPER FUNCTIONS
//***********************************************************************************************//	
	/// <summary>
	/// Calculates the jump vertical speed.
	/// </summary>
	/// <returns>
	/// The jump vertical speed.
	/// </returns>
	private float CalculateJumpVerticalSpeed () {
	    // From the jump height and gravity we deduce the upwards speed 
	    // for the character to reach at the apex.
	    return Mathf.Sqrt(2 * jumpHeight * gravity);
	}
	/// <summary>
	/// Jump steps.
	/// </summary>
	private void Jump() {
		CalculateVerticalForce();
		playerState = State.Jumping;
		//grounded = false;
	}
	/// <summary>
	/// Stand steps.
	/// </summary>
	private void Stand() {
		playerState = State.Standing;
		// Set the Landing animation
		animator.SetBool("isJumping", false);
		animator.SetBool("isFalling", false);
	}
	/// <summary>
	/// Sets the collider.
	/// </summary>
	/// <param name='height'>
	/// Height.
	/// </param>
	/// <param name='radious'>
	/// Radius.
	/// </param>
	/// <param name='center'>
	/// Center.
	/// </param>
	private void SetCollider(float height, float radius, Vector3 center){
		collider.height = height;
		collider.radius = radius;
		collider.center = center;
		
		c = center * colliderScale;
		h = height * colliderScale;
		r = radius * colliderScale;
		/*
		c = center;
		h = height;
		r = radius;
		*/
	}
	/// <summary>
	/// Resets the collider.
	/// </summary>
	private void ResetCollider(){
		SetCollider(originalHeight, originalRadius, originalCentre);
	}
//***********************************************************************************************//
//									  COLLISIONS FUNCTIONS
//***********************************************************************************************//	
	/// <summary>
	/// Raises the collision stay event.
	/// </summary>
	/*void OnCollisionStay () {
	    // If the character touches the ground then he is grounded
		grounded = true;    
	}*/
	private bool Grounded(){
		for (int i = 0; i < 3; i++){
			Debug.DrawRay(transform.position + new Vector3(0, skin * colliderScale, -direction * (i - 1) * r), -transform.TransformDirection(Vector3.up), Color.red);	
			if (Physics.Raycast(transform.position + new Vector3(0, skin * colliderScale,  (i - 1) * r ) , -transform.TransformDirection(Vector3.up), out hit, 1)){
            	distanceToGround = hit.distance;
				if(distanceToGround < skin){
					return true;
				}
    		} 	
		}
		return false;
	}
}
