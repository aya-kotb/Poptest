using UnityEngine;
using System;
using System.Collections;
using Poptropica2.MDSModule;
using Poptropica2;

namespace Poptropica2.APIVersioningSystem
{
    /// <summary>
    /// API Versioning Service
    /// To setup this class, add it to an empty gameobject.
    /// And attach VersioningSystemPopup prefab to popupMessagePanel GameObject.
    /// 
    /// This class is use for checking the version of client and server,
    /// that both systems are running the same version of Poptropica.
    /// If the version is not same in both client and server then this class will
    /// restart web client to match the server, and On mobile platforms, it will prompt the user to update the app via their respective app store.
    /// </summary>
    public class APIVersioningSystem : MonoBehaviour, IService
    {

        public GameObject popupMessagePanel;
        public string appStoreLinkiOS = "https://www.google.com";
        public string appStoreLinkAndroid = "https://www.google.com";
        public int sceneToLoad = 0;
        public float shortInterval = 60f;   // 1 Minutes


        private const string UpdateMessage = "Please update your game to continue being able to buy items and save your game to our servers.";
        private const float longInterval = 3600f;   // 1 Hour
        private const string updateStatusPending = "pending";

        private bool publishPendingChecked = false;
        private const string invokeMethod = "GetServerVersion";
        MDSManager mdsManager;

        /// <summary>
        /// Initializes the services
        /// </summary>
        void Awake()
        {
            // Using MDSManager for UI Popup.
            SAMApplication.mainInstance.AddService("MDSManager", new Poptropica2.MDSModule.MDSManager());
            mdsManager = SAMApplication.mainInstance.GetService<MDSManager>();

            if (CheckInstance() == false)
            {
                return;
            }

            // Should not destroy this gamobject if need to be called every.
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// Start API Server Verson check.
        /// </summary>
        void Start ()
        {
            GetServerVersion();
        }

        /// <summary>
        /// Gets the server version.
        /// This method calls the server api to get server current version.
        /// </summary>
        void GetServerVersion ()
        {
            GameSparksManager.Instance().GetServerVersion(HandleonGetServerVersion, HandleonRequestFailed);
        }

        /// <summary>
        /// This method is the callback from GameSparks api
        /// which receives the server version and version time and date.
        /// </summary>
        /// <param name="version">Version string, server version.</param>
        /// <param name="versionDate">Version date and time.</param>
        void HandleonGetServerVersion (ServerVersionResponse serverVersion)
        {
            Debug.Log("created "+serverVersion.created);
            Debug.Log("currentVersion "+serverVersion.currentVersion);
            Debug.Log("maxVersion "+serverVersion.maxVersion);
            Debug.Log("minVersion "+serverVersion.minVersion);
            Debug.Log("published "+serverVersion.published);
            Debug.Log("published "+serverVersion.status);
            Debug.Log("published "+serverVersion.message);

            if (serverVersion.status.Contains (updateStatusPending))
            {
                if (publishPendingChecked == false)
                {
                    publishPendingChecked = true;
                    string message = serverVersion.published + "\n" + serverVersion.message;
                    PopUpMessage(message, false);
                }
                InvokeAPIVersionCheck (shortInterval);
            }
            else
            {
                InvokeAPIVersionCheck(longInterval);
            }
        }

        /// <summary>
        /// This method is the callback from GameSparks api on request failed.
        /// which receive error type and error message.
        /// </summary>
        /// <param name="error">GameSparksError, contain enum and string errorString.</param>
        void HandleonRequestFailed (GameSparksError error)
        {
            Debug.Log(error.errorMessage);
            Debug.Log(error.errorString);

            if (error.errorMessage == GameSparksErrorMessage.invalid_version)
            {
                // When version is different and need updation.
                // Which will restart the game or redirect to app store for update.
                PopUpMessage(UpdateMessage, true);
            }
            else
            {
                InvokeAPIVersionCheck(longInterval);
            }
        }

        /// <summary>
        /// Invokes the APIVersionCheck after given time.
        /// </summary>
        /// <param name="time">float Time in seconds.</param>
        public void InvokeAPIVersionCheck (float time)
        {
            // Cancel previous invoke method
            if (IsInvoking(invokeMethod) == true)
            {
                CancelInvoke("GetServerVersion");
            }
            // Invoke the method invokeMethod name in time seconds.
            Invoke(invokeMethod, time);
        }

        /// <summary>
        /// Pops up message.
        /// This method will Display the message.
        /// </summary>
        /// <param name="msg">Message to be displayed.</param>
        /// <param name="addButton">If set to <c>true</c>Pop up message contain a Button.</param>
        public void PopUpMessage (string msg, bool isUpdateMsg = false)
        {
            string buttonText = "";
            GameObject go = Instantiate(popupMessagePanel);
            APIVersioningSytemPopup popup = go.GetComponent<APIVersioningSytemPopup>();
            popup.HandleInstantiatedPrefab();

            if (isUpdateMsg == true)
            {
                #if UNITY_WEBGL
                popup.DisplayPopup(msg, "Update Alert!", false, true, UpdateCallback);
                #elif UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID
                popup.ShowPopup(msg, "Update Alert!", false, true, UpdateCallback);
                #endif
            }
            else
            {
                popup.DisplayPopup(msg, "New Version!", true, false, null);
            }
        }

        /// <summary>
        /// This method will check other gameobject which contain this instance.
        /// If its already there then no need to instanciate this object.
        /// </summary>
        /// <returns><c>true</c>, if there is no other object of same instance, <c>false</c> if there is other object of same instance..</returns>
        bool CheckInstance ()
        {
            if (GameObject.FindObjectOfType<APIVersioningSystem>().gameObject.name == "~APIVersioningService")
            {
                DestroyImmediate(this.gameObject);
                return false;
            }

            this.gameObject.name = "~APIVersioningService";
            return true;
        }

        /// <summary>
        /// Is a callback method which is called when update/restart button is pressed.
        /// </summary>
        void UpdateCallback ()
        {
            #if UNITY_WEBGL
            // RESTART APP
            UnityEngine.SceneManagement.SceneManager.LoadScene((sceneToLoad));
            #elif UNITY_IOS || UNITY_IPHONE
            // Open Apple App store url...
            Application.OpenURL (appStoreLinkiOS);
            #elif UNITY_ANDROID
            // Open Android App store...
            Application.OpenURL (appStoreLinkAndroid);
            #endif
        }

        #region Service Interface implementation
        public void ShowInspectorUI ()
        {

        }
        #endregion
    }
}
    