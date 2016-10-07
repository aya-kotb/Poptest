using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Poptropica2.MDSModule;
using Poptropica2;

/// <summary>
/// MDS test.
/// This component is used for testing the features of the Message Display System
/// </summary>
public class MDSTest : MonoBehaviour
{
    MDSManager mdsManager;
    public Image bgcolorPanel;
    public Dropdown imageThemeSet;
    public Dropdown colorSet;

    enum ColorSet {
        C_Light = 0,
        C_Dark,
        C_Plain
    };

	void Start () {
		
        SAMApplication.Instantiate();
        SAMApplication.mainInstance.AddService("MDSManager", new MDSManager());
        mdsManager = SAMApplication.mainInstance.GetService<MDSManager>();
	}

    /// <summary>
    /// Raises the button click event.
    /// </summary>
    /// <param name="i">The index.</param>
    public void OnButtonClick (int i) {

        switch (i) {
            case 1:
                CreateSimpleDialogbox ();
                break;
            case 2:
                CreateDialogboxWithHeader_Content ();
                break;
            case 3:
                CreateDialogboxWith_Content_OneButton ();
                break;
            case 4:
                CreateDialogboxWith_Header_Content_OneButton ();
                break;
            case 5:
                CreateDialogboxWith_Header_Content_TwoButtons ();
                break;
            case 6:
                CreateDialogboxWith_Header_Content_MoreThanTwoButtons ();
                break;
            case 7:
                SimpleErrorMessage ();
                break;
            case 8:
                RatePopUp ();
                break;
            case 9:
                SimpleAchivementMessgage ();
                break;
            case 10:
                SimpleWarningMessage ();
                break;
        }
        
    }
    /// <summary>
    /// Creates the simple dialogbox.
    /// </summary>
    private void CreateSimpleDialogbox ()
    {
        MDSWindow newWindow = mdsManager.CreateWindow();
        newWindow.SetDimensions (200.0f, 300.0f);
        newWindow.SetContentText ("Hello This is a simple Dialog box \n You can drag it !!!!");
        newWindow.SetIsDraggable (true);
        newWindow.SetCloseOnClik (true);
        mdsManager.AddContentsToWindow(newWindow);
    }
    /// <summary>
    /// Creates the content of the dialogbox with header and Content.
    /// </summary>
    private void CreateDialogboxWithHeader_Content () {
        MDSWindow newWindow = mdsManager.CreateWindow();
        newWindow.SetTitleText ("Test window");
        newWindow.SetContentText ("Hello This is a simple Dialog box");
        newWindow.SetTitleCloseButton (true);
        mdsManager.AddContentsToWindow(newWindow);
    }
    /// <summary>
    /// Creates the dialogbox with content and one button.
    /// </summary>
    private void CreateDialogboxWith_Content_OneButton () {

        MDSWindow newWindow = mdsManager.CreateWindow();
        newWindow.SetContentText ("Change color to red");
        newWindow.SetTitleCloseButton (true);
        newWindow.AddButton ("Red",ChangeColorToRed);
       
        mdsManager.AddContentsToWindow(newWindow);
    }


    /// <summary>
    /// Creates the dialogbox with header content and one button.
    /// </summary>
    private void CreateDialogboxWith_Header_Content_OneButton ()
    {
        MDSWindow newWindow = mdsManager.CreateWindow();
        newWindow.SetTitleText ("Color Change");
        newWindow.SetContentText ("Change color to red");
        newWindow.SetTitleCloseButton (true);
        newWindow.AddButton ("Green",ChangeColorToGreen);
        mdsManager.AddContentsToWindow(newWindow);
    }
    /// <summary>
    /// Creates the dialogbox with header content and two buttons.
    /// </summary>
    private void CreateDialogboxWith_Header_Content_TwoButtons ()
    {

        MDSWindow newWindow = mdsManager.CreateWindow();
        newWindow.SetTitleText ("Color Change");
        newWindow.SetContentText ("Change color");
        newWindow.SetTitleCloseButton (true);
        newWindow.AddButton ("Red",ChangeColorToRed);
        newWindow.AddButton ("Green",ChangeColorToGreen);
        mdsManager.AddContentsToWindow(newWindow);
    }

    /// <summary>
    /// Creates the dialogbox with header content and more than two buttons.
    /// </summary>
    private void CreateDialogboxWith_Header_Content_MoreThanTwoButtons ()
    {

        MDSWindow newWindow = mdsManager.CreateWindow();
        newWindow.SetTitleText ("Color Change");
        newWindow.SetContentText ("Choose a color to change the BG color");
        newWindow.SetTitleCloseButton (true);
        newWindow.AddButton ("Red",ChangeColorToRed);
        newWindow.AddButton ("Green",ChangeColorToGreen);
        newWindow.AddButton ("Blue",ChangeColorToBlue);
        newWindow.AddButton ("White",ChangeColorToWhite);
        mdsManager.AddContentsToWindow(newWindow);

    }
    /// <summary>
    /// Displays Simples the error message.
    /// </summary>
    private void SimpleErrorMessage ()
    {

        MDSWindow newWindow = mdsManager.CreateWindow();
        newWindow.SetTitleText ("Error!!!");
        newWindow.SetContentText ("An error Occured !!!");
        newWindow.AddIconImDialogBox(UI.Dialogs.eIconType.Error);
        newWindow.AddButton ("Close",ButtonPressCallBack_Close);
        newWindow.SetTitleCloseButton (false);

        mdsManager.AddContentsToWindow(newWindow);
    }

    /// <summary>
    /// Displays Simples the warning message.
    /// </summary>
    private void SimpleWarningMessage ()
    {

        MDSWindow newWindow = mdsManager.CreateWindow();
        newWindow.SetTitleText ("Warming!!!");
        newWindow.SetContentText ("You sure you want to continue?? !!!");
        newWindow.AddIconImDialogBox(UI.Dialogs.eIconType.Warning);
        newWindow.AddButton ("Yes",ButtonPressCallBack_Yes);
        newWindow.AddButton ("No",ButtonPressCallBack_NO);
        newWindow.SetTitleCloseButton (false);

        mdsManager.AddContentsToWindow(newWindow);
    }

    /// <summary>
    /// Displays Rates the pop up.
    /// </summary>
    private void RatePopUp ()
    {

        MDSWindow newWindow = mdsManager.CreateWindow();
        newWindow.SetTitleText ("Rate Me");
        newWindow.SetContentText ("If You Enjoy using sample app, Please rate us!!!");
        newWindow.AddButton ("Yes",ButtonPressCallBack_Yes);
        newWindow.AddButton ("No",ButtonPressCallBack_NO);
        newWindow.AddButton ("Never",ButtonPressCallBack_Never);
        newWindow.SetImageTheme (UI.Dialogs.eThemeImageSet.SciFi);
        newWindow.SetColorScheme("Light");
        newWindow.SetTitleCloseButton (false);

        mdsManager.AddContentsToWindow(newWindow);
    }
    /// <summary>
    /// Displays Simples the achivement messgage.
    /// </summary>
    private void SimpleAchivementMessgage ()
    {

        MDSWindow newWindow = mdsManager.CreateWindow();
        newWindow.SetTitleText ("Rate Me");
        newWindow.SetContentText ("If You Enjoy using sample app, Please rate us!!!");
        newWindow.AddButton ("Dismiss",ButtonPressCallBack_Dismiss);
        newWindow.SetColorScheme("Light");
        newWindow.SetTitleCloseButton (false);
        mdsManager.AddContentsToWindow(newWindow);
    }

    #region CommonCallBacks
    public void ChangeColorToRed ()
    {
        Debug.Log ("Call back for Red");
        bgcolorPanel.color = Color.red;
    }

    public void ChangeColorToGreen ()
    {
        Debug.Log ("Call back for Green");
        bgcolorPanel.color = Color.green;
    }
    public void ChangeColorToBlue ()
    {
        Debug.Log ("Call back for Blue");
        bgcolorPanel.color = Color.blue;
    }

    public void ChangeColorToWhite ()
    {
        Debug.Log ("Call back for White");
        bgcolorPanel.color = Color.white;
    }

    public void ButtonPressCallBack_Close ()
    {
        CommonMsg ( "<color=blue>Yes</color>");
    }
    public void ButtonPressCallBack_Ok ()
    {
        CommonMsg ( "<color=green>OK</color>");
    }
    public void ButtonPressCallBack_NO ()
    {
        CommonMsg ( "<color=red>No</color>");
    }
    public void ButtonPressCallBack_Never ()
    {
        CommonMsg ( "<color=blue>Never</color>");
    }
    public void ButtonPressCallBack_Dismiss ()
    {
        CommonMsg ( "<color=red>Dismiss</color>");
    }
    public void ButtonPressCallBack_Yes ()
    {
        CommonMsg ( "<color=green>Yes</color>");
    }

    private void CommonMsg (string _msg)
    {
        MDSWindow newWindow = mdsManager.CreateWindow();
        newWindow.SetContentText ("You have pressed " +_msg + " Button\n Click me to close\"");
        newWindow.SetColorScheme("Plain");
        newWindow.SetTitleCloseButton (false);
        newWindow.SetShowTitleBar (false);
        newWindow.SetCloseOnClik (true);
        mdsManager.AddContentsToWindow(newWindow);
    }

    #endregion

}
