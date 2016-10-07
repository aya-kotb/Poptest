using UnityEngine;
using System.Collections;

public class ParallaxSpeed : MonoBehaviour
{
    public float speed = 0.75f;

    // base position vector
    private Vector3 basePos;
    // inverse speed
    private float iSpeed;

	void Start ()
    {
        // calculate offsets so backdrop aligns with background at lower left
        float offsetY = Camera.main.orthographicSize / 4.0f;
        float offsetX = offsetY * Screen.width / Screen.height;
        // set base position vector
        basePos = new Vector3(transform.position.x - offsetX, transform.position.y - offsetY, 0f);
        // get inverse speed
        iSpeed = 1.0f - speed;
	}
	
	void LateUpdate ()
    {
        // get current camera position vector
        Vector3 cam = Camera.main.transform.position;
        // set transform
        transform.position = new Vector3(basePos.x + iSpeed * cam.x, basePos.y + iSpeed * cam.y, 0f);
	}
}