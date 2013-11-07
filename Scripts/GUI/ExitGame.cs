using UnityEngine;
using System.Collections;

public class ExitGame : MonoBehaviour {
	public Texture2D exit;
	public Texture2D exitHover;
	
	void OnMouseEnter(){
		guiTexture.texture = exitHover;
	}
	
	void OnMouseExit(){
		guiTexture.texture = exit;
	}
	
	void OnMouseDown(){
		Application.Quit();
	}
}
