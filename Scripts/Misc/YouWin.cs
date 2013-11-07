using UnityEngine;
using System.Collections;

public class YouWin : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player")
			GameObject.Find("Main Camera").GetComponent<StartAndPauseScreen>().YouWin();
	}
}
