using UnityEngine;
using System.Collections;

public class TrapDoors : MonoBehaviour {
	public GameObject[] traps;
	// Use this for initialization
	void Start () {
		RotateFirstSet ();
		traps [0].GetComponent<BoxCollider2D> ().enabled = false;
		traps [1].GetComponent<BoxCollider2D> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void RotateFirstSet() {
		traps [2].GetComponent<BoxCollider2D> ().enabled = true;
		traps [3].GetComponent<BoxCollider2D> ().enabled = true;

		LeanTween.rotate(traps[0], new Vector3(0f,0f,0f), 1f).setDelay( 2f );
		LeanTween.rotate(traps[1], new Vector3(0f,0f,0f), 1f).setDelay( 2f );
		LeanTween.rotate(traps[2], new Vector3(0f,0f,90f), 1f).setDelay( 2f );
		LeanTween.rotate(traps[3], new Vector3(0f,0f,90f), 1f).setDelay( 2f ).setOnComplete(RotateSecondSet).setOnStart(FirstSetPlatforms);

	}

	void RotateSecondSet() {
		traps [0].GetComponent<BoxCollider2D> ().enabled = true;
		traps [1].GetComponent<BoxCollider2D> ().enabled = true;

		LeanTween.rotate(traps[0], new Vector3(0f,0f,90f), 1f).setDelay( 2f );
		LeanTween.rotate(traps[1], new Vector3(0f,0f,90f), 1f).setDelay( 2f );
		LeanTween.rotate(traps[2], new Vector3(0f,0f,0f), 1f).setDelay( 2f );
		LeanTween.rotate(traps[3], new Vector3(0f,0f,0f), 1f).setDelay( 2f ).setOnComplete(RotateFirstSet).setOnStart(SecondSetPlatforms);
	}

	void FirstSetPlatforms() {
		traps [2].GetComponent<BoxCollider2D> ().enabled = false;
		traps [3].GetComponent<BoxCollider2D> ().enabled = false;
	}

	void SecondSetPlatforms() {
		traps [0].GetComponent<BoxCollider2D> ().enabled = false;
		traps [1].GetComponent<BoxCollider2D> ().enabled = false;
	}
}
