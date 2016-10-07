using UnityEngine;
using System.Collections;
using System;

namespace Poptropica2.Characters {
	public class StandardCharacterModel : CharacterModel {
		//For testing current state
		AnimatorStateInfo stateInfo;
		readonly int skidHash = Animator.StringToHash("skid");
		readonly int rollHash = Animator.StringToHash("roll");

        [HideInInspector] public bool isDisabled = false; // RLH: to pause avatar during popups

		//Layer masks
		int envAndPlatsMask;
		int pushableMask;

		[Header("Standard Jump")]
		public float jumpSpeedLimitX;


		GameObject view;
		public GameObject View {
			get {
				if (view == null) {
					view = GetComponentInChildren<Spine.Unity.SkeletonAnimator>().gameObject;
				}
				return view;
			}
		}

		void OnEnable()
		{
			EventManager.GetInstance ().OnOverrideController += OnOverrideController;
		}


		void OnDisable()
		{
			EventManager.GetInstance ().OnOverrideController -= OnOverrideController;
		}

		void OnOverrideController(ICharacterController overridingController)
		{
			controller = overridingController;
		}
			
		/**
		Material skeletonMaterial;
		public Material SkeletonMaterial {
			get {
				if (skeletonMaterial == null) {
					skeletonMaterial = View.GetComponent<Renderer>().material;
				}
				return skeletonMaterial;
			}
			set {
				View.GetComponent<ReyePositionenderer>().material = value;
            }
		}/**/


		bool facingRight;
		override public bool FacingRight {
			get {
				return facingRight;
			}
			protected set {
				facingRight = value;
				skeletonAnimator.skeleton.flipX = value;
			}
		}

		public override bool OnRope {
			get {
				return base.OnRope;
			}
			set {   //Set by rope objects.
				base.OnRope = value;
				stateMachine.SetBool(StandardAnimatorHashes.climbing, OnRope);
			}
		}

		public override bool InWater {
			get {
				return base.InWater;
			}

			set {
				base.InWater = value;
				stateMachine.SetBool(StandardAnimatorHashes.swimming, InWater);
			}
		}


		override protected void Start() {
			base.Start();
			envAndPlatsMask = LayerMask.GetMask("Environment", "Platforms");
			pushableMask = LayerMask.GetMask("Environment");
		}


		bool onGround, tryingMoveX;
		public float jumpStartTime;

		void FixedUpdate()
        {
            if (!isDisabled)
            {
                //Send controller axes to state machine
                stateMachine.SetFloat(StandardAnimatorHashes.speedPctHoriz, Mathf.Abs(controller.HorizontalAxisDegree));
                stateMachine.SetFloat(StandardAnimatorHashes.speedPctVert, controller.VerticalAxisDegree);

                stateInfo = stateMachine.GetCurrentAnimatorStateInfo(0);

                tryingMoveX = !Mathf.Approximately(controller.HorizontalAxisDegree, 0f);	//Controller is attempting to move the character in the X axis.
                if (tryingMoveX && stateInfo.shortNameHash != skidHash && stateInfo.shortNameHash != rollHash)
                { 
                    //About face if changing directions, but not if releasing horizontal controls or in a state which shouldn't reverse.
                    FacingRight = controller.HorizontalAxisDegree > 0;
                }

                onGround = CheckOnGround();
                stateMachine.SetBool(StandardAnimatorHashes.onGround, onGround);
                stateMachine.SetBool(StandardAnimatorHashes.falling, !onGround && !InWater && !OnRope);    //Falling if not on ground, in water, or on rope. (jump & fall triggers together ok)
                stateMachine.SetBool(StandardAnimatorHashes.jumping, Jumping(onGround, OnRope, InWater));
                stateMachine.SetBool(StandardAnimatorHashes.pushing, CheckPushing());
                stateMachine.SetBool(StandardAnimatorHashes.ducking, onGround && controller.VerticalAxisDegree < 0);
            }
		}

		RaycastHit2D hit;
		bool CheckOnGround() {
            for (int i = 0; i < groundRaycasters.Length; i++) {
				hit = Physics2D.Raycast(groundRaycasters[i].position, Vector2.down, 0.2f, envAndPlatsMask);
                if (hit.collider != null) {
					//Debug.Log("Hit ground: " + hit.collider.gameObject.name);
					//jumpNeedsRelease = controller.Inputs.Contains(InputControl.Jump);
					return true;
				}
			}
			return false;
		}

		bool CheckPushing() {
			if (!tryingMoveX) { //Even if touching something, not pushing if not trying to move.
				return false;
			}

			float direction = FacingRight ? 1f : -1f;
			float distance = 0.75f;

			for (int i = 0; i < lateralRaycasters.Length; i++) {
				hit = Physics2D.Raycast(lateralRaycasters[i].position, direction * Vector2.right, distance, pushableMask);
				//Debug.DrawLine(lateralRaycasters[i].position, lateralRaycasters[i].position + new Vector3(distance * direction, 0f, 0f), Color.red);
				if (hit.collider != null) {
					return true;
				}
			}
			return false;
		}

		float currentJumpTime;
		bool Jumping(bool onGround, bool onRope, bool inWater) {
			if (controller.Inputs.Contains(InputControl.Jump)) 
			{
				if ((onGround || onRope || inWater)) 
				{
					jumpStartTime = Time.time;
					return true;
				} 
				else 
				{
					currentJumpTime = Time.time - jumpStartTime;
					if (currentJumpTime < jumpDuration) {
						//Debug.Log(currentJumpTime + " of " + jumpDuration);
						return true;
					}
				}
			}
            canJumpToTarget = false;
			return false;
		}
	}



	/// <summary>For communicating with Animator state machine without using strings. </summary>
	public static class StandardAnimatorHashes {
		public static readonly int hurt = Animator.StringToHash("hurt");
		public static readonly int speedPctHoriz = Animator.StringToHash("speedPctHoriz");
		public static readonly int speedPctVert = Animator.StringToHash("speedPctVert");
		public static readonly int jumping = Animator.StringToHash("jumping");
		public static readonly int falling = Animator.StringToHash("falling");
		public static readonly int swimming = Animator.StringToHash("swimming");
		public static readonly int climbing = Animator.StringToHash("climbing");
		public static readonly int ducking = Animator.StringToHash("ducking");
		public static readonly int pushing = Animator.StringToHash("pushing");
		public static readonly int onGround = Animator.StringToHash("onGround");
	}
}
