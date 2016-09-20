using UnityEngine;
using System.Collections;

/// <summary>
/// Computer joystick bevavior.
/// </summary>
public class ComputerJoystick : MonoBehaviour
{
	public GameObject rabbot;

	// variables
	private Vector3 rotation = Vector3.zero;	// init base vector4
	private bool mouseDown = false;				// mousedown flag
	private float lastAngle = 0f;				// last joystick angle
	private float speedX = 0f;					// rabbot horizontal speed
    private ComputerPopup popup;                // reference to popup

    void Start()
    {
        popup = transform.parent.gameObject.GetComponent<ComputerPopup>();
    }

	void Update ()
    {
		// skip out if game over
		if (popup.gameOver)
			return;
		
		// if mouse is down
		if (mouseDown) {
			
			// get mouse position
			Vector3 mouseRef = Input.mousePosition;

			// get offsets
			float offsetX = Mathf.Clamp (this.transform.position.x - Camera.main.ScreenToWorldPoint (mouseRef).x, -2.5f, 2.5f);
			float offsetY = -this.transform.position.y + Camera.main.ScreenToWorldPoint (mouseRef).y;

			// calculate angle in degrees and clamp to 30 degrees each side
			float angle = Mathf.Clamp (Mathf.Atan2 (offsetX, offsetY) * 180f / Mathf.PI, -30f, 30f);

			// set new rotation
			rotation.z = angle - lastAngle;
			transform.Rotate (rotation);

			// remember last angle
			lastAngle = angle;

			// calc rabbot speed
			speedX = offsetX * 0.6f;
		}

		// update rabbot based on speed even when mouse is up
		float x = Mathf.Clamp(rabbot.transform.localPosition.x - speedX * Time.deltaTime, -4.5f, 4.5f);
		rabbot.transform.localPosition = new Vector3(x, rabbot.transform.localPosition.y, 0f);
	}

	void OnMouseDown()
    {
		mouseDown = true;
	}

	void OnMouseUp()
    {
		mouseDown = false;
	}
}