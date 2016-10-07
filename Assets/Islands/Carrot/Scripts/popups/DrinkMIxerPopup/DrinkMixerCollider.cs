using UnityEngine;
using System.Collections;

/// <summary>
/// Drink mixer collider for cup and bottom of popup.
/// </summary>
public class DrinkMixerCollider : MonoBehaviour
{
	public GameObject drinkButton;

	// variables
	private bool firstDrop = true;				// first drop flag (we don't mix colors on first drop)
	private const float mixFactor = 0.02f;		// amount of mixing
	private float buttonAlpha = 0f;				// drink button alpha value

    void Start()
    {
        // get sprite renderer for drink button
        SpriteRenderer button = drinkButton.GetComponent<SpriteRenderer>();
        button.color = new Color (1f, 1f, 1f, 0f);
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		//if sprite renderer attached, then we are colliding with glass fluid
		if (gameObject != null)
		{
 			// sprite renderer for glass fluid
			SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
			if (sprite != null)
			{
				// get current scale of glass fluid
				Vector3 scale = gameObject.transform.localScale;
				// if still filling glass, then scale up
				if (scale.y < 1.0f)
				{
                    // 100 increments
                    gameObject.transform.localScale = new Vector3 (Mathf.Pow(scale.y, 0.25f), scale.y + 0.01f, 1f);
				}

				// get drop color
				Color dropColor = other.gameObject.GetComponent<SpriteRenderer>().color;
				// get sprite renderer for drink button
				SpriteRenderer button = drinkButton.GetComponent<SpriteRenderer>();
				// if first drop
				if (firstDrop)
				{
					firstDrop = false;
					// set fluid to drop color
					sprite.color = dropColor;
					drinkButton.SetActive (true);
				}
				else
				{
					// mix colors if not first drop
					sprite.color = sprite.color + (dropColor - sprite.color) * mixFactor; 
				}

				// undim drink button
				button.color = new Color (1f, 1f, 1f, buttonAlpha);
				// increase button alpha
				buttonAlpha += 0.05f;
			}
		}
		Destroy(other.gameObject);
	}
}