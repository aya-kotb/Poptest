using UnityEngine;
using System.Collections;
using Poptropica2.Characters;

public class ControlTester : MonoBehaviour {
	public ICharacterControllerContainer controller;
	public bool horiz, vert, jump, enter;
	public float horizAxis, vertAxis, eyeAngle, eyeOffsetAmt;
	InputControlGroup ctrlState;
	

	void Start() {
		ctrlState = controller.Result.Inputs;
	}

	void Update () {
		horiz = ctrlState.Contains(InputControl.Horizontal);
		vert = ctrlState.Contains(InputControl.Vertical);
		jump = ctrlState.Contains(InputControl.Jump);
		enter = ctrlState.Contains(InputControl.Context);
		horizAxis = controller.Result.HorizontalAxisDegree;
		vertAxis = controller.Result.VerticalAxisDegree;
		eyeAngle = controller.Result.EyeLookAngle;
		eyeOffsetAmt = controller.Result.EyeOffsetAmount;
	}

	[System.Serializable]
	public class ICharacterControllerContainer : IUnifiedContainer<ICharacterController> { }
}
