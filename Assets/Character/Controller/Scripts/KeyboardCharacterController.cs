using UnityEngine;
using System.Collections;
using System;

namespace Poptropica2.Characters {
	public class KeyboardCharacterController : MonoBehaviour, ICharacterController {
		public KeyCode keyLeft = KeyCode.A;
		public KeyCode keyRight = KeyCode.D;
		public KeyCode keyDown = KeyCode.S;
		public KeyCode keyUp = KeyCode.W;
		public KeyCode keyContext = KeyCode.W;
		public KeyCode keyJump = KeyCode.Space;
		public string horizontalAxisName = "Horizontal";
		public string verticalAxisName = "Vertical";


		InputControlGroup state;
		[Range(-1f, 1f)]
		float horizontalAxisDegree = 0f;
		[Range(-1f, 1f)]
		float verticalAxisDegree = 0f;
		bool doorThisFrame, jumpThisFrame, walkThisFrame;

		void Awake() {
			state = new InputControlGroup();
		}

		Transform eyeCenter;

		//ICharacterController implementations
		public InputControlGroup Inputs { get { return state; } }

		public float HorizontalAxisDegree { get { return horizontalAxisDegree; } }

		public float VerticalAxisDegree { get { return verticalAxisDegree; } }

		Vector2 lookOrigin;
		public float EyeLookAngle {
			get {
				Vector2 originToMouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - lookOrigin;
				float angle = Vector2.Angle(Vector2.right, originToMouse);
				if (originToMouse.y < 0) {
					angle = (180f- angle) + 180f;
				}
                return angle;
			}
		}

		public float EyeOffsetAmount {
			get {
				//Assumes landscape mode - smaller of screen height and width is height. 
				//Clamped to 1, because in the editor and possibly other environments the mouse can go out of bounds.
				float offset = (Vector2.Distance(lookOrigin, Input.mousePosition) / Screen.height) * 2f;
				return offset > 1f ? 1f : offset;
			}
		}

		void Start() {
			eyeCenter = GameObject.FindWithTag("Player").GetComponent<CharacterModel>().pupilOffseter.transform;
			if (eyeCenter == null) { eyeCenter = this.transform; }
		}

		void Update() {
			//state.Clear();

			state.Insert(InputControl.Context, IsPushingContext());
			state.Insert(InputControl.Jump, IsJumping());
			state.Insert(InputControl.Horizontal, IsHorizontalPressed());
			state.Insert(InputControl.Vertical, IsVerticalPressed());

			lookOrigin = Camera.main.WorldToScreenPoint(eyeCenter.position);
		}

		bool IsJumping() {
			return Input.GetKey(keyJump);
		}

		bool IsPushingContext() {
			return Input.GetKey(keyContext);
		}

		bool IsHorizontalPressed() {
			if (Input.GetKey(keyLeft)) {
				horizontalAxisDegree = Input.GetAxis(horizontalAxisName);
				return true;
			} else if (Input.GetKey(keyRight)) {
				horizontalAxisDegree = Input.GetAxis(horizontalAxisName);
				return true;
			} else {
				horizontalAxisDegree = 0f;
			}
			return false;
		}

		bool IsVerticalPressed() {
			if (Input.GetKey(keyDown)) {
				verticalAxisDegree = Input.GetAxis(verticalAxisName);
				return true;
			} else if (Input.GetKey(keyUp)) {
				verticalAxisDegree = Input.GetAxis(verticalAxisName);
				return true;
			} else {
				verticalAxisDegree = 0f;
			}
			return false;
		}

	}		
}
