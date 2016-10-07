using UnityEngine;
using System.Collections;

namespace Poptropica2.QuestSystem
{

	public class ConverseWithNPCStep : QuestStep
	{
		public override void EnterParentState (State state)
		{
			base.EnterParentState (state);
			//Assign event to appropriate listener here
		}

		public void OnConversedWithNPC (string stepType)
		{
			if (stepType == stepData.stepType) {
				CompleteStep ();
			}
		}
	}
}