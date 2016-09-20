using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters {
	public class HitClimb : MonoBehaviour {
		public GameObject platforms;
		public float lockoutTime = 0.2f;
		bool lockedOut = false;

		void Start() {
			// NEED TO STRIP OUT ANYTHING UNNECESSARY AND PUT INTO CLIMBSTATE//**********************************
		}

		void OnTriggerEnter2D(Collider2D other) {
			if (!lockedOut) {
				if (other.tag == "PrimaryPlayerCollider") {
					CharacterModel characterModel = other.attachedRigidbody.GetComponent<CharacterModel>();
					if (characterModel.OnRope == false) {
						characterModel.OnRope = true;
						Physics2D.IgnoreLayerCollision(8, 10, true);
						platforms.SetActive(false);

						characterModel.HitPosition = transform.position;
					}
				}
			}
		}

		void OnTriggerExit2D(Collider2D other) {
			if (other.tag == "PrimaryPlayerCollider") {
				CharacterModel characterModel = other.attachedRigidbody.GetComponent<CharacterModel>();
				if (characterModel.OnRope) {
					characterModel.OnRope = false;
					Physics2D.IgnoreLayerCollision(8, 10, false);
					platforms.SetActive(true);
					//pc.state = PlayerController.ActionState.FALL;
					StartCoroutine(LockoutRoutine(other));
				}
			}
		}

		IEnumerator LockoutRoutine(Collider2D other) {
			lockedOut = true;
			yield return new WaitForSeconds(lockoutTime);
			lockedOut = false;
		}
	}
}