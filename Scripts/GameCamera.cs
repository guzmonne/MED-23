using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	private Transform target;
	private float trackSpeed = 12;
	
	public void SetTarget(Transform t){
		target = t;
	}
	
	// Similar to update but works after all the other updates
	void LateUpdate(){
		if(target){
			float y;
			float x = IncrementTowards(transform.position.x, target.position.x, trackSpeed);
			if(target.position.y < 6.660766f){
				y = 6.660766f;
			} else {
				y = IncrementTowards(transform.position.y, target.position.y, trackSpeed);
			}
			transform.position = new Vector3(x, y, transform.position.z);
		}
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
