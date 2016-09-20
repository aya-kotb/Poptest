using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Map Details is a dummy class for getting inforamtion of items.
	/// This class contains details of all items (Island, Treasure Chest, Cloud etc) present inside map.
	/// </summary>
	public static class MapDetails
	{
		public static List<MapHandler.IslandDetail> GetAvailableIslands ()
		{
			// Fetching island details
			List<MapHandler.IslandDetail> islandDetailList = new List<MapHandler.IslandDetail> ();
			for (int i = 0; i < 4; i++)
			{
				MapHandler.IslandDetail islandDetail = new MapHandler.IslandDetail ();
				islandDetail.id = (i + 1);
				islandDetail.name = GetIslandName (islandDetail.id);
				islandDetail.difficulty = (3 - i);
				islandDetail.trophy = Random.Range (0, 10);
				islandDetail.progress = Random.Range (5, 90);

				islandDetailList.Add (islandDetail);
			}


			return islandDetailList;
		}

		public static List<MapHandler.CloudDetail> GetAvailableClouds ()
		{
			// Fetching clouds details
			List<MapHandler.CloudDetail> cloudDetailList = new List<MapHandler.CloudDetail> ();
			//For timing two cloud is enabled...
			//5 7
			MapHandler.CloudDetail cloudDetailOne = new MapHandler.CloudDetail ();
			cloudDetailOne.id = 5;
			cloudDetailOne.value = 1500;
			cloudDetailOne.canClear = true;
			cloudDetailList.Add (cloudDetailOne);

			MapHandler.CloudDetail cloudDetailTwo = new MapHandler.CloudDetail ();
			cloudDetailTwo.id = 7;
			cloudDetailTwo.value = 600;
			cloudDetailTwo.canClear = true;
			cloudDetailList.Add (cloudDetailTwo);

			return cloudDetailList;
		}

		public static List<MapHandler.TreasureChestDetail> GetAvailableChest ()
		{
			// Fetching clouds details
			List<MapHandler.TreasureChestDetail> cloudDetailList = new List<MapHandler.TreasureChestDetail> ();
			//For timing two cloud is enabled...
			//5 7
			MapHandler.TreasureChestDetail chestDetailOne = new MapHandler.TreasureChestDetail ();
			chestDetailOne.id = 1;
			chestDetailOne.type = MapHandler.TreasureChestDetail.ChestType.Treasure;
			cloudDetailList.Add (chestDetailOne);

			MapHandler.TreasureChestDetail chestDetailTwo = new MapHandler.TreasureChestDetail ();
			chestDetailTwo.id = 2;
			chestDetailTwo.type = MapHandler.TreasureChestDetail.ChestType.Ads;
			cloudDetailList.Add (chestDetailTwo);

			return cloudDetailList;
		}

		static string GetIslandName (int id)
		{
			switch (id)
			{
				case 1:
					return "Home Island";
				case 2:
					return "24 Carrot";
				case 3:
					return "Crisis Caverns";
				case 4:
					return "Lost Pirates";
				default:
					return "";
			}
		}
	}
}
