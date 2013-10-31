using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	// Public Character Properties
	public float speed = 10.0f;
	public float maxVelocityChange = 10.0f;
	public float gravity = 30.0f;
	public float jumpHeight = 4.0f;
	public float boostValue = 1.5f;
	// Public Hidden Variables
	[HideInInspector]
	public float currentSpeed;
	// Private Character Properties
	private bool grounded = false;
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
	private float xAxis;
	private float direction;
	private float boostMultiplier;
	private float distanceToWall;
	private float distanceToGround;
	private int framesTillAction = 20;
	private bool firstSlideFrame = true;
	// Components
	private Animator animator;
	// Player States
	public enum State {
		Standing,
		Running,
		Jumping,
		Sliding,
		Falling,
		Shooting
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
		// Check if the character is Grounded
		Grounded();
		// Set the boost multiplier if the character is pressing "Boost"
		boostMultiplier = (Input.GetButton("Boost")) ? boostValue : 1f;
		// Grab the Player Horizontal movement
		xAxis = Input.GetAxis("Horizontal");
		// Test for Player and Character events depending on the Character State
		switch (playerState){
		case State.Standing:
			Debug.Log("Standing");
			// Set the Idle animation
			currentSpeed = 0;
			animator.SetFloat("Speed", Mathf.Abs(currentSpeed));
			// Check if Player is making the character run
			if( xAxis != 0){
				playerState = State.Running;
				break;
			}
			// Check if the Player has jumped
			if(Input.GetButton("Jump") && framesTillAction > 20){
				Jump();
				break;
			}
			// Check if the player is shooting
			if(Input.GetButton("Fire")){
				playerState = State.Shooting;
				break;
			}
			break;
		case State.Running:
			Debug.Log("Running");
			// Check if the Character it's not on the ground
			if(!grounded){
				playerState = State.Falling;
				break;
			}
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
			if(Input.GetButton("Jump") && framesTillAction > 10){
				Jump();
				break;
			}
			if(Input.GetButton("Slide") && framesTillAction > 10){
				Slide();
				break;
			}
			// Check if the player is shooting
			if(Input.GetButton("Fire")){
				playerState = State.Shooting;
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
			CalculateSlideForce();
			break;
		case State.Shooting:
			Debug.Log("Shooting");
			// Check if the Character it's not on the ground
			if(!grounded){
				playerState = State.Falling;
				break;
			}
			//	Set the Shooting animation
			animator.SetBool("isShooting", true);
			animator.SetFloat("Speed", Mathf.Abs(currentSpeed));
			CalculateHorizontalForce();
			// Check if the Player has changed the direction of the jump
			if (xAxis != 0)
				TurnAround();
			// Check if Player has jumped
			if(Input.GetButton("Jump") && framesTillAction > 10){
				// Stop the shooting animation
				animator.SetBool("isShooting", false);
				Jump();
				break;
			}
			if(Input.GetButton("Slide") && framesTillAction > 10){
				animator.SetBool("isShooting", false);
				Slide();
				break;
			}
			// Check if the Player has stopped shooting
			if(!Input.GetButton("Fire")){
				Stand();
				break;
			}
			break;
		case State.Falling:
			Debug.Log("Falling");
			CalculateHorizontalForce();
			// Set the Falling animation
			animator.SetBool("isFalling", true);
			// Check if the Character has turned around
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
		// We count the frames till the next jump and we reset it if the character is not grounded
		if(framesTillAction < 50){
			framesTillAction++;
		}
		if(!grounded || Input.GetButton("Slide")){
			framesTillAction = 0;
		}
	}
//***********************************************************************************************//
//										MAIN FUNCTIONS
//***********************************************************************************************//	
	/// <summary>
	/// Calculates the horizontal force.
	/// </summary>
	private void CalculateHorizontalForce(bool constant = false){
		// Check if the Character hits a wall
		if(!CharacterHitsWall()){
			// Calculate how fast we should be moving
			// Check if the player is moving to the left or to the right
			float zTargetVelocity = (constant) ? 1 : (direction * xAxis);
	        Vector3 targetVelocity = new Vector3(0, 0, zTargetVelocity);
	        // Transforms direction from local space to world space. Then multiply by speed
			targetVelocity = transform.TransformDirection(targetVelocity);
	        targetVelocity *= (speed * boostMultiplier);
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
	/// Calculates the slide force.
	/// </summary>
	private void CalculateSlideForce() {
		// Give a little impulse when sliding only if it is the first frame of the slide
		if(firstSlideFrame){
			rigidbody.AddForce(Vector3.forward * Mathf.Sign(direction) * 10 * boostMultiplier, ForceMode.Impulse);
			firstSlideFrame = false;
		}
		// If the character is sliding under anything then the movement continues until he is out
		// Mainly to solve the problem of getting stuck under a low roof that you need to slide under
		// Also, we stand the character immediately after there is no more vertical contraints.
		if(TouchingVertical("any", "up", 1f)){
			CalculateHorizontalForce(true);
		} else {
			if(Mathf.Abs(rigidbody.velocity.z) < 3){
				firstSlideFrame = true; 
				Stand();
			}
		}
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
	    return Mathf.Sqrt(2 * jumpHeight * gravity * boostMultiplier);
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
		// Set the Standing animation
		animator.SetBool("isFalling", false);
		animator.SetBool("isJumping", false);
		animator.SetBool("isSliding", false);
		animator.SetBool("isShooting", false);
		ResetCollider();
	}
	private void Slide() {
		playerState = State.Sliding;
		// Change the size of the Capsule
		SetCollider(1.8f, 0.6f, new Vector3(0, 1.18f, -0.26f));
		// Set the Sliding animation
		animator.SetBool("isSliding", true);
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
		// We wait for 20 frames to pass before we reset the collider
		SetCollider(originalHeight, originalRadius, originalCentre);
	}
//***********************************************************************************************//
//									  COLLISIONS FUNCTIONS
//***********************************************************************************************//	
	
	/// <summary>
	/// Raises the collision stay event.
	/// </summary>
	/// <param name='collisionInfo'>
	/// Collision info.
	/// </param>
	void OnCollisionStay(Collision collisionInfo){
		switch (collisionInfo.gameObject.name) {
		case "MovingPlatform":
			transform.parent = collisionInfo.transform;
			break;
		} 
	}
	/// <summary>
	/// Raises the collision enter event.
	/// </summary>
	/// <param name='collisionInfo'>
	/// Collision info.
	/// </param>
	void OnCollisionEnter(Collision collisionInfo){
		switch (collisionInfo.gameObject.name) {
		case "MovingPlatform":
			transform.parent = collisionInfo.transform;
			break;
		}
	}
	/// <summary>
	/// Raises the collision exit event.
	/// </summary>
	/// <param name='collisionInfo'>
	/// Collision info.
	/// </param>
	void OnCollisionExit(Collision collisionInfo){
		switch (collisionInfo.gameObject.name) {
		case "MovingPlatform":;
			transform.parent = null;
			break;
		} 
	}
	/// <summary>
	/// Checks if the character touches something in front.
	/// </summary>
	/// <returns>
	/// True if he touched something
	/// </returns>
	/// <param name='platform'>
	/// If set to <c>true</c> platform.
	/// </param>
	private bool TouchingHorizontal(string platform) {
		for(int i = 0; i < 3; i++){
			Vector3 origin = transform.position + new Vector3(0, h/4 * (1+i), 0);
			if (Physics.Raycast(origin, transform.TransformDirection(Vector3.forward), out hit, skin)){
				if (hit.collider.gameObject.name == platform)
					return true;
			}
		}
		return false;
	}
	/// <summary>
	/// Checks if the character touches something on top or below him.
	/// </summary>
	/// <returns>
	/// True if he touched something
	/// </returns>
	/// <param name='platform'>
	/// If set to <c>true</c> platform.
	/// </param>
	/// <param name='upOrDown'>
	/// If set to <c>true</c> up or down.
	/// </param>
	/// <param name='raySize'>
	/// If set to <c>true</c> ray size.
	/// </param>
	private bool TouchingVertical(string platform, string upOrDown, float raySize = 0) {
		float dir = (upOrDown == "up") ? 1 : -1;
		for (int i = 0; i < 2; i++){
			Debug.DrawRay(transform.position + new Vector3(0, h,  (i - 1) * r/2 ) , (skin + raySize) * dir * transform.TransformDirection(Vector3.up), Color.green);	
			if (Physics.Raycast(transform.position + new Vector3(0, h,  (i - 1) * r/2 ) , dir * transform.TransformDirection(Vector3.up), out hit, skin + raySize)){
				if (platform == "any" || hit.collider.gameObject.name == platform)
					return true;
    		} 	
		}
		return false;
	}
	/// <summary>
	/// Check if the character is Grounded
	/// </summary>
	private void Grounded(){
		bool setGrounded = false;
		for (float i = 0; i < 7; i++){
			Debug.DrawRay(transform.position + new Vector3(0, skin * colliderScale, (i/3 - 1) * r), -transform.TransformDirection(Vector3.up), Color.blue);	
			if (Physics.Raycast(transform.position + new Vector3(0, skin * colliderScale,  (i/3 - 1) * r ) , -transform.TransformDirection(Vector3.up), out hit, 1F)){
            	distanceToGround = hit.distance;
				if(distanceToGround < skin){
					//grounded = true;
					setGrounded = true;
					break;
				}
    		} 	
		}
		grounded = (setGrounded) ? true : false;
	}
}
