using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour {
	public float time;
	private float timer;
	public Transform player;
	public GameObject playerPrefab;
	
	void Update () {
		if(!player){
			if(timer > time){
				player = (Instantiate(playerPrefab, transform.position, transform.rotation) as GameObject).transform;
				Camera.main.GetComponent<SmoothFollow>().target = player;
				Camera.main.GetComponent<SmoothLookAt>().target = player;
				transform.parent.GetComponent<HealthBar>().player = player;
				transform.parent.GetComponent<HealthBar>().entity = player.GetComponent<Entity>();
				transform.parent.GetComponent<EnergyBar>().player = player;
				transform.parent.GetComponent<EnergyBar>().entity = player.GetComponent<Entity>();
				player.GetComponent<PlayerControl>().father = transform.parent;
				player.parent = transform.parent;
				timer = 0;
			} else {
				timer += Time.deltaTime;	
			}
		}
				
	}
}
