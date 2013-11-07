using UnityEngine;
using System.Collections;

public class AnimateText : MonoBehaviour {
	public Vector3[] path;
	public float time;
	public float delay;
	public float startDelay = 1.0f;
	public EaseType easeType;
	private int n = 0;
	
	void Start(){
		//MoveToNextPoint();	
		StartCoroutine(Wait());
	}
	
	IEnumerator Wait(){
		yield return new WaitForSeconds(startDelay);
		MoveToNextPoint();
	}
	
	void MoveToNextPoint(){
		float d = (n == 0) ? 0 : delay;
		iTween.MoveTo(gameObject, iTween.Hash("position", path[n], "time", time/path.Length, "delay", d, "easetype", iTweenX.Ease(easeType), "oncomplete", "NextOrFinish"));
	}
	
	void NextOrFinish(){
		if ((n++) == path.Length - 1){
			Destroy(gameObject);
		} else {
			MoveToNextPoint();
		}
	}
}
