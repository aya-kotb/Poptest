using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

/// <summary>
/// Mouse character controller.
/// </summary>
namespace Poptropica2.Characters {
	public class MouseCharacterController : MonoBehaviour, ICharacterController {

		[Range(-1f, 1f)]
		float horizontalAxisDegree = 0f;
		[Range(-1f, 1f)]
		float verticalAxisDegree = 0f;

		InputControlGroup state;

		Vector2 lookOrigin;

		Transform eyeCenter;

		public Collider2D deadZone;					//The deadZone area

		#region ICharacterController implementation
		public InputControlGroup Inputs {
			get {
				return state;
			}
		}
		public float HorizontalAxisDegree {
			get {
				return horizontalAxisDegree;
			}
		}
		public float VerticalAxisDegree {
			get {
				return verticalAxisDegree;
			}
		}
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
		#endregion

		void Awake() {
			state = new InputControlGroup();
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

		bool IsJumping() 
		{
			if (Input.GetKey(KeyCode.Mouse0)) 
			{
				Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if(IsPointInsideDeadZone (mousePosition)||IsPointerOverGameObject())
				{
					return false;
				}

				if(mousePosition.y>deadZone.transform.position.y)
				{
					return true;
				}

			} 
			return false;
		}

		bool IsPushingContext() {
			return false;
		}


		bool IsHorizontalPressed() {
			if (Input.GetKey(KeyCode.Mouse0)) 
			{
				Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if(IsPointInsideDeadZone (mousePosition)||IsPointerOverGameObject())
				{
					return false;
				}

				float mouseDir = (mousePosition - (Vector2)deadZone.transform.position).normalized.x;
				mouseDir = Mathf.Clamp (mouseDir, -1, 1);
				horizontalAxisDegree = mouseDir;
				return true;
			} 
			else 
			{
				horizontalAxisDegree = 0f;
			}
			return false;
		}

		bool IsVerticalPressed() {
            
			if (Input.GetKey(KeyCode.Mouse0)) 
			{
				Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if(IsPointInsideDeadZone (mousePosition)||IsPointerOverGameObject())
				{
					return false;
				}

				float mouseDir = (mousePosition - (Vector2)deadZone.transform.position).normalized.y;
				mouseDir = Mathf.Clamp (mouseDir, -1, 1);
				verticalAxisDegree = mouseDir;
				return true;
			} 
			else 
			{
				verticalAxisDegree = 0f;
			}
			return false;
		}


		/// <summary>
		/// Determines whether a mouseclick/touch point is inside the dead zone
		/// </summary>
		/// <returns><c>true</c> if the mouseclick/touch point is inside the dead zone bounds; otherwise, <c>false</c>.</returns>
		/// <param name="point">Point.</param>
		bool IsPointInsideDeadZone(Vector3 point)
		{
			return deadZone.bounds.Contains (point);
		}
		

        bool IsPointerOverGameObject()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
}
}