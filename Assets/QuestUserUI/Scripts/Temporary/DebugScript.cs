using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using Poptropica2.QuestUI;
using System.Collections.Generic;

/// <summary>
/// This class is only for Debug use
/// It'll generate quest log based on UI slider component count value
/// </summary>
public class DebugScript : MonoBehaviour {

	public QuestJournalPopup questLogPopup;

	private int openQuestCount = 0;
	private int completedQuestCount = 0;

	/// <summary>
	/// Calling this method on debug button click 
	/// Temporarily creating Quest data here and passing to quest log popup.
	/// </summary>
	/// <param name="isCompleted">If set to <c>true</c> is completed.</param>
	public void ShowNotification(bool isCompleted)
	{
		if(!isCompleted)
		{
			openQuestCount++;

			Quest newQuest = new Quest ("Quest "+openQuestCount,openQuestCount,"Quest description "+openQuestCount,isCompleted);
			questLogPopup.AddQuest (newQuest);
		}
		else
		{
			if(openQuestCount > 0 && openQuestCount != completedQuestCount)
			{
				completedQuestCount++;
				questLogPopup.UpdateQuest(completedQuestCount);
			}
		}
	}
}
