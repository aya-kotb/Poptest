using UnityEngine;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Treasure chest holds the imformation of map specials item which
	/// contains Treasures, Ads or Special Quests.
	/// This Chest could be free, require coins or certain XP level to unlock
	/// </summary>
	public class TreasureChest : MapItem {

		public int chestID;

		// Use this for initialization
		void Start () {
			InitializeComponent ();
			itemID = chestID;
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

			OpenChest ();
		}

		public override void InitializeMapItem (object id)
		{
			base.InitializeMapItem (id);
		}

		/// <summary>
		/// Opens the treasures chest.
		/// </summary>
		void OpenChest ()
		{
			Debug.Log ("Open Chest");
		}
	}
}
