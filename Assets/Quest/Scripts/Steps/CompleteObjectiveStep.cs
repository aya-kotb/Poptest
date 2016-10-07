using UnityEngine;
using System.Collections;

namespace Poptropica2.QuestSystem
{
	
	public class CompleteObjectiveStep : QuestStep
	{

		public override void EnterParentState (State state)
		{
			base.EnterParentState (state);
			QuestTester.instance.Listen (stepData.stepType, OnObjectiveCompleted);
		}

		/// <summary>
		/// Callback on step completion
		/// </summary>
		/// <param name="objectiveID">Objective ID</param>
		public void OnObjectiveCompleted (string objectiveID)
		{
			if (objectiveID == ((ObjectiveStepData)stepData).objectiveID) {
				CompleteStep ();
				QuestTester.instance.Remove (stepData.stepType, OnObjectiveCompleted);
			}
		}
	}
}