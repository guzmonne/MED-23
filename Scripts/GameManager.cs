using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	// GUI Skin
	public GUISkin customSkin;
	
	// Character Position
	private float deltaX;
	private float deltaY;
	// Game States
	private bool expensiveQualitySettings = true;
	private bool isPaused = false;
	private bool firstRun = false;
	private bool controlsGUI = false;
	private bool startGUI = false;
	private bool settingsGUI = false;
	private bool gameCompleted = false;
	public bool testing = false;
	// Characters
	public GameObject player;
	private GameObject activePlayer;
	// Camera
	public Camera cam;
	private GameCamera gameCamera;
	// Game Over
	[HideInInspector]
	public bool gameOver = false;
	
	// Start
	void Start () {
		gameCamera = cam.GetComponent<GameCamera>();
		if(testing){
			SpawnPlayer();
		} else {
			PauseGameMode();	
		}
		//SpawnPlayer();
	}
	
	// Update
	void Update () {
		if(activePlayer){
			deltaX = activePlayer.transform.position.x;
			deltaY = activePlayer.transform.position.y;
			if(deltaX > 8 && deltaY > 31){
				gameCompleted = true;
			}
			if(!activePlayer.activeSelf){
				gameCamera.SetTarget(transform.Find("Player_RagDoll(Clone)").transform);
			} else {
				gameCamera.SetTarget(activePlayer.transform);
			}
		}
		
		
		if(firstRun){
			PauseGameMode();
		}
		
		if(Input.GetKeyDown(KeyCode.Escape) && !firstRun && !gameOver && !gameCompleted){
			if(isPaused){
				ResumeGameMode();
			} else {
				PauseGameMode();
			}
		}
	}
	// OnGUI
	private void OnGUI(){
		GUI.skin = customSkin;
		
		if(startGUI)
			StartGameGUI();
		if(controlsGUI)
			ControlsGUI();
		if(isPaused)
			PauseGameGUI();
		if(settingsGUI)
			SettingsGUI();
		if(gameOver){
			PauseGameMode();
			GameOverGUI();
		}
		if(gameCompleted){
			PauseGameMode();
			GameCompletedGUI();
		}
	}
	
	public void GameOver(){
		gameOver = true;
	}
	
	private void SpawnPlayer(){ 
		activePlayer = (Instantiate(player, new Vector3(0,10,0), Quaternion.identity)) as GameObject;
		activePlayer.transform.parent = transform;
		gameCamera.SetTarget(activePlayer.transform);
		//gameCamera.SetTarget((Instantiate(player, new Vector3(0,10,0), Quaternion.identity) as GameObject).transform);
	}
	
		private void ResumeGameMode(){
		Time.timeScale = 1.0f;
		isPaused = false;
		Screen.showCursor = false;
	}
	
	private void PauseGameMode() {
		Time.timeScale = 0.0f;
		if(!firstRun && !gameOver && !gameCompleted)
			isPaused = true;
		Screen.showCursor = true;
	}
	
	private void StartGameGUI(){
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.Label(title, GUILayout.Width(200));
		GUILayout.Label(objective, GUILayout.Width(200));
		if(GUILayout.Button("Start Game", GUILayout.Width(200))){
			firstRun = false;
			startGUI = false;
			ResumeGameMode();
			SpawnPlayer();
		}
		if(GUILayout.Button("Game Controls", GUILayout.Width(200))){
			startGUI = false;
			controlsGUI = true;
		}
		if(GUILayout.Button("Settings", GUILayout.Width(200))){
			startGUI = false;
			settingsGUI = true;
		}
		if(GUILayout.Button("Exit", GUILayout.Width(200))){
			Application.Quit();
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndArea();
	}
	
	private void GameOverGUI(){
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.Label("			GAME OVER", GUILayout.Width(200));
		GUILayout.Label(hint, GUILayout.Width(200));
		if(GUILayout.Button("Retry", GUILayout.Width(200))){
			gameOver = false;
			ResumeGameMode();
			SpawnPlayer();
		}
		if(GUILayout.Button("Exit", GUILayout.Width(200))){
			Application.Quit();
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndArea();
	}
	
	private void GameCompletedGUI(){
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.Label(congratulations, GUILayout.Width(200));
		if(GUILayout.Button("New Game", GUILayout.Width(200))){
			Destroy(activePlayer);
			gameCompleted = false;
			ResumeGameMode();
			SpawnPlayer();
		}
		if(GUILayout.Button("Exit", GUILayout.Width(200))){
			Application.Quit();
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndArea();
	}
	
	private void ControlsGUI(){
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.Label(controls, GUILayout.Width(220));
		if(GUILayout.Button("Return", GUILayout.Width(220))){
			if(firstRun){
				startGUI = true;
				controlsGUI = false;
			} else {
				isPaused = true;
				controlsGUI = false;
			}
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndArea();
	}
	
	private void PauseGameGUI(){
		string message = "PAUSE";
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.Label(message, GUILayout.Width(200));
		if(GUILayout.Button("Continue", GUILayout.Width(200))){
			isPaused = false;
			ResumeGameMode();
		}
		if(GUILayout.Button("Settings", GUILayout.Width(200))){
			isPaused = false;
			settingsGUI = true;
		}
		if(GUILayout.Button("Exit", GUILayout.Width(200))){
			Application.Quit();
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndArea();
	}
	
	private void SettingsGUI(){
		string[] names = QualitySettings.names;
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.Label("			- SETTINGS -", GUILayout.Width(200));
		for(int i = 0; i < names.Length; i++){
			if(GUILayout.Button(names[i], GUILayout.Width(200)))
				QualitySettings.SetQualityLevel(i, expensiveQualitySettings);
		}
		if(GUILayout.Button("Return", GUILayout.Width(200))){
			if(firstRun){
				startGUI = true;
				settingsGUI = false;
			} else {
				isPaused = true;
				settingsGUI = false;
			}
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndArea();
	}
	
	private string title = @"
		    -*** GUX ***-
";
	
	private string objective = @"
			- OBJECTIVE -
Get to the flag at the top of the level.
";
	
	private string hint = @"
			  	- HINT -
Avoid the Saw Blades
";
	
	private string controls = @" 
		 - GAME CONTROLS -
Run left:				left arrow or a 
Run right:				right arrow or d 
Sprint:					shift + Run 
Jump:					space
Slide:					down key or s
Wall Jump:			Jump near a 
							Wall
";
	private string congratulations = @"
	- CONGRATULATIONS!!! -
You have found the missing flag! Would you like to play again?
";
}
