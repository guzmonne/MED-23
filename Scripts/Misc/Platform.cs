using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour
{
	public float distance = 3;
	public float time = 3;
	public EaseType easeType;
	public enum Direction {
		x, y, z
	}
	public Direction dir;
	private string a;
	
	void Start (){
		switch(dir){
		case Direction.x:
			a = "x";
			distance = transform.position.x + distance;
			break;
		case Direction.y:
			a = "y";
			distance = transform.position.y + distance;
			break;
		case Direction.z:
			a = "z";
			distance = transform.position.z + distance;
			break;
		}
		
		iTween.MoveAdd(gameObject,iTween.Hash(a, distance,
											  "time", time,
											  "looptype", iTween.LoopType.pingPong,
											  "easetype", iTweenX.Ease(easeType),
											  "delay",Random.Range(0,.4f)));
	}
}
