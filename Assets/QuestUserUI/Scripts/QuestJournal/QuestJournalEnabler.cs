using UnityEngine;
using System.Collections;

namespace Poptropica2.QuestUI 
{
	/// <summary>
	// This class used for enable quest log popup
	/// </summary>
	public class QuestJournalEnabler : MonoBehaviour
	{
		public GameObject questPopup;

		/// <summary>
		/// Enables popup panel of quest Journal
		/// </summary>
		public void OnQuestsButtonClicked()
		{
			questPopup.SetActive (true);
		}
	}
}
