using UI.Dialogs;
using UnityEngine.Events;
using System.Collections.Generic;
namespace Poptropica2.MDSModule
{
    /// <summary>
    /// MDSWindow: This class used to make the customisable dialog box
    /// By accessing this class instance, we will be able to make the customisable dialog boxes
    /// This customisable component shoud be passed to MDSManager class to make the custom dialog box
    /// </summary>
    public class MDSWindow
    {
        public string titleText;
        public string contentText;
        public float height;
        public float width;
        public bool isDraggable;
        public bool showTitle = true;
        public bool showTitleCloseButton;
        public bool closeOnClick;
        public bool isDestroyAfterClose = true;
        public string colorScheme;
        public eThemeImageSet imageThemeSet;
        public eIconType iconType;
        public MDSWindow newWindow;

        public List <MDSButton> buttonList;
        /// <summary>
        /// Sets the title text.
        /// </summary>
        /// <param name="_titleText">Title text.</param>
        public void SetTitleText (string _titleText)
        {
            titleText = _titleText;
        }
        /// <summary>
        /// Sets the content text.
        /// </summary>
        /// <param name="_contentText">Content text.</param>
        public void SetContentText (string _contentText)
        {
            contentText = _contentText;
        }
        /// <summary>
        /// Sets the dimensions.
        /// </summary>
        /// <param name="_height">Height.</param>
        /// <param name="_width">Width.</param>
        public void SetDimensions (float _height, float _width)
        {
            height = _height;
            width = _width;
        }
        /// <summary>
        /// Sets the is draggable.
        /// </summary>
        /// <param name="_isDraggable">If set to <c>true</c> is draggable.</param>
        public void SetIsDraggable (bool _isDraggable)
        {
            isDraggable = _isDraggable;
        }
        /// <summary>
        /// Sets the show title bar.
        /// </summary>
        /// <param name="_isShow">If set to <c>true</c> Will display the title bar</param>
        public void SetShowTitleBar (bool _isShow)
        {
            showTitle = _isShow;
        }
        /// <summary>
        /// Sets the title close button.
        /// </summary>
        /// <param name="_isTitleCloseButton">If set to <c>true</c> Shows the close button on the title bar</param>
        public void SetTitleCloseButton (bool _isTitleCloseButton)
        {
            showTitleCloseButton = _isTitleCloseButton;
        }
        /// <summary>
        /// Sets the Window close on click on it.
        /// </summary>
        /// <param name="_closeOnClick">If set to <c>true</c> close window</param>
        public void SetCloseOnClik (bool _closeOnClick)
        {
            closeOnClick = _closeOnClick;
        }
        /// <summary>
        /// Adds the icon im dialog box.
        /// </summary>
        /// <param name="_icontype">Icontype.</param>
        /// Example : eIconType.Error
        public void AddIconImDialogBox (eIconType _icontype)
        {
            iconType = _icontype;
        }
        /// <summary>
        /// Sets the destroy after close.
        /// The window will destroy if set true, else it will stay in the heirarchy
        /// </summary>
        /// <param name="_isDestroyAfterClose">If set to <c>true</c> is destroy after close.</param>
        public void SetDestroyAfterClose (bool _isDestroyAfterClose)
        {
            isDestroyAfterClose = _isDestroyAfterClose;
        }
        /// <summary>
        /// Sets the image theme.
        /// </summary>
        /// <param name="_imageThemeSet">Image theme set.</param>
        /// Example :  eThemeImageSet.Angular
        public void SetImageTheme (eThemeImageSet _imageThemeSet)
        {
            imageThemeSet = _imageThemeSet;
        }
        /// <summary>
        /// Set the Color scheme for the Dialog box
        /// </summary>
        /// <param name="_colorScheme">Color scheme.</param>
        /// Examples : Light, Plain,Dark,Green Highlight,Orange Red,Blue Highlight,Fantasy,Blue Glow and Green Glow
        public void SetColorScheme (string _colorScheme)
        {
            colorScheme = _colorScheme;
        }
        /// <summary>
        /// This method is for adding button to the window
        /// </summary>
        /// <param name="_buttonText">Button text.</param>
        /// <param name="_callbackAction">Callback action.</param>
        public void AddButton (string _buttonText, UnityAction _callbackAction)
        {
            if (buttonList == null)
            {
                buttonList = new List<MDSButton> ();
            }
            MDSButton nb = new MDSButton ();
            nb.buttonText = _buttonText;
            nb.CallBackFunction = _callbackAction;
            buttonList.Add (nb);
        }
    }
}

