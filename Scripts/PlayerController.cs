using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : Entity {
	
	// Player Handling
	public float walkSpeed = 8;
	public float runSpeed = 12;
	public float acceleration = 30;
	public float gravity = 20;
	public float jumpHeight = 18;
	public float slideDeceleration = 10;
	
	// System
	private float currentSpeed;
	private float targetSpeed;
	private float animationSpeed;
	private Vector2 amountToMove;
	private float moveDiretionX;
	
	// States
	private bool jumping;
	private bool sliding;
	private bool wallHolding;
	
	// Components
	private PlayerPhysics playerPhysics;
	private Animator animator;
	
	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		// Character hits a wall
		if(playerPhysics.movementStopped){
			targetSpeed = 0;
			currentSpeed = 0;
		}
		
		// Character is on the ground
		if(playerPhysics.grounded || playerPhysics.roofed){
			amountToMove.y = 0;
			
			if(wallHolding){
				wallHolding = false;
				animator.SetBool("Wall-Hold", false);
				playerPhysics.ResetCollider();
			} 
			
			if(jumping && !playerPhysics.roofed){
				jumping = false;
				animator.SetBool("Jumping", false);
			}
			if(sliding){
				if (Mathf.Abs(currentSpeed) < 0.25f){
					sliding = false;
					animator.SetBool("Sliding", false);
					playerPhysics.ResetCollider();
				}
			}

			// Slide Input
			if(Input.GetButtonDown("Slide") && Mathf.Abs(currentSpeed) > 3){
				playerPhysics.SetCollider(new Vector3(0.15f, 0.05f, 0.05f), new Vector3(0, -0.050f, 0));
				sliding = true;
				animator.SetBool("Sliding", true);
				targetSpeed = 0;
			}
		} else {
			if(!wallHolding){
				if(playerPhysics.canWallHold){
					playerPhysics.SetCollider(new Vector3(0.1f, 0.14f, 0.05f), new Vector3(0.003f, 0.003f, 0));
					wallHolding = true;
					animator.SetBool("Wall-Hold", true);
				}
			} else {
				if(playerPhysics.closeToGround){
					wallHolding = false;
					animator.SetBool("Wall-Hold", false);
					playerPhysics.ResetCollider();
				}
			}
		}
		
		// Jump Input
		if(Input.GetButtonDown("Jump")){
			if((playerPhysics.grounded || wallHolding) && !sliding){
				amountToMove.y = jumpHeight;
				jumping = true;
				animator.SetBool("Jumping", true);	
				
				if(wallHolding){
					wallHolding = false;
					animator.SetBool("Wall-Hold", false);
					playerPhysics.ResetCollider();
				}
			}
		}
		
		animationSpeed = IncrementTowards(animationSpeed, Mathf.Abs(targetSpeed), acceleration);
		animator.SetFloat("Speed", animationSpeed);
		
		// Input
		moveDiretionX = Input.GetAxisRaw("Horizontal");
		if(!sliding){
			float speed = (Input.GetButton("Run")) ? runSpeed : walkSpeed;
			targetSpeed = moveDiretionX * speed;
			currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);	
			
			// Face Direction
			float moveDir = moveDiretionX;
			if(moveDir != 0 && !wallHolding){
				transform.eulerAngles = (moveDir > 0) ? Vector3.up * 180: Vector3.zero;
			}
		
		} else {
			currentSpeed = IncrementTowards(currentSpeed, targetSpeed, slideDeceleration);	

		}

		// Set amount to move
		amountToMove.x = currentSpeed;
		
		// Stop moving towards x
		if (wallHolding){
			amountToMove.x = 0;
			if (Input.GetAxisRaw("Vertical") != -1){
				amountToMove.y = amountToMove.y * 0.8f;
			}
		}
		
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move(amountToMove * Time.deltaTime, moveDiretionX);
	}
	
	// Increase a towards target by speed
	private float IncrementTowards(float n, float target, float a){
		if(n == target){
			return n;
		} else {
			float dir = Mathf.Sign(target - n);
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target - n)) ? n : target;
		}
	}
}
