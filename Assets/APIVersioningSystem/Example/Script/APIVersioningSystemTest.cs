using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Poptropica2;
using Poptropica2.APIVersioningSystem;

public class APIVersioningSystemTest : MonoBehaviour {

    public Text versionText;
    public float invokeTime = 5f;

    public APIVersioningSystem apiVersioningService;

    // Use this for initialization
	void Start () {
        versionText.text = "API Current Version: " + GameSparksManager.GSVersion.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            apiVersioningService.PopUpMessage("Update Pending");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            apiVersioningService.PopUpMessage("Please update your game to continue being able to buy items and save your game to our servers.", true);
        }
	}

    public void OnClickIncreaseVersion ()
    {
        int version = GameSparksManager.GSVersion;
        version++;
        GameSparksManager.SetGSVersion(version);
        versionText.text = "API Current Version: " + GameSparksManager.GSVersion.ToString();
    }

    public void OnClickDecreaseVersion ()
    {
        int version = GameSparksManager.GSVersion;
        version--;
        GameSparksManager.SetGSVersion(version);
        versionText.text = "API Current Version: " + GameSparksManager.GSVersion.ToString();
    }

    public void OnClickGetServerVersion ()
    {
        if (apiVersioningService == null)
        {
            apiVersioningService = GameObject.FindObjectOfType<APIVersioningSystem>();
            
        }

        apiVersioningService.InvokeAPIVersionCheck(invokeTime);
    }
}
