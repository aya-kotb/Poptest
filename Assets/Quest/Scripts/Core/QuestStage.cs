using UnityEngine;
using System.Collections;
using Poptropica2;
using System.Collections.Generic;
using System;

namespace Poptropica2.QuestSystem
{

	public class QuestStage : State
	{
		public StageData stageData;
		//callback from quest whenever any stage is completed
		public Action OnStageComplete;

		void Awake ()
		{
			onEnterAction = new UnityEngine.Events.UnityEvent ();
			onEnterAction.AddListener (OnActivate);
			onExitAction = new UnityEngine.Events.UnityEvent ();
			onExitAction.AddListener (CompleteStage);
			onUpdateAction = new UnityEngine.Events.UnityEvent ();
		}

		/// <summary>
		/// Stage is activated at start and remain active until stage is completed
		/// </summary>
		private void OnActivate ()
		{
			stageData.isActive = true;
			links [0].conditions = new IStateTransitionCondition[stageData.steps.Count];

			int index = 0;
			foreach (var step in stageData.steps) {
				QuestStep condition = GetCondition (step);
				condition.stepData = step;
				condition.OnStepComplete = CompleteStage;
				links [0].conditions [index++] = condition;
			}
		}

		public QuestStep GetCondition (StepData stepData)
		{
			if (stepData.stepType == "AcquireObject")
				return new PickUpItemStep ();
			else if (stepData.stepType == "CompleteObjective")
				return new CompleteObjectiveStep ();
			return null;
		}

		/// <summary>
		/// Completes the stage if all the mandatory conditions are completed
		/// </summary>
		public void CompleteStage ()
		{
			foreach (var item in stageData.steps) {
				if (item.mandatory && !item.isComplete)
					return;
			}

			stageData.isComplete = true;
			stageData.isActive = false;

			OnStageComplete.Invoke ();
			//for testing
			QuestTester.instance.OnStageComplete (stageData.stageID);
		}
	}
}