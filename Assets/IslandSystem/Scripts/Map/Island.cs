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
	
		void Awake ()
        {
			itemID = islandID;
		}

		void Start ()
        {
			InitializeComponent ();
		}

        /// <summary>
        /// This method used for Initialising the variables and properties.
        /// Initialises the component attached within this gameobject.
        /// </summary>
		public override void InitializeComponent ()
		{
			base.InitializeComponent ();
		}

        /// <summary>
        /// This method is triggered when island is selected.
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
        /// <param name="info">object Pass the Information of item if any details has to be stored in Map item.</param>
		public override void InitializeMapItem (object islandDetail)
		{
			this.islandDetail = islandDetail as MapHandler.IslandDetail;
			islandNameText.text = this.islandDetail.name;
			base.InitializeMapItem (islandDetail);
		}

		/// <summary>
		/// This method will call UI handler to Popup the message for Displaying the details of island.
		/// </summary>
		public void ViewIsland ()
		{
            islandSystemManager.mapHandler.currentItem = this;
            islandSystemManager.islandSystemUI.SwitchUIPanel (IslandSystemUIHandler.PanelState.IslandInformation);
		}

        /// <summary>
        /// Gives command to blimp to move towards the selected island position.
        /// </summary>
        void MoveBlimp ()
        {
            islandSystemManager.mapHandler.blimp.MoveBlimp(this);
        }
	}
}
