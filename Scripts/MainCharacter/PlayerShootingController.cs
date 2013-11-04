using UnityEngine;
using System.Collections;

public class PlayerShootingController : MonoBehaviour {
	// Public Properties
	public int shootingSpeed = 10;
	public Rigidbody bullet;
	public Transform cannon;
	// Hidden Properties
	[HideInInspector]
	public bool isShooting = false;
	// Private Properties
	private int speedTimer = 0;
	private Vector3 velocity;
	private Rigidbody clone;
	// Components
	private MouseClickLocation mouseClickLocation;
	private PlayerControl playerControl;
	
	void Start(){
		// Get necessary components
		mouseClickLocation = GetComponent<MouseClickLocation>();
		playerControl = GetComponent<PlayerControl>(); 
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// If the mouse button is pressed we shoot
		if (Input.GetMouseButton(0)) {
			isShooting = true;
			// We increment the speedTimer until it matches the originalSpeed. It needs to be set here to make sure
			// that the hand animation has finished before we start shooting
			speedTimer = (speedTimer == shootingSpeed) ? shootingSpeed : (speedTimer + 1); 
			Shoot();
		} else {
			// We reset the timer
			isShooting = false;
			speedTimer = 0;
		}
	}
	
	private void Shoot(){
		// If the speedTimer equals the shootingSpeed then we shoot
		if(speedTimer == shootingSpeed){
			// The velocity of the bullet is given by the normalized targetPoint multiplied by a constant plus the velocity of the 
			// character in z
			velocity = mouseClickLocation.targetPoint.normalized * 80.0f + new Vector3(0, 0, Mathf.Abs(playerControl.currentSpeed));
			// We instatiate a buller clone in the position of the cannon and we rotate it according to the 
			// mouse angle tita got in the mouseClickLocation component. Also we need to take into acount the character direction
			if(playerControl.direction < 0){
				// We need to create the mirrored image of thin to -z	
				velocity = new Vector3(velocity.x, velocity.y, -velocity.z);
				//clone = (Rigidbody) Instantiate(bullet, pos, Quaternion.AngleAxis(360 - mouseClickLocation.tita, Vector3.left));
				clone = (Rigidbody) Instantiate(bullet, cannon.position, Quaternion.AngleAxis(360 - mouseClickLocation.tita, Vector3.left));
			} else {
				clone = (Rigidbody) Instantiate(bullet, cannon.position, Quaternion.AngleAxis(mouseClickLocation.tita, Vector3.left));
			}
			// We apply the velocity
			clone.velocity = velocity;
			// We reset the speedTimer
			speedTimer = 0;
		}
	}
}
