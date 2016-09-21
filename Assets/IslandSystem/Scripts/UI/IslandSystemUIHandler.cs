using UnityEngine;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Island System UI Handler.
	/// This class keep references of all UI objects and classes present in Island System.
	/// </summary>
	public class IslandSystemUIHandler : MonoBehaviour {

        /*public static IslandSystemUIHandler Instance
		{
			get
			{
				return instance;
			}
		}*/

//		static IslandSystemUIHandler instance;

		public enum PanelState
		{
			None,
			IslandInformation,
			ClearArea,
            MapSpecial,
            VideoTrailer
		}

		public GameObject islandInformationObject;
		public GameObject clearMapAreaObject;
        public GameObject mapSpecialObject;
        public GameObject videoTrailerPanel;

		void Awake ()
		{
//			if (instance == null)
//			{
//				instance = this;
//			}
//			else
//			{
//				DestroyImmediate (this);
//			}
		}

		// Use this for initialization
		void Start () {
			
		}
		
        /// <summary>
        /// Open the user interface panel.
        /// This method instatiate the panel and show in the screen.
        /// </summary>
        /// <param name="state">enum panel state (IslandSystemUIHandler.PanelState).</param>
		public void SwitchUIPanel (PanelState state)
		{
			//Disable the map camera control.
			MapCamera.Instance.ToggleCameraControl (false);

			switch (state)
			{
				case PanelState.IslandInformation:
					{
						GameObject go = Instantiate (islandInformationObject);
						go.transform.SetParent (this.transform, false);
						go.SetActive (true);
					}
					break;

				case PanelState.ClearArea:
					{
						GameObject go = Instantiate (clearMapAreaObject);
						go.transform.SetParent (this.transform, false);
						go.SetActive (true);
					}
					break;

                case PanelState.MapSpecial:
                    {
                        GameObject go = Instantiate (mapSpecialObject);
                        go.transform.SetParent (this.transform, false);
                        go.SetActive (true);
                    }
                    break;
                case PanelState.VideoTrailer:
                    {
                        GameObject go = Instantiate (videoTrailerPanel);
                        go.transform.SetParent (this.transform, false);
                        go.SetActive (true);
                    }
                    break;
			}
		}
	}
}
