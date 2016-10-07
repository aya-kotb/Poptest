using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters {
	/// <summary> This is a controller that automatically moves the character around for testing purposes. </summary>
	public class AutoAlternatingCharacterController : MonoBehaviour, ICharacterController {
		public float eyeRollSpeed = 10f;
		public float eyeWobbleSpeed = 3f;
		public float timePerPhase = 0.5f;
		public float rightSideBias = 0.2f;
		public bool eyesOnly = false;

		float curTime;

		float horizontalAxisDegree, verticalAxisDegree;
		InputControlGroup state;
		
		public float EyeLookAngle {
			get {
				return (Time.time * eyeRollSpeed) % 360f;
			}
		}

		public float EyeOffsetAmount {
			get {
				return Mathf.Sin(Time.time * eyeWobbleSpeed);
			}
		}

		public float HorizontalAxisDegree {
			get {
				return horizontalAxisDegree;
			}
		}

		public InputControlGroup Inputs {
			get {
				return state;
			}
		}

		public float VerticalAxisDegree {
			get {
				return verticalAxisDegree;
			}
		}


		void Awake() {
			state = new InputControlGroup();
			curTime = Random.Range(0f, timePerPhase);
		}

		
		void Start() {
			StartCoroutine(AlternateMovementsCoroutine());
		}

		IEnumerator AlternateMovementsCoroutine() {
			while (!eyesOnly) {	//Set before starting
				//Duck
				verticalAxisDegree = -1f;
				state.Insert(InputControl.Vertical, true);
				while (curTime < timePerPhase) {
					curTime += Time.deltaTime;
					yield return null;
				}
				Reset();
				yield return new WaitForSeconds(timePerPhase);
				//Walk left
				state.Insert(InputControl.Horizontal, true);
				while (curTime < timePerPhase) {
					horizontalAxisDegree -= Time.deltaTime;
					horizontalAxisDegree = horizontalAxisDegree < -1f ? -1f : horizontalAxisDegree;
					curTime += Time.deltaTime;
					yield return null;
				}
				Reset();
				yield return new WaitForSeconds(timePerPhase);
				//Walk right
				state.Insert(InputControl.Horizontal, true);
				while (curTime < timePerPhase + rightSideBias) {
					horizontalAxisDegree += Time.deltaTime;
					horizontalAxisDegree = horizontalAxisDegree > 1f ? 1f : horizontalAxisDegree;
					curTime += Time.deltaTime;
					yield return null;
				}
				Reset();
				yield return new WaitForSeconds(timePerPhase);
				// Jump
				state.Insert(InputControl.Jump, true);
				while (curTime < timePerPhase) {
					curTime += Time.deltaTime;
					yield return null;
				}
				yield return new WaitForSeconds(timePerPhase);
				Reset();
			}
		}

		void Reset() {
			state.Clear();
			verticalAxisDegree = 0f;
			horizontalAxisDegree = 0f;
			curTime = 0f;
		}
	}
}