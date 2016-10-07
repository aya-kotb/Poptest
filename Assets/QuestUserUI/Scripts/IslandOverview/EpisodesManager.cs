using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Poptropica2.QuestUI
{
	/// <summary>
	/// This class is used to manage the episode panels when the initial island screen is loaded.
	/// It manages number of episodes need to be shown in particular island.
	/// This requires an event system to set the episode count.
	/// </summary>
	public class EpisodesManager : MonoBehaviour
	{
		private int episodeCount = 0; //Number of episode panels

		[HideInInspector]
		public List<GameObject> episodeList; //Keep track of episode panels
		[HideInInspector]
		public List<GameObject> dotList; //Keep track of dot indicator prefab 

		public GameObject episodePrefab;//Assign episode panel prefab
		public GameObject dotPrefab; //Assign dot prefab
		public GameObject dotParentObject; //Assign dot parent gameobject
		public GameObject parentObject; //Episode panel parent
		public GameObject restartProgressPopup;
		public Image islandIcon; //Island icon image

		/// <summary>
		/// Sets a value indicating whether this instance island icon.
		/// </summary>
		/// <value><c>true</c> if this instance island icon; otherwise, <c>false</c>.</value>
		public Sprite IslandIcon 
		{ 
			set 
			{ 
				islandIcon.GetComponent<Image> ().sprite = value; 
			} 
		}

		/// <summary>
		/// Adding episode panel prefabs into canvas panels
		/// </summary>
		public void AddEpisodePanels (List<Episode> episodeData)
		{
			for (int i = 0; i < episodeData.Count; i++) 
			{
				//Create a new episode panel, and set the parent
				GameObject newEpisodePanel = Instantiate (episodePrefab) as GameObject;
				newEpisodePanel.transform.SetParent (parentObject.transform, false);
				episodeList.Add (newEpisodePanel);

				QuestReference questReference = newEpisodePanel.GetComponent<QuestReference> ();
				questReference.ActivateQuestDisplay (episodeData[i]);
				questReference.restartPopup = restartProgressPopup;

				//Create a new episode panel's dot prefab, and set the parent
				GameObject dotObject = Instantiate (dotPrefab) as GameObject;
				dotObject.transform.SetParent (dotParentObject.transform, false);
				dotList.Add (dotObject);

				//Assigning toggle group component to dot prefab 
				if (episodeCount != 1) 
				{
					dotObject.GetComponent<Toggle> ().group = dotObject.transform.parent.gameObject.GetComponent<ToggleGroup> ();
				}
				dotObject.SetActive (true);
			}
		}

		void OnDisable ()
		{
			//Clear the list before adding other
			if (episodeList != null) 
			{
				for (int i = 0; i < episodeList.Count; i++) 
				{
					Destroy (episodeList [i]);
					Destroy (dotList [i]);
				}

				episodeList.Clear ();
				dotList.Clear ();
			}
		}
		/// <summary>
		/// This method is called by close button
		/// </summary>
		public void OnCloseButtonClicked ()
		{
			Debug.Log ("Close button Clicked");
		}
	}
}
