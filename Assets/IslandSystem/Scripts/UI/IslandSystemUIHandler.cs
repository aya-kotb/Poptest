using UnityEngine;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Island System UI Handler.
	/// This class keep references of all UI objects and classes present in Island System.
	/// </summary>
	public class IslandSystemUIHandler : MonoBehaviour {

		public static IslandSystemUIHandler Instance
		{
			get
			{
				return instance;
			}
		}

		static IslandSystemUIHandler instance;

		public enum PanelState
		{
			None,
			IslandInformation,
			ClearArea
		}

		public GameObject islandInformationObject;
		public GameObject clearMapAreaObject;

		PanelState currentPanelState;

		void Awake ()
		{
			if (instance == null)
			{
				instance = this;
			}
			else
			{
				DestroyImmediate (this);
			}
		}

		// Use this for initialization
		void Start () {
			
		}
		
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
			}

			currentPanelState = state;
		}
	}
}
