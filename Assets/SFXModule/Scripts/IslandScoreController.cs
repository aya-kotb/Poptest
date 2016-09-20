using UnityEngine;
using System.Collections;
using Poptropica2.SFXModule;
using System.Linq;
/// <summary>
/// IslandScoreController : Controlls all the IslandComponent In the scene
/// GetInstance : Return the IslandScoreController instance
/// currentIslandScore : Get or Set the CurrentPlaying IslandScore
/// PlayIslandScore () : Plays the IslandScore with Maximum priority
/// StopIslandScore () : Stop currently active IslandScore
/// SetIslandScorePriority (int islandComponentIndex) : Set the "islandComponentIndex"th IslandScore as high priority
/// </summary>

public class IslandScoreController : MonoBehaviour {
    
    #region Variables
    public bool isPlayAutomatically;
    private IslandScore [] islandScores;
    private static IslandScoreController instance;
    #endregion

    #region External
    /// <summary>
    /// Get or Set CurrentIslandScore
    /// </summary>
    public IslandScore currentIslandScore
    {
        get;
        set;
    }

    /// <summary>
    /// Get the IslandController Instance
    /// </summary>
    /// <value>The get instance.</value>
    public static IslandScoreController GetInstance
    {
        get {
            if (instance == null)
                {
                    instance = FindObjectOfType<IslandScoreController> ();
                   
                    if (instance == null)
                    {
                        GameObject go = new GameObject("IslandScoreController");
                        go.AddComponent<IslandScoreController>();
                        instance = go.GetComponent<IslandScoreController>();
                    }
                }
            return instance;
        }
    }

    /// <summary>
    /// Play the IslandScore according to priority
    /// </summary>
    public void PlayIslandScore ()
    {
        if (islandScores != null && islandScores.Length > 0)
        {
            currentIslandScore = GetHighPriorityIslandScore (islandScores);
            PlayMostPriorIslandScore (currentIslandScore);
        } 
        else
        {
            Debug.Log ("No island scores found");
        }
    }

    /// <summary>
    /// Stop the current playing islandScore
    /// </summary>
    public void StopIslandScore ()
    {
        if (currentIslandScore != null)
        {
            currentIslandScore.StopIslandScore();
        }
    }

    /// <summary>
    /// Setting the IslandTrack priority
    /// </summary>
    /// <param name="_islandComponentIndex">Island component index.</param>
    public void SetIslandScorePriority (int islandComponentIndex)
    {
        if (islandScores != null && islandScores.Length > 0)
        {
            for (int i = 0 ;i < islandScores.Length; i++)
            {
                islandScores[i].islandPriority = 0;
            }
            islandScores[islandComponentIndex].islandPriority = 1;

        } else {
            Debug.Log ("No island scores found");
        }
    }
    #endregion

    #region Internal
    void Start ()
    {
        GetAllIslandScores ();
        if (isPlayAutomatically)
        {
            PlayIslandScore();
        }
    }

    void Awake ()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    void OnLevelWasLoaded (int level)
    {
        GetAllIslandScores ();
        if (isPlayAutomatically)
        {
            PlayIslandScore();
        }
    }

    private void StopAllIslandScores ()
    {
        for (int i =0 ;i< islandScores.Length; i++)
        {
            islandScores[i].isPlaying = false;
            islandScores[i].gameObject.SetActive (false);
        }
    }

    private IslandScore GetHighPriorityIslandScore (IslandScore[] islandScores)
    {
        IslandScore tempIslandscore;
        tempIslandscore = islandScores[0];
        foreach (var islandScore in islandScores)
        {
            if (tempIslandscore.islandPriority < islandScore.islandPriority)
            {
                tempIslandscore.islandPriority = islandScore.islandPriority;
                tempIslandscore = islandScore;
            }
        }
        return tempIslandscore;
    }

    private void GetAllIslandScores ()
    {
        if (currentIslandScore != null)
        {
            currentIslandScore.StopIslandScore(true);
            currentIslandScore = null;
        }

        islandScores = null;
        islandScores = FindObjectsOfType<IslandScore>();

        if (islandScores != null && islandScores.Length > 0)
        {
            islandScores = islandScores.Where(t => !t.isPlaying).ToArray();
        }
        Debug.Log ("Total Number of Islandscores " +islandScores.Length);
        StopAllIslandScores ();
    }

    private void PlayMostPriorIslandScore (IslandScore islandScore) {
        islandScore.isPlaying = true;
        islandScore.gameObject.SetActive (true);
        islandScore.gameObject.AddComponent<DonotDestroyOnLoad>();
    }
    #endregion
}
