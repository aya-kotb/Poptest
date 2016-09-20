using UnityEngine;
using System.Collections;
using System;

namespace Poptropica2.Characters {
	public class GamepadCharacterController : MonoBehaviour, ICharacterController {
		public string horizontalAxisName = "Horizontal";
		public string verticalAxisName = "Vertical";
		public KeyCode jumpButton = KeyCode.Joystick1Button0;
		public KeyCode contextButton = KeyCode.Joystick1Button1;

		InputControlGroup state;
		[Range(-1f, 1f)]
		public float horizontalAxisDegree = 0f;
		[Range(-1f, 1f)]
		public float verticalAxisDegree = 0f;


		void Awake() {
			state = new InputControlGroup();
		}

		//ICharacterController implementations
		public InputControlGroup Inputs { get { return state; } }

		public float HorizontalAxisDegree { get { return horizontalAxisDegree; } }

		public float VerticalAxisDegree { get { return verticalAxisDegree; } }

		public float EyeLookAngle {
			get {
				return Mathf.Atan2(verticalAxisDegree, horizontalAxisDegree) * Mathf.Rad2Deg;
			}
		}

		public float EyeOffsetAmount {
			get {
				return Vector2.ClampMagnitude(new Vector2(horizontalAxisDegree, verticalAxisDegree), 1f).magnitude;
			}
		}

		void Update() {
			//state.Clear();

			state.Insert(InputControl.Context, IsPushingContext());
			state.Insert(InputControl.Jump, IsJumping());
			state.Insert(InputControl.Horizontal, IsHorizontalTilted());
			state.Insert(InputControl.Vertical, IsVerticalTilted());
		}

		bool IsJumping() {
			return Input.GetKey(jumpButton);
		}

		bool IsPushingContext() {
			return Input.GetKey(contextButton);
		}

		bool IsHorizontalTilted() {
			horizontalAxisDegree = Input.GetAxis(horizontalAxisName);
			return !Mathf.Approximately(horizontalAxisDegree, 0f);
		}

		bool IsVerticalTilted() {
			verticalAxisDegree =Input.GetAxis(verticalAxisName);
			return !Mathf.Approximately(verticalAxisDegree, 0f);
		}
	}
}
