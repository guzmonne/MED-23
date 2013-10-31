using UnityEngine;
using System.Collections;

public class BoostController : MonoBehaviour {
	public Transform movingParticles;
	public Transform staticParticles;
	
	private float y = 0;
	private float z = 0;
	private float tita = 0;
	private PlayerControl playerControl;
	private float currentSpeed;
	private Vector3 velocity;
	
	void Awake() {
		playerControl = GetComponent<PlayerControl>();
	}
	
	void Update() {
		velocity = transform.TransformDirection(rigidbody.velocity);
		y = velocity.y;
		z = velocity.z;
		tita = (z != 0) ? (Mathf.Atan(y/z) * Mathf.Rad2Deg) : tita;
		currentSpeed = playerControl.currentSpeed;
		if(Input.GetButton("Boost")){
			staticParticles.gameObject.SetActive(true);
			if (currentSpeed != 0){
				movingParticles.gameObject.SetActive(true);
				if (currentSpeed > 0)
					movingParticles.transform.eulerAngles = new Vector3( 180 - tita, 0, 0);
				if (currentSpeed < 0)
					movingParticles.transform.eulerAngles = new Vector3( tita, 0, 0);
			} else {
				movingParticles.gameObject.SetActive(false);
			}
		} else {
			staticParticles.gameObject.SetActive(false);
			movingParticles.gameObject.SetActive(false);
		}
	}
}
