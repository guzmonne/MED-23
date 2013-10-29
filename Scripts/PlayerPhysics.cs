using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {
	public LayerMask collisionMask;
	
	private BoxCollider collider;
	
	private int collisionsDivisionsX = 3;
	private int collisionsDivisionsY = 10;
	
	private Vector3 s;
	private Vector3 c;
	private Vector3 originalSize;
	private Vector3 originalCentre;
	
	private float skin = 0.005f;
	private float colliderScale;
	[HideInInspector]
	public bool grounded = true;
	[HideInInspector]
	public bool roofed = true;
	[HideInInspector]
	public bool movementStopped = true;
	[HideInInspector]
	public bool canWallHold;
	[HideInInspector]
	public bool closeToGround;
	
	Ray ray;
	RaycastHit hit;
	
	private PlayerController playerController;
		
	// Use this for initialization
	void Start () {
		collider = GetComponent<BoxCollider>();
		colliderScale = transform.localScale.x;
		originalSize = collider.size;
		originalCentre = collider.center;
		SetCollider(originalSize, originalCentre);
		playerController = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void Move(Vector2 moveAmount, float moveDiretionX){
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		
		Vector2 p = transform.position;

		// Ceheck collisions left and right
		movementStopped = false;
		canWallHold = false;
		if(deltaX != 0){
			for(int i = 0; i < collisionsDivisionsY; i++){
				float dir = Mathf.Sign(deltaX);
				float x = p.x + c.x + s.x/2 * dir;
				float y = p.y + c.y - s.y/2 + s.y/(collisionsDivisionsY-1) * i;
				
				ray = new Ray(new Vector2(x,y), new Vector2(dir, 0));
				Debug.DrawRay(ray.origin, ray.direction);
				
				if(Physics.Raycast(ray, out hit, Mathf.Abs(deltaX) + 0.48f, collisionMask)){
					if(hit.collider.tag == "Wall Jump" && i > 4){
						if((Mathf.Sign(deltaX) == Mathf.Sign(moveDiretionX)) && moveDiretionX != 0){
							canWallHold = true;	
						}
					}
				}
				
				if(Physics.Raycast(ray, out hit, Mathf.Abs(deltaX) + skin, collisionMask)){
					
					// Get distance between player and ground
					float dst = Vector3.Distance(ray.origin, hit.point);
				
					// Stop player's downwards movement after coming within a skin width of a collider
					if(dst > skin){
						deltaX = dst * dir - skin * dir;
					} else {
						deltaX = 0;
					}
					movementStopped = true;
					break;
				}			
			}
		}
				
		// Ceheck collisions up and down
		grounded = false;
		roofed = false;
		closeToGround = false;
		for(int i = 0; i < collisionsDivisionsX; i++){
			float dir = Mathf.Sign(deltaY);
			float x = (p.x + c.x - s.x/2) + s.x/(collisionsDivisionsX-1) * i;
			float y = p.y + c.y + s.y/2 * dir;
			
			ray = new Ray(new Vector2(x,y), new Vector2(0, dir));
			Debug.DrawRay(ray.origin, ray.direction);
			
			if(Physics.Raycast(ray, out hit, Mathf.Abs(deltaY) + 1.5f, collisionMask)){
				float closenes = Vector3.Distance(ray.origin, hit.point);
				if(closenes < 1.5f && dir < 0) {
					closeToGround = true;
				}
			}
			
			if(Physics.Raycast(ray, out hit, Mathf.Abs(deltaY) + skin, collisionMask)){
				// Get distance between player and ground
				float dst = Vector3.Distance(ray.origin, hit.point);
				
				// Stop player's downwards movement after coming within a skin width of a collider
				if(dst > skin){
					deltaY = dst * dir - skin * dir;
				} else {
					deltaY = 0;
				}
				
				if(dir < 0){
					grounded = true;
				} else {
					roofed = true;
				}
				break;
			}
		}
		

		
		if(!grounded && !movementStopped && !roofed){
			Vector3 playerDir = new Vector3(deltaX, deltaY);
			Vector3 o = new Vector3(p.x + c.x + s.x/2 * Mathf.Sign(deltaX), p.y + c.y +s.y/2 * Mathf.Sign(deltaY));
			ray = new Ray(o, playerDir.normalized);
		
			if(Physics.Raycast(ray, Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY), collisionMask)){
				grounded = true;
				deltaY = 0;
			}	
		}
		
		Vector2 finalTransform = new Vector2(deltaX, deltaY);
		
		transform.Translate(finalTransform, Space.World);
	}
	
	public void SetCollider(Vector3 size, Vector3 center){
		collider.size = size;
		collider.center = center;
		
		s = size * colliderScale;
		c = center * colliderScale;
	}
	
	public void ResetCollider(){
		SetCollider(originalSize, originalCentre);
	}
}
