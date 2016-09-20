using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;


namespace Poptropica2 {
	[System.Serializable]
	public class TimedCondition : IStateTransitionCondition {
		public float durationUntilTrigger = 1f;
		private float beganEvaluating = -1f;

		public override string ToString()
		{
			return "Timed: " + durationUntilTrigger;
		}
		public void EnterParentState(State state)
		{
			Debug.Log("Entering state for timed condition");
			beganEvaluating = Time.time;
		}
		public bool Evaluate()
		{
			return Time.time > beganEvaluating + durationUntilTrigger;
		}



#if UNITY_EDITOR
        public void ShowInspectorUI ()
		{
			durationUntilTrigger = EditorGUILayout.FloatField("Duration", durationUntilTrigger);
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
