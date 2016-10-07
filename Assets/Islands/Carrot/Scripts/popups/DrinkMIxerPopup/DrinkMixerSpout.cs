using UnityEngine;
using System.Collections;

/// <summary>
/// Drink mixer spout behavior.
/// </summary>
public class DrinkMixerSpout : MonoBehaviour
{
    public enum spoutColors
    {
        black,
        white,
        blue,
        red,
        yellow
    }

	public GameObject pourDrop;
    public spoutColors color;
	public Transform drinkFluid;

	// variables
    private Vector3 startPos = new Vector3(0f, 1.1f, 0f);   // start pos for handle
    private Vector3 downPos = new Vector3(0f, 0.7f, 0f);    // down pos for handle
	private bool isMouseDown = false;			            // mousedown flag
	private bool isGlassFull = false;			            // glass full flag
	private const float dropDelay = 0.05f;		            // delay between drops
    private GameObject handle;                              // spout handle

    void Start()
    {
        handle = transform.Find("handle").gameObject;
    }

	void OnMouseDown()
    {
		// set mousedown state
		isMouseDown = true;
		// move handle down
		handle.transform.localPosition = downPos;
		// check if glass is full
		if (drinkFluid.localScale.y >= 1.0f)
			isGlassFull = true;
		// start pouring
		StartCoroutine (PourLiquid ());
	}

    /// <summary>
    /// Pours the liquid while mouse is down.
    /// </summary>
    /// <returns>The liquid.</returns>
	IEnumerator PourLiquid()
	{
		int count = 0;

		// while mousedown state is true
		while (isMouseDown)
        {
			// create single drop
			GameObject drop = (GameObject)Instantiate (pourDrop, Vector3.zero, Quaternion.identity);
			// set handle as parent
			drop.transform.SetParent (handle.transform);
			// random x offset
			float offset = 0.07f;
			drop.transform.localPosition = new Vector3 (Random.Range(-offset, offset), -1f, 0f);
			// set color
            Color colorVal =  new Color32(0, 0, 0, 255);
            switch (color)
            {
                case spoutColors.white:
                    colorVal =  new Color32(255, 255, 255, 255);
                    break;
                case spoutColors.blue:
                    colorVal =  new Color32(117, 192, 255, 255);
                    break;
                case spoutColors.red:;
                    colorVal =  new Color32(255, 33, 0, 255);
                    break;
                case spoutColors.yellow:
                    colorVal =  new Color32(255, 242, 0, 255);
                    break;
            }

            drop.GetComponent<SpriteRenderer>().color = colorVal;

			// delay between drops
			yield return new WaitForSeconds (dropDelay);

			// check if glass is full
			if (drinkFluid.localScale.y >= 1.0f)
				isGlassFull = true;

			// if full class and 5 drops then break;
			if ((isGlassFull) && (count > 4))
            {
				// restore handle position
				handle.transform.localPosition = startPos;
				break;
			}
			count++;
		}
	}

	void OnMouseUp()
    {
		// set mousedown state to false
		isMouseDown = false;
		// restore handle position
		handle.transform.localPosition = startPos;
	}

	void OnMouseExit()
    {
		// set mousedown state to false
		isMouseDown = false;
		// restore handle position
		handle.transform.localPosition = startPos;
	}
}