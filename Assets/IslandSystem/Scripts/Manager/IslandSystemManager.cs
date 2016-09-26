using UnityEngine;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Island System Manager. This class will keeps all information of all others
	/// class, prefabs and objects of island system.
	/// </summary>
    public class IslandSystemManager : MonoBehaviour, IService
	{
		public Camera mapCamera;
		public IslandSystemUIHandler islandSystemUI;
		public MapHandler mapHandler;

        //For temporary using CharacterInfo class to fetch the character ID
        public CharacterInfo characterInfo;

		void Awake ()
		{
            SAMApplication.mainInstance.AddService("IslandSystemManager", this);
		}
		
		// Use this for initialization
		void Start () {
			characterInfo = new CharacterInfo ();

			//As we are using static map for now we can disable the camera control.
			MapCamera.Instance.ToggleCameraControl (false);
		}

		/// <summary>
		/// Loads the current selected island.
		/// This method Load the island scene according to the selected island.
		/// </summary>
		void LoadIsland ()
		{
			string sceneName = SceneName(mapHandler.currentItem.GetItemDetail<MapHandler.IslandDetail>().name);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
		}

        /// <summary>
        /// Visits the island.
        /// This method is called to GameSpark manager to register character
        /// for visited island
        /// </summary>
        public void VisitIsland ()
        {
            GameSparksManager.Instance().VisitIsland(
                characterInfo.characterID,
                mapHandler.currentItem.ID,
                HandleonIslandVisited,
                HandleonRequestFailed);
        }

        void HandleonIslandVisited (string island_id)
        {
            Debug.Log("Handle on Island Visited: " + island_id);
            LoadIsland();
        }

        void HandleonRequestFailed (GameSparksError error)
        {
            Debug.LogError("Handle on Request Failed: " + error.errorMessage);
        }

		string SceneName (string islandName)
		{
			switch (islandName)
			{
				case "Home Island":
					return "HomeIsland";
				case "24 Carrot":
					return "24Carrot";
				case "Crisis Caverns":
					return "CrisisCaverns";
				case "Lost Pirates":
					return "LostPirates";

				default:
					return "";
			}
		}

        /// <summary>
        /// ShowInspectorUI: This is the implemenation method for the IService interface
        /// </summary>
        public void ShowInspectorUI()
        {

        }
	}
}
