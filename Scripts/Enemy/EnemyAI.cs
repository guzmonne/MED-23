using UnityEngine;
using System.Collections;

public class EnemyAI : Entity {
	// Public Variables
	public Transform target;
	public string pathName;
	public float alfa = 30;
	public float beta = 30;
	public int rays = 10;
	public float l = 10;
	public bool onTarget = false;
	public bool onPath = false;
	public bool searching = false;
	// Private Variables
	private float searchDelay = 3;
	private float searchTimer = 0;
	private bool foundCharacter = false;
	// Private Components
	private FollowPath path;
	private EnemyPhysics enemy;
	
	void Start(){
		// Get Components
		path = GetComponent<FollowPath>();
		enemy = GetComponent<EnemyPhysics>();
	}
	
	void Update(){
		CharacterSearch();
		// onTarget Debug
		if(onTarget){
			// We check to see if the enemy was on its predefined path and we cancel its movement and change the "onPath" value
			if(onPath){
				onPath = false;
				iTween.Stop(transform.gameObject);
			}
			// We move the enemy to the target's position
			enemy.MoveToTarget();
		} else {
			// If there is no pathname defined we just set the variable onPath
			if (pathName != ""){
				// If the enemy was not on its predefined path then we stop the other tweens and put him on his path
				if(!onPath){
					iTween.Stop(transform.gameObject);
					path.StartPath(pathName);
				}
				onPath = true;
			}
					
		}
	}
	
	/// <summary>
	/// Looks for the character using a bunch of rays cast from the center of the enemy and sets it as the target
	/// </summary>
	private void CharacterSearch(){
		// If the rays count is zero we break the function to avoid zero division
		if(rays == 0)
			return;
		// We look for the character breaking one of the rays. So, first we reset the Found Character value
		foundCharacter = false;
		for(int i = 0; i < rays; i++){
			// Calculate the ray's direction as the foward vector rotated alfa degrees + beta/rays
			Vector3 v = Quaternion.AngleAxis(alfa + (beta/rays-1)*i, transform.right) * transform.forward * l;		
			RaycastHit hit;
			Debug.DrawRay(transform.position, v, Color.green);
			if(Physics.Raycast(transform.position, v, out hit, l)){
				// If the player crosses the ray then we set it transform as the target
				if(hit.transform.gameObject.tag == "Player"){
					target = hit.transform;
					foundCharacter = true;
					searching = false;
				} 
			}
		}
		// If the Character was not found then we keep the enemy alert for 'searchDelay' seconds before he returns to its path
		if(onTarget && !foundCharacter){
			if(searchTimer > searchDelay){
				searching = false;
				onTarget = false;
			} else {
				searchTimer += Time.deltaTime;
				searching = true;
			}
		} else {
			searchTimer = 0;
			searching = false;
		}
		// We set the onTarget value to true if it was found
		onTarget = (foundCharacter) ? true : onTarget;
	}
	
	void OnCollisionEnter(Collision other){
		if(other.transform && other.gameObject.tag == "Bullet"){
			target = GameObject.FindGameObjectWithTag("Player").transform;
			onTarget = true;
		}
	}
}
