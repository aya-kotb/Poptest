using UnityEngine;
using System.Collections;
using Poptropica2.PopupSystem;

// author: Rick Hocker

/// <summary>
/// Demo for subscribing to button clicks
/// </summary>
public class Demo : MonoBehaviour
{
	void Start ()
    {
        PopupManager.instance.Subscribe("PopupTemplate", PopupButtonID.CloseButton, closeButtonClicked);
        PopupManager.instance.Subscribe("PopupTemplate", PopupButtonID.CloseButton, closeButtonClicked2);
        PopupManager.instance.Subscribe("PopupUITemplate", PopupButtonID.CloseButton, closeButtonClicked);
        PopupManager.instance.Subscribe("DialogBoxTemplate", PopupButtonID.OKButton, okButtonClicked);
        PopupManager.instance.Subscribe("DialogBoxTemplate", PopupButtonID.CancelButton, cancelButtonClicked);
    }

    void closeButtonClicked()
    {
        Debug.Log("Close button clicked!");
    }

    void closeButtonClicked2()
    {
        Debug.Log("Close button clicked second listener!");
    }

    public void closeButtonClickedPublic()
    {
        Debug.Log("Close button clicked public!");
    }

    void okButtonClicked()
    {
        Debug.Log("OK button clicked!");
    }

    void cancelButtonClicked()
    {
        Debug.Log("Cancel button clicked!");
    }

    void OnDestroy()
    {
        PopupManager.instance.Unsubscribe("PopupTemplate", PopupButtonID.CloseButton, closeButtonClicked);
        PopupManager.instance.Unsubscribe("PopupUITemplate", PopupButtonID.CloseButton, closeButtonClicked);
        PopupManager.instance.Unsubscribe("DialogBoxTemplate", PopupButtonID.OKButton, okButtonClicked);
        PopupManager.instance.Unsubscribe("DialogBoxTemplate", PopupButtonID.CancelButton, cancelButtonClicked);
    }
}
