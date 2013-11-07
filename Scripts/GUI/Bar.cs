using UnityEngine;
using System.Collections;

public class Bar : MonoBehaviour {
	public float barDisplay = 1;
	public Vector2 pos = new Vector2(20, 40);
	public Vector2 size = new Vector2(60, 20);
	public Texture2D barEmpty;
	public Texture2D barFull;
	public bool updateBar = true;
	public bool animateBar = false;
	public GUIStyle style;
	
	void OnGUI(){
		// Style used by the box
		GUI.skin.box = style;
		// Draw the background
		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
			GUI.Box (new Rect (0,0, size.x, size.y),barEmpty);
			if (updateBar){
			// Draw the filled in part:
				GUI.BeginGroup(new Rect (0, 0, size.x * barDisplay, size.y));
					GUI.Box(new Rect(0,0, size.x, size.y), barFull);
				GUI.EndGroup();
			}
		GUI.EndGroup();
	}
	
	void Update(){
		if(updateBar)
			barDisplay = CalculateBarWidth();
		if(animateBar)
			AnimateBar();
	}
	
	// This function should return a value between 0 and 1;
	public virtual float CalculateBarWidth(){
		return 100 / Random.Range(1, 100);
	}
	
	public virtual void AnimateBar(){}
}
