using UnityEngine;
using System;
using System.Collections;
using Poptropica2;

namespace Poptropica2.APIVersioningSystem
{
    /// <summary>
    /// This class is used for checking the version of client and server,
    /// that both systems are running the same version of Poptropica.
    /// If the version is not same in client with server then this class will
    /// restart web client to match the server, and On mobile platforms, it will prompt the user to update the app via respective app stores.
    /// </summary>
	public class APIVersioningService : MonoBehaviour, IService
    {
		// Interval is in seconds
		private float shortInterval = 60f;	
		private float longInterval = 3600f;	

		private const string updateMessage = "Please update your game to continue being able to buy items and save your game to our servers.";
		private const string invokeMethod = "GetServerVersion";

        private bool publishPendingChecked = false;

		private APIVersioningServiceConfig config;
        
		/// <summary>
        /// Initializes the services
        /// </summary>
        void Awake()
        {
			if(SAMApplication.mainInstance.GetService<APIVersioningService>() == null)
			{
				DontDestroyOnLoad(this.gameObject);
			}
			else
			{
				DestroyImmediate(this.gameObject);
			}

			SAMApplication.mainInstance.GetService<APIVersioningService>();
        }

        /// <summary>
        /// Start API Server Version check.
        /// </summary>
        void Start ()
        {
			InvokeAPIVersionCheck(1f);
        }

		/// <summary>
		/// Triggers when new scene got loaded.
		/// Since start method won't call again on DontDestroyOnLoad we are using this default unity method to start the API call again.
		/// </summary>
		void OnLevelWasLoaded()
		{
			InvokeAPIVersionCheck(10f);
		}

		/// <summary>
		/// Invokes the APIVersionCheck after given time.
		/// </summary>
		/// <param name="time">float Time in seconds.</param>
		public void InvokeAPIVersionCheck (float time)
		{
			// Cancels previous invoke method
			if (IsInvoking(invokeMethod) == true)
			{
				CancelInvoke(invokeMethod);
			}

			// Invoke the method with method name
			Invoke(invokeMethod, time);
		}

        /// <summary>
        /// This method calls the Gamespark api to get servers current version.
        /// </summary>
        void GetServerVersion ()
        {
            GameSparksManager.Instance().GetServerVersion(HandleOnGetServerVersion, HandleOnRequestFailed);
        }

        /// <summary>
		/// This method is the callback from GameSparks GetServerVersion api
        /// which receives the server version, version time and date.
        /// </summary>
        /// <param name="version">Version string, server version.</param>
        void HandleOnGetServerVersion (ServerVersionResponse serverVersion)
        {
			if (serverVersion.status.Contains (config.updateStatus))
            {
                if (publishPendingChecked == false)
                {
                    publishPendingChecked = true;
                    string message = serverVersion.published + "\n" + serverVersion.message;
                    PopupMessage(message, false);
                }
                InvokeAPIVersionCheck (shortInterval);
            }
            else
            {
				Debug.Log("<color=green>Success : Client and server running same version of poptropica! </color>");
                InvokeAPIVersionCheck(longInterval);
            }
        }

        /// <summary>
		/// This method is the callback from GameSparks GetServerVersion api on request failed.
        /// which receive error type and error message.
        /// </summary>
        /// <param name="error">GameSparksError, contain enum and string errorString.</param>
		/// <param name="serverVersion">ServerVersionResponse, Gives detail about serverVersion.</param>
		void HandleOnRequestFailed (GameSparksError error, ServerVersionResponse serverVersion)
        {
            Debug.Log(error.errorMessage);
            Debug.Log(error.errorString);

			// When the error message equals to invalid version, system will prompt the user to make an update or restart based on platform
            if (error.errorMessage == GameSparksErrorMessage.invalid_version)
            {
				PopupMessage(updateMessage, true);
            }
            else
            {
                InvokeAPIVersionCheck(longInterval);
            }
        }

        /// <summary>
        /// This method instantiates the popup and sets the information to it.
        /// </summary>
        /// <param name="msg">Message to be displayed.</param>
		/// <param name="isUpdateMsg">If set to <c>true</c>popup message will be updated</param>
        public void PopupMessage (string msg, bool isUpdateMsg = false)
        {
            string buttonText = "";
			GameObject go = Instantiate(config.popupMessagePanel);
            APIVersioningSystemPopup popup = go.GetComponent<APIVersioningSystemPopup>();
            popup.HandleInstantiatedPrefab();

            if (isUpdateMsg == true)
            {
				popup.DisplayPopup(msg, "New Version!", false, true, UpdateCallback);
            }
            else
            {
                popup.DisplayPopup(msg, "Update Alert!", true, false, null);
            }
        }

        /// <summary>
        /// It is a callback method which is called when update/restart button is pressed.
		/// If the app is running on WebGL platform then restart the app.
		/// If its running on iOS/Android platform then redirect to appropriate app stores
        /// </summary>
        void UpdateCallback ()
        {
#if UNITY_WEBGL
            // RESTART APP
			UnityEngine.SceneManagement.SceneManager.LoadScene(config.sceneIndexToRestart,UnityEngine.SceneManagement.LoadSceneMode.Single);
#elif UNITY_IOS || UNITY_IPHONE
            // Open Apple App store url...
			Application.OpenURL (config.appStoreLink_iOS);
#elif UNITY_ANDROID
            // Open Android App store...
			Application.OpenURL (config.appStoreLink_Android);
#endif
        }

        #region Service Interface implementation
		public void StartService (SAMApplication application)
        {
		}

		public void StopService(SAMApplication application)
		{
		}

		public void Configure(ServiceConfiguration serviceConfig)
		{
			config = serviceConfig as APIVersioningServiceConfig;
		}
        #endregion
    }
}
    