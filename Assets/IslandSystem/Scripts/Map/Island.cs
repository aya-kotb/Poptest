using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Island class will hold the island information.
	/// </summary>
	public class Island : MapItem {

		public int islandID;
		public Text islandNameText;
		public MapHandler.IslandDetail islandDetail;

		void Awake () {
			itemID = islandID;
		}

		// Use this for initialization
		void Start () {
			InitializeComponent ();
		}

		public override void InitializeComponent ()
		{
			base.InitializeComponent ();
		}

		public override void OnClickItem ()
		{
			if (canSelect == false)
			{
				return;
			}

			ViewIsland ();
		}

		public override void InitializeMapItem (object islandDetail)
		{
			this.islandDetail = islandDetail as MapHandler.IslandDetail;
			islandNameText.text = this.islandDetail.name;
			base.InitializeMapItem (islandDetail);
		}

		/// <summary>
		/// Views the detail information of island.
		/// Create the transaction.
		/// </summary>
		void ViewIsland ()
		{
			IslandSystemManager.Instance.mapHandler.currentItem = this;
			IslandSystemUIHandler.Instance.SwitchUIPanel (IslandSystemUIHandler.PanelState.IslandInformation);
		}

		/// <summary>
		/// Get Character ID is a dummy function for getting character id
		/// which we need while calling Island api.
		/// </summary>
		void GetCharacterID ()
		{
			GameSparksManager.Instance ().Authenticate ("sujil_01", "12345", HandleonAuthSuccess, HandleonAuthFailed);
		}

		void HandleonAuthSuccess (AuthResponse authResponse)
		{
//			GameSparksManager.Instance ().CreateCharacter ("")
		}

		void HandleonAuthFailed (AuthFailed error)
		{
			
		}

	}
}
