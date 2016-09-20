using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;


namespace Poptropica2 {
	[System.Serializable]
	public class DialogSelectionCondition : IStateTransitionCondition {
		public string dialogOptionText = "";
		private bool wasTriggered = false;
		public void EnterParentState(State state)
		{
			wasTriggered = false;
			//TODO: attach to dialog UI with Select as a callback
			//DialogSystem.AddChoice(dialogOptionText, Select);
		}

		private void Select()
		{
			wasTriggered = true;
		}
		public bool Evaluate()
		{
			return wasTriggered;
		}


#if UNITY_EDITOR
        public void ShowInspectorUI ()
		{
			dialogOptionText = EditorGUILayout.TextField("Dialog", dialogOptionText);
		}

		public int GetInspectorLineCount()
		{
			return 1;
		}


#endif

		public string Serialize()
		{
			return JsonUtility.ToJson(this);
		}

		public void Deserialize(string serializedContents)
		{
			JsonUtility.FromJsonOverwrite(serializedContents, this);
		}
	}
}
