using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Temporary script.
/// Used to switch scenes between QuestJournal & IslandOverview
/// </summary>
public class SceneLoad : MonoBehaviour {

	AsyncOperation acync;

	/// <summary>
	/// Raises the scene switch button clicked event.
	/// </summary>
	/// <param name="levelNumber">Level number.</param>
	public void OnSceneSwitchButtonClicked (int levelNumber) 
	{
		StartCoroutine (StartLoadScene(levelNumber));
	}

	IEnumerator StartLoadScene (int level)
	{
		AsyncOperation async = SceneManager.LoadSceneAsync (level, LoadSceneMode.Single);
		while (!async.isDone) {
			
			yield return new WaitForEndOfFrame ();
		}
	}
}
