using UnityEngine;
using System.Collections;

namespace Poptropica2.QuestSystem
{
	public class WinMiniGameStep : QuestStep
	{

		public override void EnterParentState (State state)
		{
			base.EnterParentState (state);
			//Assign event to appropriate listener here
		}

		public void OnMiniGameWon (string stepType)
		{
			if (stepType == stepData.stepType) {
				CompleteStep ();
			}
		}
	}
}