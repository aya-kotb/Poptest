using UnityEngine;
using System.Collections;

public class EngineSwitch: MonoBehaviour {

	public GameObject knob;
	public GameObject buttonOn;

	public Vector3[] positions;
	public bool isOn = false;

	int currPos = 0;

	[Range(0, 2)]
	public int onNum;

	void OnMouseDown() {
		if (currPos == 0) {
			currPos = 1;
		} else if (currPos == 1) {
			currPos = 2;
		} else {
			currPos = 0;
		}
		knob.transform.localPosition = positions[currPos];
		if (currPos == onNum) {
			buttonOn.gameObject.SetActive (true);
			isOn = true;
		} else {
			buttonOn.gameObject.SetActive (false);
			isOn = false;
		}
	}
}
