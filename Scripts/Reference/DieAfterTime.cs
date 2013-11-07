using UnityEngine;
using System.Collections;

public class DieAfterTime : MonoBehaviour {
	public float lifetime = 0.5f;
	
	void Start(){
		Destroy(gameObject, lifetime);
	}
}
