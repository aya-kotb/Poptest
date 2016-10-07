using UnityEngine;
using System.Collections;
using Poptropica2.PopupSystem;

/// <summary>
/// Drink mixer cup animation in response to mouse cursor.
/// </summary>
public class DrinkMixerCup : MonoBehaviour
{
	// transition speed to target position
	private const float transitionSpeed = 5f;

	void Update ()
	{
		// get world position of mouse cursor
        Vector3 worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		// get x position based on popup parent and account for scaling
        float localX = (worldPos.x - this.gameObject.transform.parent.transform.position.x) / PopupManager.instance.fitScale;
		// calculate distance between mouse cursor and cup
        float dist = (transform.localPosition.x - localX);

		// clamp x to outward limits
		float offset = 9.4f;
		localX = Mathf.Clamp (localX, -offset, offset);
		// destination vector
		Vector3 destPos = new Vector3 (localX, transform.localPosition.y, 0f);
		// move gradually toward destination vector
		transform.localPosition = Vector3.Lerp (transform.localPosition, destPos, transitionSpeed * Time.unscaledDeltaTime);

		// set cup rotation and limit rotation
		float maxAngle = 10f;
		if (dist > 0)
			transform.localRotation = Quaternion.Euler (0, 0, Mathf.Clamp(dist * 2, 0, maxAngle));
		else
			transform.localRotation = Quaternion.Euler (0, 0, Mathf.Clamp(dist * 2, -maxAngle, 0));
	}
}