using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Island class will hold the island information.
	/// </summary>
	public class Island : MapItem {

        public string islandID;
		public Text islandNameText;
		public MapHandler.IslandDetail islandDetail;
	
		void Awake () {
			itemID = islandID;
		}

		// Use this for initialization
		void Start () {
			InitializeComponent ();
		}

        /// <summary>
        /// This method used for Initialising the variables and properties.
        /// </summary>
		public override void InitializeComponent ()
		{
			base.InitializeComponent ();
		}

        /// <summary>
        /// This method is called when island is selected.
        /// </summary>
		public override void OnClickItem ()
		{
			if (canSelect == false)
			{
				return;
			}

            MoveBlimp ();
		}

        /// <summary>
        /// Initializes the map item.
        /// This method is used for storing the information or details of item
        /// </summary>
        /// <param name="info">Pass the Information of item.</param>
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
		public void ViewIsland ()
		{
            islandSystemManager.mapHandler.currentItem = this;
            islandSystemManager.islandSystemUI.SwitchUIPanel (IslandSystemUIHandler.PanelState.IslandInformation);
		}

        /// <summary>
        /// Moves the blimp to the selected island position.
        /// </summary>
        void MoveBlimp ()
        {
            islandSystemManager.mapHandler.blimp.MoveBlimp(this);
        }
	}
}
