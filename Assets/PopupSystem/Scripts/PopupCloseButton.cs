using UnityEngine;
using System.Collections;

// author: Rick Hocker

namespace Poptropica2.PopupSystem
{
    /// <summary>
    /// Popup close button script attached to non-UI close button prefab.
    /// </summary>
    public class PopupCloseButton : MonoBehaviour
    {
        // popup buttons interface from popup script
        [HideInInspector]public IPopupButtons script;

    	void OnMouseUp()
        {
            // trigger click handler from popup script
            script.OnCloseButtonClicked();
    	}

        void OnMouseEnter()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.9f, 0.9f);
        }

        void OnMouseExit()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        }

        void OnMouseDown()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f);
        }
    }
}