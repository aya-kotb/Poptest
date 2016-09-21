using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Scene load manager class.
	/// This class will setup automatically when its instance is callled.
	/// It will create a gamobject with name "SceneLoadManager".
	/// This class manage in loading the scene. And holds all the scene information.
	/// </summary>
    public class SceneLoadManager : MonoBehaviour {
		
        //#region Singleton
		/// <summary>
		/// A reference to the scene load manager object used for the singleton
		/// </summary>
		private static SceneLoadManager instance = null;
        /*
		/// <summary>
		/// Returns the current state of this class
		/// The singleton ensures that only one instance of the scene load manager exists.
		/// </summary>
		/// <value>The instance of SceneLoadManager.</value>
		public static SceneLoadManager Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject go = new GameObject("SceneLoadManager");
					instance = go.AddComponent<SceneLoadManager>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }
        
        #endregion*/

        public static string currentScene;
        public static string previousScene;

        void Awake ()
        {
			
		}

        public SceneLoadManager ()
        {
            if (instance != null)
            {
                DestroyImmediate(this);
            }
            else
            {
                instance = this;
                GameObject go = new GameObject("SceneLoadManager");
                instance = go.AddComponent<SceneLoadManager>();
                DontDestroyOnLoad(go);
            }
        }
		
		// Use this for initialization
		void Start () {
			
		}

        /// <summary>
		/// Loads the scene by its name in Build Settings.
		/// </summary>
		/// <param name="sceneName">Pass the Scene name.</param>
		public static void LoadScene(string sceneName)
		{
            instance.Load(sceneName);
		}

		/// <summary>
		/// Loads the scene asynchronously in the background.
		/// </summary>
		/// <returns>The scene async.</returns>
		/// <param name="sceneName">Pass the Scene name.</param>
		public static AsyncOperation LoadSceneAsync(string sceneName)
		{
            Scene(sceneName);
			AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
			CleanUp();
			return async;
		}

		/// <summary>
		/// Loads the scene with delay by its name in Build Settings.
		/// </summary>
		/// <param name="sceneBuildIndex">Pass the Scene build index number.</param>
		/// <param name="delay">Delay in seconds before loading.</param>
		public static void LoadSceneWithDelay (string sceneName, float delay)
		{
            instance.StartCoroutine(Load(sceneName, delay));
		}

		/// <summary>
		/// Load the specified sceneBuildIndex and time.
		/// </summary>
		/// <param name="sceneBuildIndex">Scene build index.</param>
		/// <param name="time">Time.</param>
		private static IEnumerator Load(string sceneName, float time)
		{
            Scene(sceneName);
			yield return new WaitForSeconds(time);
            instance.Load(sceneName);
		}

		/// <summary>
		/// Load the specified sceneBuildIndex.
		/// </summary>
		/// <param name="sceneBuildIndex">Scene build index.</param>
		private void Load(string sceneName)
		{
            Scene(sceneName);
			SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
			CleanUp();
		}

		/// <summary>
		/// Scene the specified sceneBuildIndex.
		/// </summary>
		/// <param name="sceneBuildIndex">Scene build index.</param>
        private static void Scene (string sceneName)
		{
            previousScene = SceneManager.GetActiveScene().name;
            currentScene = sceneName;
		}

		/// <summary>
		/// Unloads assets that are not used.
		/// </summary>
		private static void CleanUp()
		{
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}
	}
}
