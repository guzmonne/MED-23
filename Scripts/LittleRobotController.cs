using UnityEngine;
using System.Collections;

public class LittleRobotController : MonoBehaviour {
	// Player Handling
	public float walkSpeed = 4;
	public float runSpeed = 8;
	public float acceleration = 30;
	public float jumpHeight = 10;
	public float slideDeceleration = 10;
	
	// System
	private float currentSpeed;
	private float targetSpeed;
	private float animationSpeed;
	private Vector2 amountToMove;
	private float moveDiretionZ;
	private float originalCapsuleHeight;
	private float originalCapsuleRadious;
	private float skin = 0.005f;
	private bool grounded = true;
	private bool movementStopped = true;
	
	// States
	private bool jumping;
	private bool sliding;
	
	// Components;
	private Animator animator;
	private CapsuleCollider collider;
	
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		collider = GetComponent<CapsuleCollider>();
		originalCapsuleHeight = collider.height;
		originalCapsuleRadious = collider.radius;
	}
	
	void Update () {
		
	}	
}
