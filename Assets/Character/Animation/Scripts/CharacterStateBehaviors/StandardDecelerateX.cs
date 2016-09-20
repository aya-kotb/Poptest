using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters {
	public class StandardDecelerateX : CharacterStateBehavior {
		public AnimationCurve slowDownCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));
		float startTime, progress;
		Vector2 startVelocity;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			startTime = Time.time;
			startVelocity = Rb.velocity;
			//Debug.Log("Decel from " + startVelocity + " for " + stateInfo.length + "s.");
		}

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            float curTime = Time.time - startTime;
			float progress = curTime / stateInfo.length;
			progress = progress < 1f ? progress : 1f;

			Vector2 newVelocity = new Vector2(slowDownCurve.Evaluate(progress) * startVelocity.x, Rb.velocity.y);
			if (ValidVelocity(newVelocity)) {
				//Debug.Log("Decel to " + newVelocity + " at time: " + curTime + ", progress: " + progress +  ", curve eval: " + slowDownCurve.Evaluate(progress));
				Rb.velocity = newVelocity;
			} else {
				//Debug.Log("Decel velocity invalid: " + newVelocity);
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