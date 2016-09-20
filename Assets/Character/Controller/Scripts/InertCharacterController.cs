using UnityEngine;
using System.Collections;
using System;

namespace Poptropica2.Characters {
	public class InertCharacterController : MonoBehaviour, ICharacterController {
		InputControlGroup emptyGroup = new InputControlGroup();

		public float EyeLookAngle {
			get {
				return 0f;
			}
		}

		public float EyeOffsetAmount {
			get {
				return 0f;
			}
		}

		public float HorizontalAxisDegree {
			get {
				return 0f;
			}
		}

		public InputControlGroup Inputs {
			get {
				return emptyGroup;
			}
		}

		public float VerticalAxisDegree {
			get {
				return 0f;
			}
		}
	}
}