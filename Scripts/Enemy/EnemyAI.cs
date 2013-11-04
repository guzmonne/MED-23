using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	public Transform target;
	public string pathName;
	public float alfa = 30;
	public float beta = 30;
	public int rays = 5;
	public float l = 1;
	public float searchDelay = 1;
	public bool onTarget = false;
	public bool onPath = false;
	public bool searching = false;
	public bool foundCharacter = false;
	private FollowPath path;
	private EnemyPhysics enemy;
	//public bool targetFound = false; 
	
	void Start(){
		path = GetComponent<FollowPath>();
		enemy = GetComponent<EnemyPhysics>();
	}
	
	void Update(){
		CharacterSearch();
		// onTarget Debug
		if(onTarget){
			Debug.Log("I can see you!");
			//Debug.Log("I can see you Little Robot. You are at: (" + target.position.x.ToString() + ", " + target.transform.position.y.ToString() + ", " + target.transform.position.z.ToString() + ")" );
			if(onPath)
				iTween.Stop(transform.gameObject);
			enemy.MoveToTarget();
			onPath = false;
		} else {
			Debug.Log("Where are you!?");
			if (pathName != ""){
				if(!onPath){
					iTween.Stop(transform.gameObject);
					path.StartPath(pathName);
				}
				onPath = true;
			}
					
		}
	}
	
	private void CharacterSearch(){
		// If the rays count is zero we break the function to avoid zero division
		if(rays == 0)
			return;
		// We look for the character breaking one of the rays
		foundCharacter = false;
		for(int i = 0; i < rays; i++){
			// Calculate the ray
			Vector3 v = new Vector3(0 , Mathf.Sin(Mathf.Deg2Rad * (alfa + (beta / (rays - 1)) * i)), Mathf.Cos(Mathf.Deg2Rad * (alfa + (beta / (rays - 1)) * i))) * l;
			RaycastHit hit;
			if(enemy.facingBack)
				v = new Vector3(0, v.y, -v.z);
			Debug.DrawRay(transform.position, v, Color.green);
			if(Physics.Raycast(transform.position, v, out hit, l)){
				// If the player crosses the ray then we set it transform as the target else we start the searching command (unless he is already searching him)
				if(hit.transform.gameObject.tag == "Player"){
					Debug.Log("Hit!!!");
					target = hit.transform;
					foundCharacter = true;
					searching = false;
				} 
			}
		}
		if(onTarget && !foundCharacter){
			if(!searching){
				Debug.Log("StartSearchingTimer!");
				searching = true;
				Invoke("StopSearching", searchDelay);
			}
		}
		onTarget = (foundCharacter) ? true : onTarget;
	}
	
	private void StopSearching(){ 
		searching = false;
		onTarget = false;
	}
}
