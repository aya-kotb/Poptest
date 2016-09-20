using UnityEngine;
using System.Collections;

public class RabbotEarsButton : MonoBehaviour {
	public Sprite buttonStart;
	public Sprite buttonDown;
	public Sprite buttonPressed;

	public Vector3[] positions;

	public RabbotEarsPopup mainPopup;

	SpriteRenderer rend;

	// Use this for initialization
	void Start () {
		rend = GetComponent<SpriteRenderer> ();
	}

	void OnMouseDown() {
		rend.sprite = buttonDown;
		transform.localPosition = positions [1];
	}

	void OnMouseUp() {
		rend.sprite = buttonPressed;
		transform.localPosition = positions [0];
		mainPopup.Close ();
	}
}
