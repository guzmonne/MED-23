using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour {
		// Public Properties
	public int shootingSpeed = 10;
	public int bulletSpeed = 40;
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
	private EnemyAI ai;
	private EnemyPhysics ePhysics;
	
	void Start(){
		// Get necessary components
		ai = GetComponent<EnemyAI>(); 
		ePhysics = GetComponent<EnemyPhysics>(); 
	}
	
	// Update is called once per frame
	void Update () {
		if (ai.onTarget && ePhysics.distanceToTarget < 15){
			isShooting = true;
			speedTimer = (speedTimer == shootingSpeed) ? shootingSpeed : (speedTimer + 1);
			Shoot();
		} else {
			isShooting = false;
			speedTimer = 0;
		}
	}
	
	private void Shoot(){
		// If the speedTimer equals the shootingSpeed then we shoot
		if(speedTimer == shootingSpeed){
			// The velocity of the bullet is given by the normalized target location multiplied by 80 and the body height of the character
			velocity = transform.forward * bulletSpeed;
			// We instatiate a bullet clone inside the Enemy cannon
			clone = (Rigidbody) Instantiate(bullet, cannon.position, transform.rotation);
			// We apply the velocity
			clone.velocity = velocity;
			// We reset the speedTimer
			speedTimer = 0;
		}
	}
}
