using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Poptropica2.QuestUI;

/// <summary>
/// Used to assign dummy episode data.
/// Used for IslandOverview scene. 
/// </summary>
public class DebugIslandData : MonoBehaviour 
{
	public EpisodesManager episodeManager;
	// Used it for debugging purpose only. Temporarily holding Quest details from Mockclass
	public List<Episode>  episodeDetails = new List<Episode> ();

	void OnEnable()
	{
		CreateTestQuestData ();
	}

	/// <summary>
	/// Temporarily creating our own Episode data to demonstrate.
	/// It has to come from Quest system.
	/// </summary>
	public void CreateTestQuestData() 
	{
		episodeDetails.Clear();
		int randomEpisodeCount = UnityEngine.Random.Range (1,6);
		for(int i = 0; i < randomEpisodeCount; i++)
		{
			Episode newEpisode = new Episode ();

			newEpisode.episodeTitle = "Episode Title" + i;
			newEpisode.description = "Episode Description " + i;

			newEpisode.progress = UnityEngine.Random.Range (0.0f,1.0f);
			newEpisode.difficultyLevel = UnityEngine.Random.Range (1,3);
			newEpisode.medallionCount = UnityEngine.Random.Range (0,3).ToString();

			episodeDetails.Add (newEpisode);
		}

		episodeManager.AddEpisodePanels (episodeDetails);
	} 
}
