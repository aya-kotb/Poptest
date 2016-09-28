using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Poptropica2;
using Poptropica2.APIVersioningSystem;

public class APIVersioningSystemTest : MonoBehaviour {

    public Text versionText;
    public float invokeTime = 5f;

    public APIVersioningService apiVersioningService;

    // Use this for initialization
	void Start () {
        versionText.text = "API Current Vesrion: " + GameSparksManager.GSVersion.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickIncreaseVersion ()
    {
        int version = GameSparksManager.GSVersion;
        version++;
        GameSparksManager.SetGSVersion(version);
        versionText.text = "API Vesrion: " + GameSparksManager.GSVersion.ToString();
    }

    public void OnClickDecreaseVersion ()
    {
        int version = GameSparksManager.GSVersion;
        version--;
        GameSparksManager.SetGSVersion(version);
        versionText.text = "API Vesrion: " + GameSparksManager.GSVersion.ToString();
    }

    public void OnClickGetServerVersion ()
    {
        if (apiVersioningService == null)
        {
            apiVersioningService = GameObject.FindObjectOfType<APIVersioningService>();
            
        }

        apiVersioningService.InvokeAPIVersionCheck(invokeTime);
    }
}
