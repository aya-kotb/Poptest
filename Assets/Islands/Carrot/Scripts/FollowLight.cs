using UnityEngine;
using System.Collections;



public class FollowLight : MonoBehaviour {
	public GameObject player;
	public Vector3 offset = new Vector3(-1f, 0f, 0f);
	Vector3 target;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	void Update () {
		target = new Vector3 (player.transform.localPosition.x, player.transform.localPosition.y, -1.58f);
		transform.localPosition = target + offset;
	}
}
