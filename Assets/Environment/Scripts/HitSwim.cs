using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters {
	public class HitSwim : MonoBehaviour {
		
		public float lockoutTime = 0.2f;
		bool lockedOut = false;

		void OnTriggerEnter2D(Collider2D other) {
			if (!lockedOut) {
				if (other.tag == "PrimaryPlayerCollider") {
					CharacterModel characterModel = other.attachedRigidbody.GetComponent<CharacterModel>();
					if (characterModel.InWater == false) {
						characterModel.InWater = true;
					}
				}
			}
		}

		void OnTriggerExit2D(Collider2D other) {
			if (!lockedOut) {
				if (other.tag == "PrimaryPlayerCollider") {
					CharacterModel characterModel = other.attachedRigidbody.GetComponent<CharacterModel>();
					if (characterModel.InWater) {
						characterModel.InWater = false;
					}
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