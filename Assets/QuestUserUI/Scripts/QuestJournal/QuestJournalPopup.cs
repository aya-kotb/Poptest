using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Poptropica2.QuestUI
{
	/// <summary>
	/// This class is used for generating the quest log & updating it in terms of UI
	/// It'll also handle the quest progress in UI
	/// </summary>
	public class QuestJournalPopup : MonoBehaviour
	{
		public GameObject questLogPrefab; //Assign quest log prefab
		public GameObject parentObject; //Assign quest log scroll panel content game object component 

		[HideInInspector]
		public List<GameObject> questLogList;

		public Slider islandQuestProgressbar; //Quest progress bar
		public Text progressBarPercentageText; //Quest progress percentage text 

		public QuestNotification questNotificationRef;

		/// <summary>
		/// Sets the quest progress value and progress text value.
		/// </summary>
		/// <value>The quest progress.</value>
		public int QuestProgress 
		{
			set 
			{ 
				islandQuestProgressbar.value = value;
				progressBarPercentageText.text = islandQuestProgressbar.value.ToString () + " %";
			}
		}

		/// <summary>
		/// Adds the Quest items into quest journal panel.
		/// Takes Quest as input parameter. 
		/// This parameter should be changed according to the quest system.
		/// </summary>
		/// <param name="questData">Quest data.</param>
		public void AddQuest (Quest questData)
		{
			//Create a new quest log object, and set the parent
			GameObject questLogObject = Instantiate (questLogPrefab) as GameObject; 

			//Get Quest Logs script 
			QuestLogs questLog = questLogObject.GetComponent<QuestLogs> ();
			questLog.QuestDetail = questData;

			questLogObject.transform.SetParent (parentObject.transform, false);
			questLogObject.SetActive (true);

			questLogList.Add (questLogObject);

			questNotificationRef.gameObject.SetActive (true);
			questNotificationRef.NotificationText = questData.description;
			questNotificationRef.NotificationIcon = questNotificationRef.questIcon;

			UpdateIslandQuestProgress(questLogList.Count,GetCompletedQuestCount());
		}

		/// <summary>
		/// Updates the questlog with the given Quest ID.
		/// </summary>
		/// <param name="questID">Quest I.</param>
		public void UpdateQuest(int questID)
		{
			QuestLogs questLog = GetQuestLogByQuestID(questID);
			if(questLog != null)
			{
				questLog.UpdateLog(true);	

				questNotificationRef.gameObject.SetActive (true);
				questNotificationRef.NotificationText = questLog.questDetail.description;
				questNotificationRef.NotificationIcon = questNotificationRef.completedQuestIcon;

				UpdateIslandQuestProgress(questLogList.Count, GetCompletedQuestCount());
			}
		}

		/// <summary>
		/// Returns the quest log object by comparing the given questID
		/// </summary>
		/// <returns>The quest log.</returns>
		/// <param name="questID">Quest I.</param>
		QuestLogs GetQuestLogByQuestID(int questID)
		{
			for(int i = 0; i < questLogList.Count; i++)
			{
				QuestLogs questLog = questLogList[i].GetComponent<QuestLogs>();
				if(questLog.questDetail.id == questID)
				{
					return questLog;
				}
			}
			return null;
		}

		/// <summary>
		/// Returns the number of completed quests from QuestLogList.
		/// </summary>
		/// <returns>The completed quest count.</returns>
		int GetCompletedQuestCount()
		{
			int completedQuestCount = 0;
			for(int i = 0; i < questLogList.Count; i++)
			{
				QuestLogs questLog = questLogList[i].GetComponent<QuestLogs>();
				if(questLog.questDetail.isCompleted)
				{
					completedQuestCount++;
				}
			}

			return completedQuestCount;
		}

		/// <summary>
		/// Updates the island quest progress based on total quest count on the island with the completed quest count.
		/// </summary>
		/// <param name="totalQuestCount">Total quest count.</param>
		/// <param name="completedQuestCount">Completed quest count.</param>
		void UpdateIslandQuestProgress(int totalQuestCount,int completedQuestCount) 
		{
			int percentage = (int)(((float)completedQuestCount / (float)totalQuestCount) * 100f);
			QuestProgress = percentage;
		}

		/// <summary>
		/// Disables the complete Quest log panel on close button press
		/// </summary>
		public void OnCloseButtonClicked ()
		{
			gameObject.SetActive (false);
		}
	}
}