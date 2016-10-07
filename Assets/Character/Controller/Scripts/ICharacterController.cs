using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters {

	/// <summary>
	/// The public-facing aspect of any controller system. Regardless of controller type,
	/// this will provide the common inputs that the character motion and internal animation systems need.
	/// </summary>
	public interface ICharacterController {
		InputControlGroup Inputs { get; }

		/// <summary>
		/// Get the degree to which the controller is in a left or right state. Left is -1, right is 1.
		/// Only used in conjunction with InputControl.Horizontal.
		/// </summary>
		/// <returns>A float, -1 to 1, for the degree.</returns>
		float HorizontalAxisDegree { get; }

		/// <summary>
		/// Get the degree to which the controller is in a left or right state. Left is -1, right is 1.
		/// Only used in conjunction with InputControl.Vertical.
		/// </summary>
		/// <returns>A float, -1 to 1, for the degree.</returns>
		float VerticalAxisDegree { get; }

		/// <summary>Get the angle (right at 0°, up at 90°, etc) to which the eyes should point</summary>
		float EyeLookAngle { get; }

		/// <summary>Get the amount by which the eyes should be offset, from 0 to 1. </summary>
		float EyeOffsetAmount { get; }

	}

	public enum InputControl {
		Horizontal,
		Vertical,
		Jump,
		Context
	}

	public class InputControlGroup {
		BitArray buttons;

		public InputControlGroup() {
			//Set length of queryable button presses to the number of possible inputs
			buttons = new BitArray(System.Enum.GetValues(typeof(InputControl)).Length);
			Clear();
		}

		public void Insert(InputControl index, bool value) {
			buttons[(int)index] = value;
		}

		public void Clear() {
			buttons.SetAll(false);
		}

		public bool Contains(InputControl query) {
			return buttons[(int)query];
		}
	}
}