using UnityEngine;
using System.Collections;

public class FollowPath : MonoBehaviour {
	public EaseType easyType;
	private EnemyPhysics enemy;
	
	void Start(){
		enemy = GetComponent<EnemyPhysics>();
	}
	
	/// <summary>
	/// Starts the movement of the gameObject on the path 'Path'.
	/// </summary>
	/// <param name='path'>
	/// Path.
	/// </param>
	public void StartPath(string path) {
		iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(path),
											  "speed", enemy.speed,
											  "looptype", "pingPong",
											  iTweenX.easeType, iTweenX.Ease(easyType)));
	}
}







