using UnityEngine;
using System.Collections;

public class TryAgain : MonoBehaviour {
	public Texture2D tryAgain;
	public Texture2D tryAgainHover;
	public GameObject gameManager;
	
	void OnMouseEnter(){
		guiTexture.texture = tryAgainHover;
		Debug.Log("Mouse Enter");
	}
	
	void OnMouseExit(){
		guiTexture.texture = tryAgain;
		Debug.Log("Mouse Exit");
	}
	
	void OnMouseDown(){
		Debug.Log("Mouse Down");
		Destroy(GameObject.Find("Metal_You_Win(Clone)"));
		Destroy(GameObject.Find("Metal_Exit(Clone)"));
		Instantiate(gameManager);
		GameObject robot = GameObject.Find("Little Robot RigidBody");
		Camera.main.GetComponent<SmoothFollow>().target = robot.transform;
		Camera.main.GetComponent<SmoothLookAt>().target = robot.transform;
		Time.timeScale = 1.0f;
		Destroy(gameObject);
	}
}
