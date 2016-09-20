using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters {
	//[ExecuteInEditMode]
	public class EyelidSpriteAnimator : MonoBehaviour {
		public bool reactiveEyelids = true;
		[Tooltip("A list of sprites from wide open to as closed as possible when tracking the pupil.")]
		public Sprite[] pupilFollowSprites;
		[Tooltip("A sequence of sprites which will play in order whenever a blink is periodically fired.")]
		public Sprite[] blinkSequence;
		[Tooltip("X axis - the height of the pupil. Y axis - the frame number of the eyelid to show.")]
		public AnimationCurve pupilHeightToFrameNum = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));

		public float timeBetweenBlinks = 1f;
		float blinkTimer = 0f;
		bool blinking = false;

		float height;

		SpriteRenderer display;
		GameObject staticRearLash;
		ICharacterController controller;
		int current = 0;
		PupilOffsetter pupilOffsetter;
		
		void Start() {
			display = GetComponent<SpriteRenderer>();
			staticRearLash = transform.FindChild("RearLashesSprite").gameObject;	//This REQUIRED to exist regardless of whether it is visible.
			controller = GetComponentInParent<CharacterModel>().ControllerContainer.Result;
			pupilOffsetter = GetComponentInParent<PupilOffsetter>();
			if (!reactiveEyelids) {
				display.enabled = false;
			}
		}

		void Update() {
			if (!blinking) {
				blinkTimer += Time.deltaTime;
				if (reactiveEyelids) {
					height = pupilOffsetter.PupilOffsetAmt.y;
					current = Mathf.FloorToInt(pupilHeightToFrameNum.Evaluate(height) * (pupilFollowSprites.Length - 1));
					if (current == 0) { //Eyes wide open
						display.enabled = false;
						staticRearLash.SetActive(true);
					} else {	//Eyes partially open
						display.enabled = true;
						display.sprite = pupilFollowSprites[current];
						staticRearLash.SetActive(false);
					}
				}
				if (blinkTimer > timeBetweenBlinks) {
					StartCoroutine(Blink());
				}
			}
		}

		IEnumerator Blink() {
			blinking = true;
			display.enabled = true;
			staticRearLash.SetActive(false);
			for (int i = 0; i < blinkSequence.Length; i++) {
				display.sprite = blinkSequence[i];
				yield return null;
			}
			blinkTimer = 0f;
			blinking = false;
			if (!reactiveEyelids) {
				display.enabled = false;
			}
			staticRearLash.SetActive(true);
		}
	}
}