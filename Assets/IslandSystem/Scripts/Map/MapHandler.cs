using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Map Handler.
	/// This class will handle all objects belongs to map
	/// and keep the information of all items in map.
	/// </summary>
	public class MapHandler : MonoBehaviour {

		public Island[] island;
		public Cloud[] cloud;
		public TreasureChest[] chest;

		public MapItem currentItem;
		public MapItem homeIsland;

		// Use this for initialization
		void Start () {
			InitializeMapItems ();
		}

		[System.Serializable]
		public class IslandDetail
		{
			public int id;			// Island ID
			public int difficulty;	// Number od difficulty.
			public int trophy;		// Total trophy achieved.
			public string name;		// Name of the island.
			public float progress;	// Progress made in the island.
		}

		public class CloudDetail
		{
			public int id;			// Cloud ID
			public int value;		// Coin value to clear the area.
			public bool canClear;	// Whether the cloud can be cleared by using coins.
		}

		public class TreasureChestDetail
		{
			public enum ChestType
			{
				Ads,
				Treasure,
				Quest
			}

			public int id;			// Chest ID
			public ChestType type;	// Type of rewards inside chest.
		}

		public void InitializeMapItems ()
		{
			List<IslandDetail> islandDetailList = MapDetails.GetAvailableIslands ();
			for (int i = 0; i < islandDetailList.Count; i++)
			{
				if (GetIsland (islandDetailList[i].id) != null)
				{
					GetIsland (islandDetailList [i].id).InitializeMapItem (islandDetailList [i]);
				}
			}
		}

		Island GetIsland (int id)
		{
			for (int i = 0; i < island.Length; i++)
			{
				if (island[i].islandID == id)
				{
					return island [i];
				}
			}

			return null;
		}

//		Cloud GetCloud (int id)
//		{
//			
//		}
	}
}
