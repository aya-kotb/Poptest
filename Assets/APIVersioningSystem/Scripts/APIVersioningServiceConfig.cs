using UnityEngine;
using System.Collections;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Poptropica2.APIVersioningSystem
{
	/// <summary>
	/// API versioning service config class.
	/// Holds references to be used in API Versioning system
	/// </summary>
	public class APIVersioningServiceConfig : ServiceConfiguration {

        public GameObject popupMessagePanel;

		public string appStoreLink_iOS = "https://www.google.com";
		public string appStoreLink_Android = "https://www.google.com";
        public string checkConnectionUrl = "https://www.google.com";
		public string updateStatus = "pending";

        public float apiCallInterval = 10f;

		/// <summary>
		/// Returns the API versioning system service object.
		/// </summary>
		/// <returns>The newly created service instance.</returns>
		protected override IService GetServiceClass()
		{
			return GameObject.Find("APIVersionSystem").GetComponent<APIVersioningService>();
		}

		/// <summary>
		/// Shows the editor UI. This is used for the custom service editor. You must override this in each child class.
		/// </summary>
		public override void ShowEditorUI ()
		{
			GUILayout.Label("iOS Appstore Link :");
			if(appStoreLink_iOS == null) appStoreLink_iOS = "";
			appStoreLink_iOS = GUILayout.TextField(appStoreLink_iOS);

			GUILayout.Label("Android Appstore Link :");
			if(appStoreLink_Android == null) appStoreLink_Android = "";
			appStoreLink_Android = GUILayout.TextField(appStoreLink_Android);

			GUILayout.Label("Status string to compare for upcoming update :");
			if(updateStatus == null) updateStatus = "";
			updateStatus = GUILayout.TextField(updateStatus);

			GUILayout.Label("Waiting time to call Server API :");
            apiCallInterval = Convert.ToInt32(GUILayout.TextField(apiCallInterval.ToString()));

#if UNITY_EDITOR
			GUILayout.Label("Assign alert popup prefab here :");
			popupMessagePanel = EditorGUILayout.ObjectField(popupMessagePanel,typeof(GameObject),true) as GameObject;

			EditorUtility.SetDirty(this);
#endif
		}
	}
}
