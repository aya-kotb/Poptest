using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Poptropica2;
using Poptropica2.APIVersioningSystem;

/// <summary>
/// Temporary class to test the API versioning system
/// </summary>
public class APIVersioningSystemTest : MonoBehaviour {

    public Text versionText;
    public float invokeTime = 5f;

    public APIVersioningService apiVersioningService;

	void Start () 
	{
		apiVersioningService = SAMApplication.mainInstance.GetService<APIVersioningService>();
		versionText.text = "Current API Version in client : " + GameSparksManager.GSVersion.ToString();
	}

	/// <summary>
	/// On Button click Increments the GSVersion by 1
	/// </summary>
    public void OnClickIncreaseVersion ()
    {
        int version = GameSparksManager.GSVersion;
        version++;
        GameSparksManager.SetGSVersion(version);
		versionText.text = "Current API Version in client : " + GameSparksManager.GSVersion.ToString();
		apiVersioningService.InvokeAPIVersionCheck(invokeTime);
    }

	/// <summary>
	/// On button click decrements the GSVersion by 1 
	/// </summary>
    public void OnClickDecreaseVersion ()
    {
        int version = GameSparksManager.GSVersion;
        version--;
        GameSparksManager.SetGSVersion(version);
		versionText.text = "Current API Version in client : " + GameSparksManager.GSVersion.ToString();
		apiVersioningService.InvokeAPIVersionCheck(invokeTime);
    }
}
