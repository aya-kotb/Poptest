using UnityEditor;
using UnityEngine;

/// <summary>
/// Ad editor utilies for Assets menu.
/// </summary>
public class PopupEditorUtils
{
    /// <summary>
    /// Builds the popup asset bundles for Mac from the Assests menu.
    /// </summary>
    [MenuItem ("Assets/Build Popup AssetBundle for Mac")]
    static void BuildMacAdAssetBundle()
    {
        CreateBundleForTarget(BuildTarget.StandaloneOSXUniversal);
    }

    /// <summary>
    /// Builds the popup asset bundles for Windows from the Assests menu.
    /// </summary>
    [MenuItem ("Assets/Build Popup AssetBundle for Windows")]
    static void BuildWindowsAdAssetBundle()
    {
        CreateBundleForTarget(BuildTarget.StandaloneWindows64);
    }

    /// <summary>
    /// Builds the popup asset bundles for Web from the Assests menu.
    /// </summary>
    [MenuItem ("Assets/Build Popup AssetBundle for Web")]
    static void BuildWebAdAssetBundle()
    {
        CreateBundleForTarget(BuildTarget.WebGL);
    }

    /// <summary>
    /// Creates the bundle for target.
    /// </summary>
    /// <param name="selection">Selection.</param>
    /// <param name="target">Target.</param>
    static void CreateBundleForTarget(BuildTarget target)
    {
        string bundleName = "popups";
        // get selected guids and game objects
        string[] guids = Selection.assetGUIDs;

        // if some game objects selected
        if (guids.Length != 0)
        {
            // Create the array of bundle build details.
            AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

            // add assets based on selection
            string[] assets = new string[guids.Length];
            for (int i = 0; i != guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = path;
                Debug.Log("Popups.Bundling: " + path);
            }
            buildMap[0].assetNames = assets;

            // set bundle name
            buildMap[0].assetBundleName = bundleName;

            // get folder based on target
            string folder = "AssetBundles/";
            switch (target)
            {
                case BuildTarget.StandaloneOSXUniversal:
                    folder += "OSX";
                    break;
                case BuildTarget.StandaloneWindows64:
                    folder += "Windows";
                    break;
                case BuildTarget.WebGL:
                    folder += "Web"; // TODO: fix this for web
                    break;
            }
            Debug.Log("Popups.Bundling: Using name " + bundleName);
            // create bundle
            BuildPipeline.BuildAssetBundles(folder, buildMap, BuildAssetBundleOptions.None, target);
        }
        else
        {
            Debug.Log("Popups.Bundling: No objects selected");
        }
    }
}