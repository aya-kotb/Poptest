using UnityEngine;
using System.Collections;

/// <summary>
/// Dr. Hare walks back and forth behind window
/// </summary>
public class DrHareRandom : MonoBehaviour
{
    private float startX;
    private float startScale;
    private bool walking = false;
    private const float speed = 0.5f;
    private const float range = 4f;
    private int direction = -1;

	void Start ()
    {
        // get starting x and scale
        startX = transform.position.x;
        startScale = transform.localScale.y;
        // apply delay before first walk
        StartCoroutine(ApplyDelay());
	}
	
	void Update ()
    {
        if (walking)
        {
            // move Dr. Hare
            transform.position = new Vector3(transform.position.x + (direction * speed * Time.deltaTime), transform.position.y, 0f);
            // if going left and reached end
            if ((direction == -1) && (transform.position.x < startX - range))
            {
                walking = false;
                direction = -direction;
                transform.localScale = new Vector3(-startScale, startScale, 1f);
                transform.position = new Vector3(startX - range, transform.position.y, 0f);
                StartCoroutine(ApplyDelay());
            }
            // if going right and reached end
            else if ((direction == 1) && (transform.position.x > startX))
            {
                walking = false;
                direction = -direction;
                transform.localScale = new Vector3(startScale, startScale, 1f);
                transform.position = new Vector3(startX, transform.position.y, 0f);
                StartCoroutine(ApplyDelay());
            }
        }           
	}

    /// <summary>
    /// Applies a delay before Dr. Hare's next walk
    /// </summary>
    IEnumerator ApplyDelay()
    {
        // wait from 5-15 seconds
        yield return new WaitForSeconds (Random.Range(5f, 15f));
        // set walking flag
        walking = true;
    }
}
