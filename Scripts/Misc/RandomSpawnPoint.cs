using UnityEngine;
using System.Collections;

public class RandomSpawnPoint : MonoBehaviour {
	public Vector3[] posibleSpawnPoints;
	
	// Use this for initialization
	void Start () {
		int i = Random.Range(0, posibleSpawnPoints.Length);
		transform.position = posibleSpawnPoints[i];
	}
}
