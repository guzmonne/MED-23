using UnityEngine;
using System.Collections;

public class BoostController : MonoBehaviour {
	public Transform movingParticles;
	public Transform staticParticles;
	
	private float y = 0;
	private float z = 0;
	private float tita = 0;
	private PlayerControl playerControl;
	private Vector3 velocity;
	
	void Awake() {
		playerControl = GetComponent<PlayerControl>();
	}
	
	void Update() {
		velocity = transform.TransformDirection(rigidbody.velocity);
		y = velocity.y;
		z = velocity.z;
		tita = (z != 0) ? (Mathf.Atan(y/z) * Mathf.Rad2Deg) : tita;
		if(Input.GetButton("Boost") && playerControl.canBoost){
			playerControl.UseEnergy();
			playerControl.boostMultiplier = playerControl.boostValue;
			staticParticles.gameObject.SetActive(true);
			if (playerControl.currentSpeed != 0){
				movingParticles.gameObject.SetActive(true);
				if (playerControl.currentSpeed > 0)
					movingParticles.transform.eulerAngles = new Vector3( 180 - tita, 0, 0);
				else
					movingParticles.transform.eulerAngles = new Vector3( tita, 0, 0);
			} else {
				movingParticles.gameObject.SetActive(false);
			}
		} else {
			playerControl.boostMultiplier = 1.0f;
			staticParticles.gameObject.SetActive(false);
			movingParticles.gameObject.SetActive(false);
		}
	}
}
