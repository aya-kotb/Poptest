using UnityEngine;
using System.Collections;
using Poptropica2.PopupSystem;

// author: Scott Gilman

/// <summary>
/// Test popup from popup data.
/// </summary>
public class TestPopupFromData : MonoBehaviour
{
    [Header("Drag prefab or set asset name for AssetBundle")]
    public PopupData data;

    void OnEnable()
    {
        PopupManager.instance.LoadPopupFromData(data);
    }
}