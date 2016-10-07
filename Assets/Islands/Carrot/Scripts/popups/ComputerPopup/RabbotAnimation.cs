using UnityEngine;
using System.Collections;

/// <summary>
/// Rabbot ship animation.
/// </summary>
public class RabbotAnimation : MonoBehaviour
{
    public GameObject launchAnim;
	public ComputerPopup popup;

	// variables
	private float startX;				// starting x position or rabbot
	private float startY;				// starting y position or rabbot
	private bool startLaunch = false;	// start launch flag
	private float velocity = 0;			// current vertical speed

	void Awake()
    {
		// get starting position
		startX = transform.localPosition.x;
		startY = transform.localPosition.y;
	}

	void Update ()
    {
		
		// increase rabbot speed if launching
		if (startLaunch)
        {
			velocity += (0.04f * Time.deltaTime);
			startY += velocity;
		}

		// apply speed and shake
		float offset = 0.03f;
		transform.localPosition = new Vector3(startX + Random.Range(-offset, offset), startY + Random.Range(-offset, offset), 0f);

		// when rabbot is offscreen, then hide animation and start asteroid game
		if (startY > 10)
        {
			launchAnim.SetActive (false);
			popup.StartGame();
		}
	}

    /// <summary>
    /// Starts launch sequence.
    /// </summary>
	public void DoLaunch()
    {
		startLaunch = true;
	}
}