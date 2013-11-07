using UnityEngine;
using System.Collections;

public class EnergyBar : Bar {
	public Transform player;
	public Entity entity;
	public Texture2D smoke1;
	public Texture2D smoke2;
	public Texture2D smoke3;
	private Texture2D aux;
	private int count = 0;
	
	void Start(){
		entity = player.GetComponent<Entity>();
		aux = barEmpty;
		animateBar = true;
	}
	
	// This function should return a value between 0 and 1;
	public override float CalculateBarWidth(){
		// We calculate the porcentage of the remaining health
		return (entity.energy) / entity.maxEnergy;
	}
	
	public override void AnimateBar(){
		if(Input.GetButton("Boost") && entity.canBoost){
			switch(count){
			case 0:
				barEmpty = smoke1;
				break;
			case 3:
				barEmpty = smoke2;
				break;
			case 6:
				barEmpty = smoke3;
				break;
			}
			count = (count == 9) ? 0 : (count + 1);	
		} else {
			barEmpty = aux;
		}
	}
}