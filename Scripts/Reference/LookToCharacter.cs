using UnityEngine;
using System.Collections;

public class LookToCharacter : MonoBehaviour {
	public Transform target;
	
	void FixedUpdate ()
	{	
		Vector3 pos = target.position;
		iTween.LookTo(gameObject, target.position, 3f);
		//iTween.MoveUpdate(gameObject, pos, .8f);
	}
}
