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
    /// 
    /// This class is use for checking the version of client and server,
    /// that both systems are running the same version of Poptropica.
    /// If the version is not same in both client and server then this class will
    /// restart web client to match the server, and On mobile platforms, it will prompt the user to update the app via their respective app store.
    /// </summary>
    public class APIVersioningService : MonoBehaviour, IService
    {

        #if UNITY_IPHONE || UNITY_IOS
        private const string appStoreLink = "https://www.google.com";
        #elif UNITY_ANDROID
        private const string appStoreLink = "https://www.google.com";
        #endif
        private const string UpdateMessage = "Please update your game to continue being able to buy items and save your game to our servers.";
        private const float longInterval = 3600f;   // 1 Hour
        private const float shortInterval = 120f;   // 2 Minutes

        private const string invokeMethod = "GetServerVersion";
        MDSManager mdsManager;

        /// <summary>
        /// Initializes the services
        /// </summary>
        void Awake()
        {
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
            InvokeAPIVersionCheck(longInterval);
        }

        /// <summary>
        /// This method is the callback from GameSparks api on request failed.
        /// which receive error type and error message.
        /// </summary>
        /// <param name="error">GameSparksError, contain enum and string errorString.</param>
        void HandleonRequestFailed (GameSparksError error)
        {
            switch (error.errorMessage)
            {
                case GameSparksErrorMessage.invalid_version:
                    // When version is different and need updation.
                    // Which will restart the game or redirect to app store for update.
                    PopUpMessage(UpdateMessage, true);
                    break;
                // TODO:: for expected updated.
                /*case Update expected at 0415 GMT
                    InvokeAPIVersionCheck (shortInterval); //Changing interval time to 2 minutes.
                    break;*/

                default:
                    InvokeAPIVersionCheck (longInterval);
                    break;
            }

        }

        /// <summary>
        /// Invokes the APIVersionCheck after given time.
        /// </summary>
        /// <param name="time">float Time in seconds.</param>
        public void InvokeAPIVersionCheck (float time)
        {
            Debug.Log("InvokeAPIVersionCheck "+time);
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
        void PopUpMessage (string msg, bool addButton = false)
        {
            if (mdsManager == null)
            {
                mdsManager = SAMApplication.mainInstance.GetService<MDSManager>();
            }

            MDSWindow newWindow = mdsManager.CreateWindow();
            newWindow.SetTitleText("Version Control");
            newWindow.SetContentText(msg);
            newWindow.SetTitleCloseButton (true);

            if (addButton == true)
            {
                #if UNITY_WEBGL
                newWindow.AddButton ("Restart", UpdateGameCallback);
                #elif UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID
                newWindow.AddButton ("Update", HandleCallback);
                #endif
            }

            mdsManager.AddContentsToWindow(newWindow);
        }

        /// <summary>
        /// This method will check other gameobject which contain this instance.
        /// If its already there then no need to instanciate this object.
        /// </summary>
        /// <returns><c>true</c>, if there is no other object of same instance, <c>false</c> if there is other object of same instance..</returns>
        bool CheckInstance ()
        {
            if (GameObject.FindObjectOfType<APIVersioningService>().gameObject.name == "~APIVersioningService")
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
        void UpdateGameCallback ()
        {
            #if UNITY_WEBGL
            // RESTART APP
            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            #elif UNITY_IOS || UNITY_IPHONE
            // Open Apple App store url...
            Application.OpenURL (appStoreLink);
            #elif UNITY_ANDROID
            // Open Android App store...
            Application.OpenURL (appStoreLink);
            #endif
        }

        #region Service Interface implementation
        public void ShowInspectorUI ()
        {

        }
        #endregion
    }
}
    