using UnityEngine;
using System.Collections;
using Poptropica2;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Poptropica2.QuestSystem
{

	/// <summary>
	/// Step condition holds the condition that will be evaluated
	/// </summary>
	public class QuestStep : IStateTransitionCondition
	{

		public StepData stepData;
		//callback from queststage whenever any step is completed
		public Action OnStepComplete;

		[SerializeField] 
		private bool isComplete = false;

		public override string ToString ()
		{
			return "Constant: " + isComplete;
		}

		public virtual void EnterParentState (State state)
		{
			Debug.Log ("Entering state for constant condition");
		}

		/// <summary>
		/// Completes the step and specified action is invoked on parent state
		/// </summary>
		public void CompleteStep ()
		{
			stepData.isComplete = true;
			OnStepComplete.Invoke ();
		}

		/// <summary>
		/// Evaluate this step if it is mandatory otherwise it will return true
		/// </summary>
		public bool Evaluate ()
		{
			if (!stepData.mandatory)
				return true;
			else
				isComplete = stepData.isComplete;
			return isComplete;
		}

		#if UNITY_EDITOR
		public void ShowInspectorUI ()
		{
			isComplete = EditorGUILayout.Toggle ("Is True", isComplete);
		}
		#endif

		public int GetInspectorLineCount ()
		{
			return 1;
		}

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