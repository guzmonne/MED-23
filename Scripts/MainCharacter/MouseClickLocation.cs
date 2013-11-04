using UnityEngine;
using System.Collections;

public class MouseClickLocation : MonoBehaviour
{
	// Public Variables
	public float tita; 				// Angle from position to 
	public Vector3 targetPoint;		// Point in Global Coordinates where the Player clicked
 
	// Private Variables
	private PlayerControl playerControl;
	
	void Awake() {
		playerControl = GetComponent<PlayerControl>();
	}
	
	void FixedUpdate ()
	{
		if (Input.GetMouseButton(0)) {
			// Generate a plane that passes through the origin with it's normal towards -x.
			// For this to work the player should be positioned with x close to zero
	 		//Plane playerPlane = new Plane(transform.position, transform.position + new Vector3(0, 0, 1), transform.position + new Vector3(0, 1, 0));
			Plane playerPlane = new Plane(Vector3.right, transform.position);
			// Generate a ray from the cursor position
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
	 		// Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
			// Determine the point where the cursor ray intersects the plane.
			// Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
			// then find the point along that ray that meets that distance.  This will be the point
			// where the player clicked.
			float hitdist = 0.0f;
			// If the ray is parallel to the plane, Raycast will return false.
			// For a 2D game with the plane we selected this should be unnecessary.
			if (playerPlane.Raycast (ray, out hitdist)) {
				// Get the point along the ray that hits the calculated distance.
				targetPoint = ray.GetPoint(hitdist) - new Vector3(transform.position.x, transform.position.y + playerControl.h / 2, transform.position.z);
				// If the target has turned around we need to mirror the target point vector in y
				if(playerControl.direction < 0)
					targetPoint = new Vector3(targetPoint.x, targetPoint.y, -targetPoint.z);
				// Now we calculate the angle formed by this vector using Mathf Atan y/z.
				// So we need to be sure to handle the extremes targetPoint.y == 0 and targetPoint.z == 0
				if(targetPoint.z == 0)
					tita = (targetPoint.y > 0) ? 90 : 270;
				else {
					if (targetPoint.z < 0)
						tita = (Mathf.Atan(targetPoint.y/targetPoint.z)) * Mathf.Rad2Deg + 180;
					else {
						if (targetPoint.y < 0)
							tita = (Mathf.Atan(targetPoint.y/targetPoint.z)) * Mathf.Rad2Deg + 360;
						else 
							tita = tita = (Mathf.Atan(targetPoint.y/targetPoint.z)) * Mathf.Rad2Deg;
					}
				}					
			}
		}
	}
}
