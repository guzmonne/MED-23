using UnityEngine;
using System.Collections;

public class Autodestroy : MonoBehaviour {
	public float lifetime = 1.0f;
	public GameObject deathParticles;
	
	void Start(){
		StartCoroutine(Life());
		//Destroy(transform.gameObject, lifetime);
	}
	
	IEnumerator Life(){
		yield return new WaitForSeconds(lifetime);
		Die();
	}
	
	void Die(){
		Destroy(transform.gameObject);
	}
	
	void OnCollisionEnter(Collision collisionInfo){
		if(collisionInfo.collider.tag != "Player"){
			Instantiate(deathParticles, collisionInfo.contacts[0].point, Quaternion.identity);
			Die();	
		}
	}
}
