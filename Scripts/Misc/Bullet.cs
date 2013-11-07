using UnityEngine;
using System.Collections;

public class Bullet : Weapon {
	public float lifetime = 1.0f;
	public string bulletTag;
	public GameObject deathParticles;
	
	void Start(){
		StartCoroutine(Life());
		//Destroy(transform.gameObject, lifetime);
	}
	
	IEnumerator Life(){
		yield return new WaitForSeconds(lifetime);
		Die(transform.position);
	}
	
	void Die(Vector3 position){
		Instantiate(deathParticles, position, Quaternion.identity);
		Destroy(transform.gameObject);
	}
	
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag != bulletTag){
			if(other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy"){
				Entity entity =  other.transform.GetComponent<Entity>();
				entity.TakeDamage(attackDamage);	
			}
			Debug.Log(other.gameObject.tag);
			Die(other.contacts[0].point);
		}	
	}
}
