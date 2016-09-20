using UnityEngine;
using Poptropica2;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

namespace PopTropica2.NPCAI
{
	[System.Serializable]
	public class NPCConstantCondition : IStateTransitionCondition
	{
		public bool isStateFinished = false;

		public override string ToString ()
		{
			return "Constant: " + isStateFinished;
		}

		public void EnterParentState (State state)
		{
			Debug.Log ("Entering state for constant condition");
		}

		public bool Evaluate ()
		{
			return isStateFinished;
		}


		#if UNITY_EDITOR
		public void ShowInspectorUI ()
		{
			isStateFinished = EditorGUILayout.Toggle ("Is True", isStateFinished);
		}

		public int GetInspectorLineCount ()
		{
			return 1;
		}
		#endif

		public string Serialize ()
		{
			return JsonUtility.ToJson (this);
		}

		public void Deserialize (string serializedContents)
		{
			JsonUtility.FromJsonOverwrite (serializedContents, this);
		}
	}
}