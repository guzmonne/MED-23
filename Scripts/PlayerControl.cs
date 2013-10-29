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
	// Private Character Properties
	public bool grounded = false;
	private RaycastHit hit;
	public Vector3 velocity;
	private Vector3 c;
	private Vector3 originalCentre;
	private CapsuleCollider collider;
	private float originalHeight;
	private float originalRadius;
	private float colliderScale;
	private float h;
	private float r;
	private float skin = 0.07f;
	public float currentSpeed;
	private float xAxis;
	private float direction;
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
			// Check if Player is making the character run
			if( xAxis != 0){
				playerState = State.Running;
				break;
			}
			// Check if Player has jumped
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
			// Move the character horizontally
			CalculateHorizontalForce();
			// Set the Running animation
			animator.SetFloat("Speed", Mathf.Abs(currentSpeed));
			// Make the character turn depending on it's direction or check if the Player has stopped moving the character
			if (xAxis != 0){
				TurnAround();
			} else {
				Stand();
				break;
			}
			// Check if Player has jumped
			if(Input.GetButton("Jump")){
				Jump();
				break;
			}
			// Check if the Character it's not on the ground
			if(!grounded){
				playerState = State.Falling;
				break;
			}
			break;
		case State.Jumping:
			Debug.Log("Jumping");
			CalculateHorizontalForce();
			// Set the Jumping animation
			animator.SetBool("isJumping", true);
			// Check if the Player has changed the direction of the jump
			if (xAxis != 0)
				TurnAround();
			// Check if the Character has landed
			if(grounded){
				Stand();
				break;
			}
			break;
		case State.Sliding:
			Debug.Log("Sliding");
			break;
		case State.Falling:
			Debug.Log("Falling");
			CalculateHorizontalForce();
			// Set the Falling animation
			animator.SetBool("isFalling", true);
			if (xAxis != 0)
				TurnAround();
			// Check if the Character has landed
			if(grounded){
				Stand();
				break;
			}
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
		// Check if the Character hits a wall
		if(!CharacterHitsWall()){
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
		for (int i = 0; i < 3; i++){
			//float dir = (direction < 0) ? -r : r;
			Vector3 origin = transform.position + new Vector3(0, h/4 * (1+i), 0);
			Debug.DrawRay(origin, transform.TransformDirection(Vector3.forward) * 2 * r, Color.yellow);
			
			//Debug.DrawRay(transform.position + new Vector3(0, c.y + (i-1) * h/4, r + c.z + skin), transform.TransformDirection(Vector3.forward), Color.yellow);
			if (Physics.Raycast(origin, transform.TransformDirection(Vector3.forward), out hit, 2 * r)){
	            	distanceToWall = hit.distance;
					if(distanceToWall < r){
						playerState = State.Falling;
						return true;
				}
	    	}
		}
		return false;
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
		animator.SetBool("isFalling", false);
		animator.SetBool("isJumping", false);
	}
	/// <summary>
	/// Turns the Character around.
	/// </summary>
	private void TurnAround(){
		direction = Mathf.Sign(xAxis);
		transform.eulerAngles = (direction < 0) ? Vector3.up * 180 : Vector3.zero;
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
	
	void OnCollisionStay(Collision collisionInfo){
		switch (collisionInfo.gameObject.name) {
		case "Ground":
			if(TouchingVertical("Ground", "down")){
				grounded = true;
			}
			break;
		case "MovingPlatform":
			if(TouchingVertical("MovingPlatform", "down")){
				grounded = true;
				transform.parent = collisionInfo.transform;
			}
			break;
		} 
	}
	
	void OnCollisionEnter(Collision collisionInfo){
		switch (collisionInfo.gameObject.name) {
		case "Ground":
			if(TouchingVertical("Ground", "down")){
				grounded = true;
			}
			break;
		case "MovingPlatform":
			if(TouchingVertical("MovingPlatform", "down")){
				grounded = true;
				transform.parent = collisionInfo.transform;
			}
			break;
		}
	}
	
	void OnCollisionExit(Collision collisionInfo){
		switch (collisionInfo.gameObject.name) {
		case "Ground":
			grounded = false;
			break;
		case "MovingPlatform":
			grounded = false;
			transform.parent = null;
			break;
		} 
	}
	
	private bool TouchingSide(string platform) {
		for(int i = 0; i < 3; i++){
			Vector3 origin = transform.position + new Vector3(0, h/4 * (1+i), 0);
			if (Physics.Raycast(origin, transform.TransformDirection(Vector3.forward), out hit, skin)){
				if (hit.collider.gameObject.name == platform)
					return true;
			}
		}
		return false;
	}
	
	private bool TouchingVertical(string platform, string upOrDown, float raySize = 0) {
		float dir = (upOrDown == "up") ? 1 : -1;
		for (int i = 0; i < 2; i++){
			if (Physics.Raycast(transform.position + new Vector3(0, skin * colliderScale,  (i - 1) * r/2 ) , dir * transform.TransformDirection(Vector3.up), out hit, skin + raySize)){
				if (hit.collider.gameObject.name == platform)
					return true;
    		} 	
		}
		return false;
	}
	
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
