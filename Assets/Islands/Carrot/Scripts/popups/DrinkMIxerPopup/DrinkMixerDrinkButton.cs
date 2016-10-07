using UnityEngine;
using System.Collections;
using Poptropica2.PopupSystem;

/// <summary>
/// Drink mixer drink button.
/// </summary>
public class DrinkMixerDrinkButton : MonoBehaviour
{
	public SpriteRenderer drinkFluid;
    public Sprite hiliteSprite;

    private Sprite startSprite;

    void Start()
    {
        startSprite = gameObject.GetComponent<SpriteRenderer> ().sprite;
    }

	void OnMouseDown()
    {
		// hilite button if game object exists
		if (gameObject != null)
        {
			SpriteRenderer button = gameObject.GetComponent<SpriteRenderer> ();
			// apply hilite
            button.sprite = hiliteSprite;
		}
	}

	void OnMouseExit()
    {
		// unhilite button if game object exists
		if (gameObject != null)
        {
			SpriteRenderer button = gameObject.GetComponent<SpriteRenderer> ();
            // restore sprite
            button.sprite = startSprite;
		}
	}

	void OnMouseUpAsButton()
    {
		// set avatar hair color
		//Globals.instance.SetPlayerPartColor("hair", drinkFluid.color);
		// make avatar drink
		// close popup
        transform.parent.GetComponent<PopupNonUI>().Close();
	}
}