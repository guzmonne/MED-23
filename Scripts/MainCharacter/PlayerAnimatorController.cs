using UnityEngine;
using System.Collections;

public class PlayerAnimatorController : MonoBehaviour {
	// Components
	private Animator animator;
	private PlayerControl playerControl;
	private State playerState;
	
	// Use this for initialization
	void Awake () {
		// Grab the Animator component
		animator = GetComponent<Animator>();
		// Grab the PlayerControl component
		playerControl = GetComponent<PlayerControl>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// Get the current character state
}
