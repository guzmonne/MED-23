using UnityEngine;
using System.Collections;

public class StartAndPauseScreen : MonoBehaviour {
	public GUITexture title;
	public GUITexture start;
	public GUITexture exit;
	public GUITexture pause;
	public GUITexture youWin;
	public GUITexture tryAgain;
	public GUITexture continueT;
	public bool isPaused = false;
	public bool gameStarted = false;
	public Transform go;
	
	void Start () {
		PauseGameMode();
		StartScreen();
		Screen.showCursor = true;
		isPaused = false;
	}
	
	void Update(){
		if(!gameStarted){
			if(!go)
				gameStarted = true;
		}
		if(Input.GetKeyDown(KeyCode.Escape) && gameStarted){
			if(isPaused){
				ResumeGameMode();
				Destroy(GameObject.Find("Metal_Pause(Clone)"));
				Destroy(GameObject.Find("Metal_Exit(Clone)"));
				Destroy(GameObject.Find("Metal_Continue(Clone)"));
			} else {
				PauseGameMode();
				PauseScreen();
			}
		}
	}
	
	public void YouWin(){
		PauseGameMode();
		Destroy(GameObject.Find("__GameManager(Clone)"));
		Instantiate(youWin);
		Instantiate(tryAgain, new Vector3(0.5f, 0.44f, 500), Quaternion.identity);
		Instantiate(exit, new Vector3(0.5f, 0.3f, 500), Quaternion.identity);
	}
	
	private void PauseGameMode() {
		Time.timeScale = 0.0f;
		isPaused = true;
	}
	
	private void ResumeGameMode() {
		Time.timeScale = 1.0f;
		isPaused = false;
	}
	
	private void PauseScreen(){
		Instantiate(pause);
		Instantiate(continueT, new Vector3(0.5f, 0.44f, 500), Quaternion.identity);
		Instantiate(exit, new Vector3(0.5f, 0.3f, 500), Quaternion.identity);
	}
	
	private void StartScreen(){
		Instantiate(title);
		Instantiate(start);
		Instantiate(exit);
	}
	
}
