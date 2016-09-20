using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters {
	public class StandardGravityScale : CharacterStateBehavior {
		public float gravityScale;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			Rb.gravityScale = gravityScale;
		}

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			//Debug.Log("Grav scale: " + Rb.gravityScale + ", velocity: " + Rb.velocity);
			if (gravityScale == 0f && Mathf.Approximately(animator.GetFloat(StandardAnimatorHashes.speedPctVert), 0f)) {
				Rb.velocity = new Vector2(Rb.velocity.x, 0f);
			}
		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			Rb.gravityScale = 1f;
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