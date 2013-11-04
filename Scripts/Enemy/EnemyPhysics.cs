using UnityEngine;
using System.Collections;

public class EnemyPhysics : MonoBehaviour {
	public bool facingBack = true;
	public float speed = 10;
	public float pointVelocity = 0;
	
	private FollowPath path;
	private EnemyAI ai;
	private bool init = true;
	private float fa = 0;
	
	void Start(){
		path = GetComponent<FollowPath>();	
		ai = GetComponent<EnemyAI>();
	}
	
	// Update is called once per frame
	void Update () {
		SetDirection();
	}
	
	void SetDirection(){
		if(!ai.onTarget){
			GetPointVelocity();
			if(pointVelocity > 0){
				facingBack = false;
				transform.eulerAngles = new Vector3(260, 180, 180);
			} else if (pointVelocity < 0){
				facingBack = true;
				transform.eulerAngles = new Vector3(290, 180, 0);
			}	
		}
	}
	
	public void MoveToTarget(){
		if(Vector3.Distance(transform.position, ai.target.position) > 8.0f){
			Debug.Log(Vector3.Distance(transform.position, ai.target.position).ToString());
			iTween.MoveUpdate(gameObject, iTween.Hash("position", ai.target.position, "time", CalculateTime(), "looktarget", ai.target));	
		}
	}
	
	private float CalculateTime(){
		if(ai.searching == true)
			return speed;
		else
			return speed/2;
	}
	
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
