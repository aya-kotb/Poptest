using System;
using UI.Dialogs;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Poptropica2.MDSModule
{
    /// <summary>
    /// MDSManager :This Class Provides the functionality for the Message display System
    /// This class can be use to Create the MDSWindow instance and add the Contents to it
    /// It can be used as a service
    /// </summary>
    public class MDSManager : IService
    {
        #region External
        /// <summary>
        /// ShowInspectorUI: This is the implemenation method for the IService interface
        /// </summary>
        public void ShowInspectorUI()
        {
            
        }
        /// <summary>
        /// Creates the window.
        /// It will create a MDSWindow instance and return to the calling Method
        /// </summary>
        /// <returns>The window.</returns>
        public MDSWindow CreateWindow ()
        {
            MDSWindow nw = new MDSWindow ();
            return nw;
        }
        /// <summary>
        /// Adds the contents to window.
        /// Pass the customisad window instance, and this function will inturn make the window and displays it
        /// </summary>
        /// <param name="_mdsWindow">Mds window instance</param>
        public void AddContentsToWindow (MDSWindow _mdsWindow)
        {
            if (_mdsWindow == null) 
            {
                return;
            }
            uDialog nw = uDialog.NewDialog();
            SetUpWindow (_mdsWindow, ref nw);
        }
        #endregion

        #region Internal
        /// <summary>
        /// Adds the buttons to the uDialog
        /// </summary>
        /// <param name="_buttons">Buttons.</param>
        /// <param name="_ud">Ud.</param>
        private void AddButtons (List<MDSButton> _buttons, ref uDialog _ud)
        {
            foreach (var btn in _buttons) 
            {
                uDialog_Button_Data btndta = new uDialog_Button_Data();
                btndta.ButtonText = btn.buttonText;
                btndta.OnClick += btn.CallBackFunction;
                _ud.AddButton(btndta);
            }
        }
        /// <summary>
        /// This function performs the internal customisation job
        /// </summary>
        /// <param name="_mdsWindow">Mds window.</param>
        /// <param name="_newwindow">Newwindow.</param>
        private void SetUpWindow (MDSWindow _mdsWindow, ref uDialog _newwindow)
        {
            if (_mdsWindow != null)
            {
                if (_mdsWindow.height != null && _mdsWindow.height > 0) 
                    _newwindow.SetHeight(_mdsWindow.height);
                if (_mdsWindow.width != null && _mdsWindow.width > 0) 
                    _newwindow.SetHeight(_mdsWindow.width);

                if (_mdsWindow.imageThemeSet != null) 
                {
                    _newwindow.SetThemeImageSet(_mdsWindow.imageThemeSet);
                }
                if (string.IsNullOrEmpty(_mdsWindow.colorScheme) == false) 
                {
                    _newwindow.SetColorScheme(_mdsWindow.colorScheme);
                }
                else
                {
                    _newwindow.SetColorScheme("Light");
                }
                if (string.IsNullOrEmpty( _mdsWindow.titleText ) == false) 
                {
                    _newwindow.SetTitleText(_mdsWindow.titleText);
                }
                if (string.IsNullOrEmpty( _mdsWindow.contentText ) == false) 
                {
                    _newwindow.SetContentText(_mdsWindow.contentText);
                }

                if (_mdsWindow.buttonList != null && _mdsWindow.buttonList.Count > 0)
                {
                    AddButtons (_mdsWindow.buttonList, ref _newwindow);
                }

                _newwindow.SetIcon(_mdsWindow.iconType);
                _newwindow.SetAllowDraggingViaDialog (_mdsWindow.isDraggable);
                _newwindow.SetAllowDraggingViaTitle(_mdsWindow.isDraggable);
                _newwindow.SetCloseWhenClicked(_mdsWindow.closeOnClick);
                _newwindow.SetShowTitle(_mdsWindow.showTitle);
                _newwindow.SetShowTitleCloseButton(_mdsWindow.showTitleCloseButton);
                _newwindow.SetDestroyAfterClose(_mdsWindow.isDestroyAfterClose);

            }
        }

		public void StartService(SAMApplication application)
		{
		}

		public void StopService(SAMApplication application)
		{
		}

		public void Configure(ServiceConfiguration config)
		{

		}
        #endregion
    }
}