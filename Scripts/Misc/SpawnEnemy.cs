using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {
	public GameObject enemy;
	public Transform target;
	
	void Update(){
		if(!target)
			target = (Instantiate(enemy, transform.position, transform.rotation) as GameObject).transform;	
	}
}
