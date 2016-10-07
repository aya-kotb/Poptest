using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;


namespace Poptropica2 {
	[System.Serializable]
	public class ConstantCondition : IStateTransitionCondition {
		[SerializeField] 
		private bool evaluateTo = false;
		public override string ToString()
		{
			return "Constant: " + evaluateTo;
		}

		public void EnterParentState (State state)
		{
			Debug.Log("Entering state for constant condition");
		}
		public bool Evaluate()
		{
			return evaluateTo;
		}


#if UNITY_EDITOR
        public void ShowInspectorUI ()
		{
			evaluateTo = EditorGUILayout.Toggle("Is True", evaluateTo);
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
