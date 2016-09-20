using UnityEngine;
using System.Collections;

public class EnemyCollider : MonoBehaviour {
    public Vector2 hitVelocity;
    public bool directional = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.transform.position.x > transform.position.x || ! directional) {
			other.attachedRigidbody.AddForce (hitVelocity);
		} else {
			other.attachedRigidbody.AddForce (new Vector2 (-hitVelocity.x, hitVelocity.y));
		}
	}
}
