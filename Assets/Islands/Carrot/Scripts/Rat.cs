using UnityEngine;
using System.Collections;
using Spine.Unity;

public class Rat : MonoBehaviour {

	[Header("References")]
	public PolygonCollider2D primaryCollider;
	public SkeletonAnimation skeletonAnimationRat;

	public Vector3 rightLimit;
	public Vector3 leftLimit;

	private Vector3 target;

	public float speed = 0.0f;
	private bool movingRight = true;

	void Start () {
		target = rightLimit;
	}
	
	void Update () {
		var change = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, target, change);
			
		if (Mathf.Abs (target.x - transform.position.x) < 0.05) {
			if (movingRight) {
				movingRight = false;
				target = leftLimit;
				skeletonAnimationRat.transform.localScale = new Vector3 (-1, 1, 1);
			} else {
				movingRight = true;
				target = rightLimit;
				skeletonAnimationRat.transform.localScale = new Vector3 (1, 1, 1);
			}
		}
	}
}
