using UnityEngine;
using System.Collections;

public class BoostController : MonoBehaviour {
	public Transform movingParticles;
	public Transform staticParticles;
	
	private PlayerControl playerControl;
	
	void Awake() {
		playerControl = GetComponent<PlayerControl>();
	}
	
	
	void Update() {
		if(Input.GetButton("Boost")){
			staticParticles.gameObject.SetActive(true);
			if(playerControl.currentSpeed != 0){
				Debug.Log("Current Speed is:" + playerControl.currentSpeed.ToString() + ". So, set the throters!");
				movingParticles.gameObject.SetActive(true);
			}
		} else {
			staticParticles.gameObject.SetActive(false);
			movingParticles.gameObject.SetActive(false);
		}
	}
}
