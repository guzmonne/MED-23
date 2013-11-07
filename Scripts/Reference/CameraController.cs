using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	bool hasFocus;
	public float fixX;
	public float fixY;
	public Transform target;
	
	void FixedUpdate ()
	{	
		Vector3 pos = target.position;
		pos.x = fixX;
		pos.y = target.position.y + fixY;
		iTween.MoveUpdate(gameObject, pos, .8f);
	}
}