using UnityEngine;
using System.Collections;

namespace Poptropica2.QuestSystem
{
	public class PutCostumeStep : QuestStep
	{

		public override void EnterParentState (State state)
		{
			base.EnterParentState (state);
			//Assign event to appropriate listener here
		}

		public void OnCostumePut (string stepType)
		{
			if (stepType == stepData.stepType) {
				CompleteStep ();
			}
		}
	}
}