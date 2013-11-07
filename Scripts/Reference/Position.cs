using UnityEngine;
using System.Collections;

public class Position : MonoBehaviour {
	public Vector3 position;
	// Update is called once per frame
	void Update () {
		position = transform.position;
	}
}
