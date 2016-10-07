using UnityEngine;
using System.Collections;
using Poptropica2;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;

namespace Poptropica2.QuestSystem
{

	/// <summary>
	/// Testing class for this module
	/// </summary>
	public class QuestTester : MonoBehaviour
	{
		public static QuestTester instance;
		public QuestData quest = new QuestData ();
		public List<PickUpItemStepData> pickUpStepDatas = new List<PickUpItemStepData> ();
		public List<ObjectiveStepData> objectiveStepDatas = new List<ObjectiveStepData> ();
		//for testing
		public GameObject ridePlaneBtn;
		//for testing
		public GameObject acquireObjectBtn;
		
		//for testing
		void Awake ()
		{
			instance = this;
			QuestService questService = new QuestService ();
			SAMApplication.mainInstance.AddService ("QuestService", questService);
			quest.stages [0].steps = pickUpStepDatas.ConvertAll (t => (StepData)t);
			quest.stages [1].steps = objectiveStepDatas.ConvertAll (t => (StepData)t);
			questService.AddNewQuest (quest);
		}

		public List<KeyValuePair<string,Action<string>>> listOfCallbacks = new List<KeyValuePair<string, Action<string>>> ();

		public void Listen (string stepType, Action<string> callback)
		{
			listOfCallbacks.Add (new KeyValuePair<string, Action<string>> (stepType, callback));
		}

		public void Call (string stepType, string objectID)
		{
			var list = listOfCallbacks.ToArray ();
			foreach (var item in list) {
				if (item.Key == stepType)
					item.Value.Invoke (objectID);
			}
		}

		public void Remove (string stepType, Action<string> callback)
		{
			listOfCallbacks.Remove (new KeyValuePair<string, Action<string>> (stepType, callback));
		}

		/// <summary>
		/// Button name currently set as same as step condition type
		/// </summary>
		public void OnButtonPressed (GameObject btn)
		{
			if (btn == ridePlaneBtn || btn == acquireObjectBtn)
				return;
			string stepType = btn.name.Contains ("Pick") ? "AcquireObject" : "CompleteObjective";
			if (listOfCallbacks.Exists (t => t.Key == stepType)) {
				btn.GetComponent<Button> ().interactable = false;
				btn.GetComponent<Image> ().color = Color.green;
				Call (stepType, btn.name);
			}
		}

		/// <summary>
		/// Called whenever any stage is completed
		/// </summary>
		public void OnStageComplete (string stageID)
		{
			GameObject targetBtn = null;
			if (stageID == "201") {
				targetBtn = acquireObjectBtn;
			} else if (stageID == "202") {
				targetBtn = ridePlaneBtn;
			}
			targetBtn.GetComponent<Button> ().interactable = false;
			targetBtn.GetComponent<Image> ().color = Color.green;
		}

	}
}