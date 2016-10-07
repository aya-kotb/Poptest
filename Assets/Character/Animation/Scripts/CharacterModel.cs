using UnityEngine;
using System.Collections;
using Spine.Unity;

namespace Poptropica2.Characters {
	/// <summary>
	/// The model for any player character or NPC, which determines, contains, and makes accessible the state of the character for use by other systems. 
	/// For example, whether the character is standing on the ground, mid-jump, or grasping a rope. Avoid affecting physics here - State machine behaviors should do that.
	/// Subclasses of this should inform the state machine of world and controller conditions only.
	/// </summary>
	public abstract class CharacterModel : MonoBehaviour {

		public enum CharacterModelTypes{Standard,Alternate};
		public CharacterModelTypes characterModelType;

		[Tooltip("Gamepad, keyboard, etc.")]
		public ICharacterControllerContainer controllerContainer;
		protected ICharacterController controller;

		[Header("Speeds & Timings")]
		public float runAccel = 5;
		[Tooltip("Initial jump velocity")]
		public float jumpSpeed = 12;
		[Tooltip("How long you can hold onto the jump button to increase jump height")]
		public float jumpDuration = 0.5f;
		[Tooltip("Effect of holding down the jump button")]
		public float jumpBoost = 12;

        [Tooltip("Max velocity for targeted jumping(Only for mouse/touch)")]
        public Vector2 maxTargetedJumpVelocity = new Vector2(10,10);

        [HideInInspector]
        public Vector2 jumpTarget;
        public bool canJumpToTarget = false;

		protected Animator stateMachine;
		protected Vector2 hitPosition;
		protected Vector2 eyePosition;
		protected float eyelidOpenness;
		protected SkeletonAnimator skeletonAnimator;
		protected bool climbing, swimming;

		public abstract bool FacingRight { get; protected set; }
		public virtual bool OnRope { get { return climbing; } set { climbing = value; } }
		public virtual bool InWater { get { return swimming; } set { swimming = value; } }
		public virtual Vector2 HitPosition { get { return hitPosition; } set { hitPosition = value; } }
		public virtual Vector2 EyePosition { get { return eyePosition;  } }
		public virtual float EyelidOpenness { get { return eyelidOpenness; } }
		public virtual Animator StateMachine { get { return stateMachine; } }


		[Header("Setup")]
		public Transform[] groundRaycasters;
		public Transform[] lateralRaycasters;
		public Collider2D primaryCollider;
		public Collider2D bodyCollider;
		public PupilOffsetter pupilOffseter;

		Vector3 viewResetPos;

		protected virtual void LinkToStateBehaviors() {
			//Allow state behaviors to reference character-related objects outside the behaviors themselves, without searching each time
			Rigidbody2D rb = GetComponent<Rigidbody2D>();

			CharacterStateBehavior[] behaviors = StateMachine.GetBehaviours<CharacterStateBehavior>();
			foreach (CharacterStateBehavior behavior in behaviors) {
				behavior.Model = this;
				behavior.Rb = rb;
				behavior.BodyCollider = bodyCollider;
				behavior.PrimaryCollider = primaryCollider;
				behavior.ViewTransform = skeletonAnimator.gameObject.transform;
				behavior.HitPosition = Vector2.zero;	//Only set and updated by the scene's "hit" colliders
			}
		}

		protected virtual void Start() {
			stateMachine = GetComponentInChildren<Animator>();
			skeletonAnimator = GetComponentInChildren<SkeletonAnimator>();
			viewResetPos = skeletonAnimator.transform.localPosition;    //Store this for ResetView()
			LinkToStateBehaviors();		
			controller = controllerContainer.Result;//Make controller linkable in the editor, still accessible as an interface.
		}



		/// <summary>
		/// For animations beyond the basic locomotion ones like walk, run, idle, etc., that can be set by the environment, command the trigger to change here.
		/// </summary>
		/// <param name="animationParameterHash">A hash of the trigger's name, acquired by using Animator.StringToHash(string name). Get this during initialization, NOT on every frame.</param>
		public void SetStateMachineTrigger(int animationParameterHash) {
			stateMachine.SetTrigger(animationParameterHash);
		}
		
		public void ResetStateMachineTrigger(int animationParameterHash) {
			stateMachine.ResetTrigger(animationParameterHash);
		}

		/// <summary>
		/// For animations beyond the basic locomotion ones like walk, run, idle, etc., that can be set by the environment, command the bool parameter to change here.
		/// </summary>
		/// <param name="animationParameterHash">A hash of the trigger's name, acquired by using Animator.StringToHash(string name).  Get this during initialization, NOT on every frame.</param>
		public void SetStateMachineBool(int animationParameterHash, bool val) {
			stateMachine.SetBool(animationParameterHash, val);
		}

		/// <summary>stateMachine
		/// Animates the return of the view (the SkeletonAnimator's GameObject) from an adjusted position/rotation back to the default (same as the model's).
		/// Useful for the exit of State Behaviors that rotate or otherwise move the view away from the model.
		/// </summary>
		/// <param name="viewTransform">The transform which has been adjusted and needs to be reset.</param>
		/// <param name="timeToReset">Time in seconds until the reset is complete.</param>
		/// <returns></returns>
		public void ResetView(Transform viewTransform, float timeToReset) {
			StartCoroutine(ResetViewCoroutine(viewTransform, timeToReset));
		}

		IEnumerator ResetViewCoroutine(Transform viewTransform, float timeToReset) {
			if (timeToReset > 0f) {
				float startTime = Time.time;
				float endTime = startTime + timeToReset;
				Vector3 startPos = viewTransform.localPosition;
                Quaternion startRot = viewTransform.localRotation;

				while (Time.time < endTime) {
					float progress = (Time.time - startTime) / timeToReset;
                    viewTransform.localPosition = Vector3.Lerp(startPos, viewResetPos, progress);
					viewTransform.localRotation = Quaternion.Lerp(startRot, Quaternion.identity, progress);
					//Debug.Log("Restting view. Progress: " + progress);
					yield return null;
				}
			}
			viewTransform.localPosition = viewResetPos;
			viewTransform.localRotation = Quaternion.identity;
		}

		[System.Serializable]
		public class ICharacterControllerContainer : IUnifiedContainer<ICharacterController> { }	//Wrapper to serialize interface
	}
}
