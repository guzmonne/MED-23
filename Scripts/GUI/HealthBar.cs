using UnityEngine;
using System.Collections;

public class HealthBar : Bar {
	public Transform player;
	[HideInInspector]
	public Entity entity;
	public float flashTime = 1.0f;
	public float timer = 0;
	private float savedHealth;
	public Texture2D barEmptyHit;
	public Texture2D barFullHit;
	private Texture2D auxEmpty;
	private Texture2D auxFull;
	
	void Start(){
		entity = player.GetComponent<Entity>();
		auxEmpty = barEmpty;
		auxFull = barFull;
		savedHealth = entity.health;
	}
	
	// This function should return a value between 0 and 1;
	public override float CalculateBarWidth(){
		float currentHealth = entity.health; 
		if(timer < 0){
			barEmpty = auxEmpty;
			barFull = auxFull;
			timer = 0;
		}
		if(currentHealth < savedHealth){
			barEmpty = barEmptyHit;
			barFull = barFullHit;
			savedHealth = currentHealth;
			timer = flashTime;
		}
		timer = (timer == 0) ? 0 : (timer - Time.deltaTime);
		// We calculate the porcentage of the remaining health
		return currentHealth / entity.maxHealth;
	}
}
