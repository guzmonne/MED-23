using UnityEngine;
using System.Collections;

public class EnemyPhysics : MonoBehaviour {
	// Public Variables
	public bool facingBack = true;
	public float speed = 10;
	public float pointVelocity = 0;
	// Private Variables
	private FollowPath path;
	private EnemyAI ai;
	private bool init = true;
	private float fa = 0;
	private Vector3 direction;
	private float distanceToTarget = 0;
	
	void Start(){
		path = GetComponent<FollowPath>();	
		ai = GetComponent<EnemyAI>();
	}
	
	// Update is called once per frame
	void Update () {
		// We call the SetDirection function so the enemy is looking at the correct direction through its path
		SetDirection();
	}
	
	/// <summary>
	/// Sets the direction of the enemy.
	/// </summary>
	void SetDirection(){
		// First we calculate the point velocity in this frame
		GetPointVelocity();
		// Depending on the value of the point velocity we turn the characters rotation
		if(pointVelocity > 0){
			facingBack = false;
			// We only rotate the enemy if it's moving along its path
			if(!ai.onTarget)
				transform.eulerAngles = new Vector3(30, 0, 0);
		} else if (pointVelocity < 0){
			facingBack = true;
			if(!ai.onTarget)
				transform.eulerAngles = new Vector3(30, 180, 0);
		}	
	}
	
	/// <summary>
	/// Moves the enemy to the target.
	/// </summary>
	public void MoveToTarget(){
		// We set the direction to look at at the head of the player
		direction = ai.target.position + new Vector3(0, 2.3f, 0);
		distanceToTarget = Vector3.Distance(transform.position, direction);
		// We separate the look and move tweens to make sure that the enemy doesn't crach into the Character but hovers over it.
		iTween.LookUpdate(gameObject, iTween.Hash("looktarget", direction, "time", CalculateTime()));
		if( distanceToTarget > 5.0f){
			iTween.MoveUpdate(gameObject, iTween.Hash("position", direction , "time", CalculateTime()));
		}
	}
	
	/// <summary>
	/// Calculates the time for the tweens animations based on the enemy speed.
	/// </summary>
	/// <returns>
	/// The time.
	/// </returns>
	private float CalculateTime(){
		if(ai.searching == true)
			return speed * 2;
		else
			return speed;
	}
	
	/// <summary>
	/// Gets the point velocity.
	/// </summary>
	/// <returns>
	/// The point velocity.
	/// </returns>
	public float GetPointVelocity(){
		if(init){
			fa = transform.position.z;
			init = false;
			return 0;
		}
		//Derivative definition
		// v = (f(a + h) + f(a))/h
		// f(a + h) = transform.position.z
		// f(a) = fa
		// h = Time.deltaTime
		pointVelocity = (transform.position.z - fa)/Time.deltaTime;
		fa = transform.position.z;
		return pointVelocity;
	}
}
