using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {
	// ENTITY ATTRIBUTES
	public float maxHealth = 100;
	public float health = 100;
	public float maxEnergy = 50;
	public float energy = 50;
	public float speed = 10.0f;
	public float energyUseRate = 1.5f;
	public float energyRechargeRate = 0.01f;
	public float boostValue = 1.5f;
	public float attack = 100;
	public float armor = 50;
	public bool canBoost = true;
	public GameObject ragdoll;
	// PRIVATE VARIABLES
	private Ragdoll rag;
	
	void Update(){
		// Recharge energy constantly
		energy = (energy == maxEnergy) ? maxEnergy : (energy + energyRechargeRate);
		canBoost = (energy > energyUseRate) ? true : false;
	}
	
	// ENTITY FUNCTIONS
	/// <summary>
	/// Calculates the damage taken by the characters and removes it.
	/// Also checks if the character dies
	/// </summary>
	/// <param name='dmg'>
	/// Dmg.
	/// </param>
	public void TakeDamage(float attackDamage){
		float dmg = ((attackDamage - armor) > 0) ? (attackDamage - armor) : 1; 
		health -= dmg;
		
		if(health <= 0){
			Die();
		}
	}
	/// <summary>
	/// Uses the energy.
	/// </summary>
	/// <returns>
	/// If there was energy available and uses it
	/// </returns>
	public void UseEnergy(){
		energy -= energyUseRate;		
	}
	/// <summary>
	/// The entity dies and its Ragdoll is instantiated as the death animation.
	/// </summary>
	public void Die(){
		rag = (Instantiate(ragdoll, transform.position, transform.rotation) as GameObject).GetComponent<Ragdoll>();
		rag.CopyPose(transform);
		Destroy(gameObject);
	}
}
