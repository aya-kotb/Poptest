using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters {

	public class StandardJump : CharacterStateBehavior {
		float jumpStartTime;
		float currentJumpTime;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			jumpStartTime = Time.time;
			//Rb.velocity += Vector2.up * Model.jumpSpeed;
			Rb.AddForce(Vector2.up * Model.jumpSpeed * 100);
		}

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			Vector2 boostForce = Vector2.up * Model.jumpBoost * Time.fixedDeltaTime;

			currentJumpTime = Time.time - jumpStartTime;
			if (currentJumpTime < Model.jumpDuration) {
				//Debug.Log("Jump " + currentJumpTime + " of " + Model.jumpDuration);
				Rb.AddForce(boostForce);
			}
			

		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//
		//}

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