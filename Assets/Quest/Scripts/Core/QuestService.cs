using UnityEngine;
using System.Collections;
using Poptropica2;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Poptropica2.QuestSystem
{
	public class QuestService : IService
	{
		private Transform questParentTransform;
		//All quest states and state machines will be stores inside this parent
		public Transform QuestParentTransform {
			get {
				if (questParentTransform == null) {
					questParentTransform = new GameObject ("QuestParent").transform;
				}
				return questParentTransform;
			}
		}

		private List<QuestData> questProgress;
		//Dictionary of all quest state machines with quest id as key
		private Dictionary<string,Quest> questDict;

		/// <summary>
		/// Adds the new quest to the quest progress
		/// </summary>
		public void AddNewQuest (QuestData questData)
		{
			GameObject newGO = new GameObject ("Quest" + questData.questID);
			newGO.transform.SetParent (questParentTransform);
			Quest questStateMachine = newGO.AddComponent<Quest> ();
			questStateMachine.updateMode = StateMachine.StateUpdateMode.EveryFrame;
			if (questDict == null)
				questDict = new Dictionary<string, Quest> ();
			questDict.Add (questData.questID, questStateMachine);

			questData.isActive = true;
			if (questProgress == null)
				questProgress = new List<QuestData> ();
			if (!questProgress.Contains (questData)) {
				questProgress.Add (questData);
				questStateMachine.CreateStates (questData);
			}
		}

		/// <summary>
		/// Removes the quest 
		/// </summary>
		public void RemoveQuest (string questID)
		{
			if (questProgress != null && questProgress.Count > 0) {
				QuestData quest = questProgress.Find (t => t.questID == questID);
				if (quest != null) {
					questProgress.Remove (quest);
					if (questDict.ContainsKey (questID)) {
						questDict [questID].RemoveQuest ();
						questDict.Remove (questID);
					}
				}
			}
		}

		/// <summary>
		/// Resets the whole current quest Progress
		/// </summary>
		public void ResetProgress ()
		{
			if (questProgress != null) {
				questProgress.Clear ();   
			}
		}

		/// <summary>
		/// Saves the current quest progress to specified file
		/// </summary>
		public void Save ()
		{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (Application.persistentDataPath + "/savedQuests.gd"); //you can call it anything you want
			Debug.Log (Application.persistentDataPath);
			bf.Serialize (file, questProgress);
			file.Close ();
		}

		/// <summary>
		/// Load quest progress from specified file
		/// </summary>
		public void Load ()
		{
			if (File.Exists (Application.persistentDataPath + "/savedQuests.gd")) {
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (Application.persistentDataPath + "/savedQuests.gd", FileMode.Open);
				questProgress = (List<QuestData>)bf.Deserialize (file);
				file.Close ();
			}
		}

		public void ShowInspectorUI ()
		{
			#if UNITY_EDITOR
			if (GUILayout.Button ("Save"))
				Save ();
			if (GUILayout.Button ("Load"))
				Load ();
			#endif
		}

        public void StartService(SAMApplication application)
        {
        }

        public void StopService(SAMApplication application)
        {
        }

        public void Configure(ServiceConfiguration config)
        {

        }

	}
}