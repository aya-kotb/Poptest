using UnityEngine;
using System.Collections;
using Poptropica2;
using System.Collections.Generic;
using System.Linq;

namespace Poptropica2.QuestSystem
{

	public class Quest : StateMachine
	{
		public QuestData questData;

		/// <summary>
		/// Creates the states i.e. QuestStage for specified quest
		/// </summary>
		/// <param name="questData">Info for this quest</param>
		public void CreateStates (QuestData questData)
		{
			this.questData = questData;
			List<StageData> stages = questData.stages;
			Dictionary<StageData,QuestStage> stagesDict = new Dictionary<StageData, QuestStage> ();

			foreach (var stage in stages) {
				GameObject newGO = new GameObject ("Stage" + stage.stageID);
				QuestStage questStage = newGO.AddComponent<QuestStage> ();
				newGO.transform.SetParent (transform);

				questStage.stageData = stage;
				questStage.OnStageComplete = CompleteQuest;
				questStage.links = new StateLink[1];
				questStage.links [0] = new StateLink ();

				stagesDict.Add (stage, questStage);
			}

			foreach (var item in stagesDict) {
				item.Value.links [0].linkedState = stagesDict.Values.FirstOrDefault (t => t.stageData.stageID == item.Key.nextStageID);
			}

			initialState = stagesDict.First (t => t.Key.isInitial).Value;
		}

		/// <summary>
		/// Check whether Quest is completed 
		/// called every time if any stage got completed
		/// </summary>
		public void CompleteQuest ()
		{
			foreach (var item in questData.stages) {
				if (!item.isComplete)
					return;
			}
			questData.isComplete = true;
			Debug.Log ("Quest " + questData.questID + " is completed"); 
		}

		/// <summary>
		/// On Quest removed from journal or current quest progress
		/// </summary>
		public void RemoveQuest ()
		{
			Destroy (this.gameObject);
		}
	}
}