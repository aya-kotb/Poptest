using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Poptropica2.QuestUI 
{
	/// <summary>
	/// Used for to keep reference of the restart popup buttons 
	/// </summary>
	public class RestartProgressPopup : MonoBehaviour {

		public Button cancelButton; 
		public Button OkButton; 
		public BoxCollider2D swipeCollider; //Assign swipe collider to restrict swipe

		/// <summary>
		/// This method will call upon cancel button click.
		/// </summary>
		public void OnCancelButtonClicked()
		{
			swipeCollider.enabled = true;
			gameObject.SetActive (false);
		}
	}
}

