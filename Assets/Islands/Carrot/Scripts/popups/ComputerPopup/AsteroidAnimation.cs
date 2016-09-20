using UnityEngine;
using System.Collections;

/// <summary>
/// Asteroid animation on computer screen.
/// </summary>
public class AsteroidAnimation : MonoBehaviour
{
	public ComputerPopup popup;
	public GameObject explosion;
	public GameObject drHare;

	// variables
	private bool gameOver = false; 				// game over flag
	private float startRabbotX; 				// rabbot start X
	private float startRabbotY; 				// rabbot start Y
	private GameObject rabbot; 					// rabbot game object
	private int hitCounter = 0; 				// collision counter
	private bool showHare = false; 				// show Dr. Hare flag
	private float drHareSpeedX = 0.5f; 			// Dr. Hare speed X (can be inverted)

	// constants
	private const int maxHits = 4; 			    // maximum number of collisions
	private const float asteroidSpeed = 2f; 	// speed of asteroid
	private const float rabbotY = 0.6f; 		// default y position of robbot
	private const float lowerLimit = -16f; 	    // lower limit where the asteroid gets moved back to top (reduce to increase time between asteroids)
	private const float vibrateDelay = 3f; 	    // delay for when rabbot vibrates
	private const float hareSpeedY = 2f; 		// Dr. Hare speed Y
	private const float drHareDelay = 4f; 	    // delay for Dr. Hare fly animation

	void Start()
    {
		// set starting position of asteroid offscreen
		transform.localPosition = new Vector3 (0f, -3f, 0f);
	}

	void FixedUpdate ()
    {
		// set position
		float x = transform.localPosition.x;
		float y = transform.localPosition.y - (asteroidSpeed * Time.deltaTime);

		// if reached lower limit and game not over, then reset at top
		if ((!gameOver) && (y < lowerLimit))
        {
			// top position
			y = 9f;
			x = Random.Range (-4.5f, 4.5f);
			// make visible again
			this.gameObject.GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, 1f);
		}
		transform.localPosition = new Vector3 (x, y, 0f);

		// if game over and no Dr. Hare, then vibrate rabbot
		if ((gameOver) && (!showHare))
        {
			float offset = 0.03f;
			rabbot.transform.localPosition = new Vector3 (startRabbotX + Random.Range (-offset, offset), startRabbotY + Random.Range (-offset, offset), 0f);
		}
        else if (showHare)
        {
			// if showing Dr. Hare, animate and spin Dr. Hare upward
			x = drHare.transform.localPosition.x + (drHareSpeedX * Time.deltaTime);
			y = drHare.transform.localPosition.y + (hareSpeedY * Time.deltaTime);
			drHare.transform.localPosition = new Vector3(x, y, 0f);
			drHare.transform.Rotate(Vector3.forward * 50f * Time.deltaTime);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
    {
		// if collide with rabbot and not game over
		if ((!gameOver) && (Mathf.Abs(other.gameObject.transform.localPosition.y - rabbotY) < 0.1))
        {
			// get rabbot
			rabbot = other.gameObject;
			// increment hit counter
			hitCounter++;
			// make asteroid invisible
			this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0f);
			// check for end game
			gameOver = (hitCounter >= maxHits);
			// move explosion to asteroid
			explosion.transform.position = transform.position;
			// play particles
			explosion.GetComponent<ParticleSystem> ().Play ();
			//notify popup
			popup.CollideAsteroid (hitCounter);
			popup.gameOver = gameOver;

			// if game over, then store rabbot position
			if (gameOver)
            {
				startRabbotX = rabbot.transform.localPosition.x;
				startRabbotY = rabbot.transform.localPosition.y;
				// set timer to explode rabbot
				StartCoroutine (ExplodeRabbot ());
			}
		}
	}

    /// <summary>
    /// Explodes the rabbot ship.
    /// </summary>
	IEnumerator ExplodeRabbot()
    {
		// wait for rabbot to vibrate
		yield return new WaitForSeconds (vibrateDelay);
		// hide rabbot
		rabbot.SetActive (false);
		// move explosion to ship
		explosion.transform.position = rabbot.transform.position;
		// play particles
		explosion.GetComponent<ParticleSystem> ().Play ();
		// show Dr. Hare
		showHare = true;
		drHare.transform.position = rabbot.transform.position;
		// if Dr. Hare is to right then invert speed
		if (rabbot.transform.localPosition.x > 0f)
			drHareSpeedX = -drHareSpeedX;
		// wait for Dr. Hare to animate offscreen
		yield return new WaitForSeconds (drHareDelay);
		// close popup
		popup.Close ();
	}
}