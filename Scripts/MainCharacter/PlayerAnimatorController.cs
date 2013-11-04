using UnityEngine;
using System.Collections;

public class PlayerAnimatorController : MonoBehaviour {
	
	// Components
	private Animator animator;
	private PlayerControl playerControl;
	private State playerState;
	private PlayerShootingController playerShootingController;
	private MouseClickLocation mouseClickLocation;
	
	// Use this for initialization
	void Awake () {
		// Grab the Animator component
		animator = GetComponent<Animator>();
		// Grab the PlayerControl component
		playerControl = GetComponent<PlayerControl>();
		// Grab the PlayerShootingController component
		playerShootingController = GetComponent<PlayerShootingController>();
		// Grab the MouseClickLocation component
		mouseClickLocation = GetComponent<MouseClickLocation>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// Get the current character state
		playerState = playerControl.playerState;
		switch (playerState){
		case State.Standing:
			// Set the Idle animation
			animator.SetBool("isFalling", false);
			animator.SetBool("isJumping", false);
			animator.SetBool("isSliding", false);
			animator.SetFloat("Speed", Mathf.Abs(playerControl.currentSpeed));
			break;
		case State.Running:
			// Set the Running animation
			animator.SetFloat("Speed", Mathf.Abs(playerControl.currentSpeed));
			break;
		case State.Jumping:
			// Set the Jumping animation
			animator.SetBool("isJumping", true);
			break;
		case State.Sliding:
			// Set the Sliding animation
			animator.SetBool("isSliding", true);
			break;
		case State.Falling:
			// Set the Falling animation
			animator.SetBool("isFalling", true);
			break;
		}
		if(playerShootingController.isShooting){
			animator.SetLayerWeight(1,1f);
			animator.SetFloat("ShootingAngle", mouseClickLocation.tita);
		} else {
			animator.SetLayerWeight(1,0);
		}
	}
}
