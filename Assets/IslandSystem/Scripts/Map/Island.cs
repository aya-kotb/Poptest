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

            MoveBlimp ();
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
            islandSystemManager.mapHandler.currentItem = this;
            islandSystemManager.islandSystemUI.SwitchUIPanel (IslandSystemUIHandler.PanelState.IslandInformation);
		}

        void MoveBlimp ()
        {
            islandSystemManager.mapHandler.blimp.MoveBlimp(transform, ViewIsland);
        }
	}
}
