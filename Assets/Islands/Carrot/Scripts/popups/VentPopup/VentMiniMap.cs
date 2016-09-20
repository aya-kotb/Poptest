using UnityEngine;
using System.Collections;

public class VentMiniMap : MonoBehaviour {
	public GameObject player;

	public Vector3 offset = new Vector3(0f, 0f, 0f);
	public Vector3 scaleFactor = new Vector3(1.5f, 1.5f, 1f);

	public GameObject largeMap;
	public GameObject miniMap;

	Vector3 newPos;
	Vector3 playerPos;

	void Start() {
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update () {
		playerPos = player.transform.localPosition;

		newPos = Vector3.Scale(playerPos, scaleFactor);

		newPos = newPos + offset;

		this.transform.localPosition = newPos;
	}

	public void OpenLargeMap() {
		largeMap.SetActive (true);
		miniMap.SetActive (false);
	}

	public void OpenMiniMap() {
		largeMap.SetActive (false);
		miniMap.SetActive (true);
	}
}
