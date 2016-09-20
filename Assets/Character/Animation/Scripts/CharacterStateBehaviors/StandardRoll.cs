using UnityEngine;
using System.Collections;
using Spine.Unity;

namespace Poptropica2.Characters {
	public class StandardRoll : CharacterStateBehavior {
		[Tooltip("In the local space of the view object, where is the pivot point of the Character View while rolling?")]
		public Vector2 pivotOffset;
		public float rollSpeed = 720f;
		public float unrollTime = 0.25f;
		float startTime;
		float rollDirection;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			startTime = Time.time;
			rollDirection = Model.FacingRight ? -1f : 1f;
		}

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			//float curTime = (Time.time - startTime) % stateInfo.length;
			Vector3 pivotPoint = Model.transform.position + new Vector3(pivotOffset.x * ViewTransform.localScale.x , pivotOffset.y * ViewTransform.localScale.y, 0);
			ViewTransform.RotateAround(pivotPoint, Vector3.forward, rollSpeed * rollDirection * Time.fixedDeltaTime);
		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			Model.ResetView(ViewTransform, unrollTime);
		}

		// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
		//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//
		//}

		// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
		//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//
		//}
	}
}