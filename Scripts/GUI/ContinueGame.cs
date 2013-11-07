using UnityEngine;
using System.Collections;

public class ContinueGame : MonoBehaviour {
	public Texture2D continueT;
	public Texture2D continueHover;
	
	void Start(){
		Debug.Log("Continue");
		Screen.showCursor = true;
	}
	
	void OnMouseEnter(){
		guiTexture.texture = continueHover;
		Debug.Log("Mouse Enter");
	}
	
	void OnMouseExit(){
		guiTexture.texture = continueT;
		Debug.Log("Mouse Exit");
	}
	
	void OnMouseDown(){
		GameObject.Find("Main Camera").GetComponent<StartAndPauseScreen>().isPaused = false;
		Destroy(GameObject.Find("Metal_Pause(Clone)"));
		Destroy(GameObject.Find("Metal_Exit(Clone)"));
		Time.timeScale = 1.0f;
		Destroy(gameObject);
	}
}
