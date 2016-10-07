using UnityEngine;
using System.Collections;

namespace Poptropica2.QuestSystem
{
	public class PickUpItemStep : QuestStep
	{
		public override void EnterParentState (State state)
		{
			base.EnterParentState (state);
			QuestTester.instance.Listen (stepData.stepType, OnItemAcquired);
		}

		/// <summary>
		/// Callback on step completion
		/// </summary>
		/// <param name="objectiveID">Object ID</param>
		public void OnItemAcquired (string objectID)
		{
			if (objectID == ((PickUpItemStepData)stepData).objectID) {
				CompleteStep ();
				QuestTester.instance.Remove (stepData.stepType, OnItemAcquired);
			}
		}
	}
}