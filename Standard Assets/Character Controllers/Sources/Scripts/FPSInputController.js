private var motor : CharacterMotor;
public var animator : Animator;
public var speed : float;

private var isJumping : boolean;

// Use this for initialization
function Awake () {
	motor = GetComponent(CharacterMotor);
	animator = GetComponent(Animator);
}

// Update is called once per frame
function Update () {
	// Get the input vector from kayboard or analog stick
	var directionVector = new Vector3(0, 0, Input.GetAxis("Horizontal"));
	
	if (directionVector != Vector3.zero) {
		// Get the length of the directon vector and then normalize it
		// Dividing by the length is cheaper than normalizing when we already have the length anyway
		var directionLength = directionVector.magnitude;
		directionVector = directionVector / directionLength;
		
		// Make sure the length is no bigger than 1
		directionLength = Mathf.Min(1, directionLength);
		
		// Make the input vector more sensitive towards the extremes and less sensitive in the middle
		// This makes it easier to control slow speeds when using analog sticks
		directionLength = directionLength * directionLength;
		
		// Multiply the normalized direction vector by the modified length
		directionVector = directionVector * directionLength;
	}
	
	// Apply the direction to the CharacterMotor
	motor.inputMoveDirection = transform.rotation * directionVector;
	if(isJumping){
		animator.SetBool("isJumping", false);
	}
	if(Input.GetButton("Jump")){
		animator.SetBool("isJumping", true);
		isJumping = true;
	} 
		
	motor.inputJump = Input.GetButton("Jump");
	
	speed = motor.movement.velocity.z;
	animator.SetFloat("Speed", Mathf.Abs(speed));
	animator.SetBool("isGrounded", !motor.jumping.jumping);

	if(speed != 0){
			//transform.eulerAngles = (speed < 0) ? Vector3.up * 180: Vector3.zero;
	}
}

// Require a character controller to be attached to the same game object
@script RequireComponent (CharacterMotor)
@script AddComponentMenu ("Character/FPS Input Controller")
