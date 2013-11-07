using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
	public Texture2D start;
	public Texture2D startHover;
	public GameObject gameManager;
	
	void OnMouseEnter(){
		guiTexture.texture = startHover;
	}
	
	void OnMouseExit(){
		guiTexture.texture = start;
	}
	
	void OnMouseDown(){
		Instantiate(gameManager);
		Destroy(GameObject.Find("Metal_Med-23(Clone)"));
		Destroy(GameObject.Find("Metal_Exit(Clone)"));
		Time.timeScale = 1.0f;
		GameObject robot = GameObject.Find("Little Robot RigidBody");
		Camera.main.GetComponent<SmoothFollow>().target = robot.transform;
		Camera.main.GetComponent<SmoothLookAt>().target = robot.transform;
		Destroy(gameObject);
	}
}
