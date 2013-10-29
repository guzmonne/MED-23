using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {
	private GameObject currentLevel;	
	
	// Use this for initialization
	void Start () {
		currentLevel = MyLoadLevel("Scene1");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public GameObject MyLoadLevel(string aLevelName){
    	Application.LoadLevelAdditive(aLevelName);
    	return GameObject.Find(aLevelName);
	}
}
