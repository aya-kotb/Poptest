using UnityEngine;
using System.Collections;
using Spine;
using Spine.Unity;

namespace Poptropica2.Characters {
	public class PupilOffsetter : MonoBehaviour {
		public Transform pupil;
		public float maxOffset = 0.6f;
		Vector2 pupilOffset;
		public Vector2 PupilOffsetAmt { get { return pupilOffset / maxOffset; } }

		ICharacterController controller;
		Quaternion pupilQuaternion;
		Vector3 originalOffset;
		Skeleton skeleton;

		bool init = false;

		IEnumerator Start() {
			CharacterModel model = GetComponentInParent<CharacterModel>();
            controller = model.controllerContainer.Result;
			skeleton = model.GetComponentInChildren<SkeletonAnimator>().skeleton;
			while (pupil == null) {
				yield return null;
			}
			originalOffset = pupil.localPosition;
			init = true;
		}

		void Update() {
			if (!init) return;
			//Debug.Log("Eye offset angle: " + controller.EyeLookAngle + ", amount: " + controller.EyeOffsetAmount);
			pupilQuaternion = Quaternion.Euler(0f, 0f, skeleton.FlipX ? 180f -  controller.EyeLookAngle: controller.EyeLookAngle);
			Vector3 offset = controller.EyeOffsetAmount * Vector3.right * maxOffset;
			offset = (pupilQuaternion * offset);
			pupilOffset = new Vector2(offset.x, offset.y);
            pupil.localPosition = originalOffset + offset;
		}
	}
}