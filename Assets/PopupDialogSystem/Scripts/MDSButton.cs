using UnityEngine;
using UnityEngine.Events;

namespace Poptropica2.MDSModule
{
    /// <summary>
    /// BDSButton : Button component
    /// name : text on the button
    /// CallBackFunction : Callback function for on button press
    /// </summary>
    public class MDSButton
    {
        public string buttonText;
        public UnityAction CallBackFunction;
    }
}

