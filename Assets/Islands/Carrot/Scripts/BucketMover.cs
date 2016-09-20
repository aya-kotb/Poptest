using UnityEngine;
using System.Collections;

public class BucketMover : MonoBehaviour {

	public Vector3 rightLimit;
	public Vector3 startPos;
	public float speed = 0.0f;

	void Start () {
	
	}

	void FixedUpdate () {
		var change = speed * Time.unscaledDeltaTime;
		transform.position = Vector3.MoveTowards (transform.position, rightLimit, change);
		if (rightLimit.x - transform.position.x < 0.5f) {
			transform.position = startPos;
		}
	}
}
