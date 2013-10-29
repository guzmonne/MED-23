using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {
	// PUBLIC VARIABLES
	public float health;
	public float blobTime = 0.3f;
	public float forceMultiplier = 1000;
	public GameObject ragdoll;
	// PRIVATE VARIABLES
	private GameObject ragdollInstance;
	public CharacterMotor motor;
	private Ragdoll r;
//***********************************************************************************************//
//										     AWAKE
//***********************************************************************************************//	
	void Awake(){
		motor = GetComponent<CharacterMotor>();
	}
//***********************************************************************************************//
//										ENTITY FUNCTIONS
//***********************************************************************************************//
	public void TakeDamage(float dmg){
		health -= dmg;
		
		if(health == 0){
			Die();
		} else {
			Blob();
		}
	}
	
	public void Die(){
		r = (Instantiate(ragdoll, transform.position, transform.rotation) as GameObject).GetComponent<Ragdoll>();
		r.CopyPose(transform);
		Destroy(this.gameObject);
	}
//***********************************************************************************************//
//										RAGDOLL FUNCTIONS
//***********************************************************************************************//
	public void Blob(){
		float x = motor.movement.velocity.x;
		float y = motor.movement.velocity.y;
		
		ragdollInstance = Instantiate(ragdoll, transform.position, transform.rotation) as GameObject;
		ragdollInstance.transform.parent = transform.parent.transform;
		r = ragdollInstance.GetComponent<Ragdoll>();
		r.CopyPose(transform);
		gameObject.SetActive(false);
		ragdollInstance.rigidbody.AddForce(new Vector3(-x, -y, 0) * forceMultiplier);
		Invoke("DeBlob", blobTime);
	}
	
	private void DeBlob(){
		transform.position = new Vector3(ragdollInstance.transform.position.x,
														 ragdollInstance.transform.position.y,
														 0);
		Destroy(ragdollInstance);
		gameObject.SetActive(true);
	}
}
