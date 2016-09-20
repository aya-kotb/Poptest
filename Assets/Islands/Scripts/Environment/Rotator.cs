using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	public float xspeed = 0.0f;
	public float yspeed = 0.0f;
	public float zspeed = 0.0f;

	public bool isRotating;

    private Rigidbody2D rb;

	void Start() {
        //isRotating = false;
        rb = GetComponent<Rigidbody2D>();
	}

	void Update () {
		if (isRotating) {
            if (rb != null)
            {
                rb.angularVelocity = zspeed;
            }
            else
            {
                transform.Rotate(
                    xspeed * Time.deltaTime,
                    yspeed * Time.deltaTime,
                    zspeed * Time.deltaTime
                );
            }
		}
	}
}
