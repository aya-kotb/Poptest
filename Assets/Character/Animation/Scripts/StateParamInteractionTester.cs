using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters {
	public class StateParamInteractionTester : MonoBehaviour {
		public CharacterModel characterModel;
		public string triggerName;
		public bool trigger;
		public string boolParamName;
		public bool boolParamValue;
			
		void Start() {
		}

		void Update() {
			if (trigger) {
				trigger = false;
				characterModel.SetStateMachineTrigger(Animator.StringToHash(triggerName));
			}

			if (boolParamName != "") {
				characterModel.SetStateMachineBool(Animator.StringToHash(boolParamName), boolParamValue);
			}
		}
	}
}