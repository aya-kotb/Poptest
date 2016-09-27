using UnityEngine;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Island System UI Handler.
	/// This class keep references of all UI objects and classes present in Island System.
	/// </summary>
	public class IslandSystemUIHandler : MonoBehaviour {

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

        /// <summary>
        /// Open the user interface panel.
        /// This method instatiate the given input panel and display.
        /// </summary>
        /// <param name="state">enum panel state (IslandSystemUIHandler.PanelState).</param>
		public void SwitchUIPanel (PanelState state)
		{
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
