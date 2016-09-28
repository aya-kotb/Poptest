using UnityEngine;
using System.Collections;
using Poptropica2.MDSModule;
using Poptropica2;

namespace Poptropica2.APIVersionSystem
{
    /// <summary>
    /// This class will get call from ServiceManager
    /// 
    /// This class is use for checking the version of client and server,
    /// that both systems are running the same version of Poptropica.
    /// If the version is not same in both client and server then this class will
    /// restart web client to match the server, and On mobile platforms, it will prompt the user to update the app via their respective app store.
    /// </summary>
    public class APIVersionSystem : IService
    {

        MDSManager mdsManager;
        private const string UpdateMessage = "Please update your game to continue being able to buy items and save your game to our servers.";
        private const float longInterval = 3600f;   // 1 Hour
        private const float shortInterval = 120f;   // 2 Minutes

        float interval;
        /// <summary>
        /// Initializes the services
        /// </summary>
        void Awake ()
        {
            DontDestroyOnLoad(this.gameObject);

            SAMApplication.mainInstance.AddService("MDSManager", new MDSManager());
            mdsManager = SAMApplication.mainInstance.GetService<MDSManager>();
        }

        // Use this for initialization
        void Start ()
        {
            interval = longInterval;
            // Check for the version at regular intervals
            InvokeRepeating("GetServerVersion", 0, interval);
        }

        /// <summary>
        /// Gets the server version.
        /// This method calls the server api to get server current version.
        /// </summary>
        void GetServerVersion ()
        {
            GameSparksManager.Instance().GetServerVersion(HandleonGetServerVersion, HandleonRequestFailed);
        }
        
        void HandleonGetServerVersion (string version, System.DateTime versionDate)
        {
            Debug.Log(versionDate);
            int compareDateTime = System.DateTime.Now.CompareTo(versionDate);

            if (compareDateTime < 0)
            {
                //Update vesrion date is greater than current date.
                PopUpMessage(versionDate.ToString());
            }
        }
        
        void HandleonRequestFailed (GameSparksError error)
        {
            switch (error.errorMessage)
            {
                case GameSparksErrorMessage.invalid_version:
                    PopUpMessage(UpdateMessage, true);
                    break;
                case GameSparksErrorMessage.incompatible_protocol_version:
                    PopUpMessage(error.errorString, true);
                    break;
                // TODO:: for expected updated.
                /*case Update expected at 0415 GMT
                    ChangeIntervalTime (shortInterval); //Changing interval time to 2 minutes.
                    break;*/

                default:
                    break;
            }

        }

        /// <summary>
        /// Changes the interval time for checking the version at regular intervals.
        /// </summary>
        /// <param name="time">float Time in seconds.</param>
        void ChangeIntervalTime (float time)
        {
            interval = time;

            // Cancel previous invoke method
            CancelInvoke("GetServerVersion");

            // Invoke method in new intrval time....
            InvokeRepeating("GetServerVersion", interval, interval);
        }

        /// <summary>
        /// Pops up message.
        /// This method will Display the message.
        /// </summary>
        /// <param name="msg">Message to be displayed.</param>
        /// <param name="addButton">If set to <c>true</c>Pop up message contain a Button.</param>
        void PopUpMessage (string msg, bool addButton = false)
        {
            MDSWindow newWindow = mdsManager.CreateWindow();
            newWindow.SetTitleText("Version Control");
            newWindow.SetContentText(msg);
            newWindow.SetTitleCloseButton (true);

            if (addButton == true)
            {
                #if UNITY_WEBGL
                newWindow.AddButton ("Restart", HandleCallback);
                #elif UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID
                newWindow.AddButton ("Update", HandleCallback);
                #endif
            }

            mdsManager.AddContentsToWindow(newWindow);
        }

        void HandleCallback ()
        {
            #if UNITY_WEBGL
            // RESTART APP
            #elif UNITY_IOS || UNITY_IPHONE
            // Open Apple App store url...
            //Application.OpenURL ();
            #elif UNITY_ANDROID
            // Open Android App store...
            //Application.OpenURL ();
            #endif
        }
    }
}
    