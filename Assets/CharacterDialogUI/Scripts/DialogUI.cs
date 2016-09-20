using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Poptropica2.CharacterDialogUI {
/// <summary>
/// This component is attached to the dialog prefab
/// The dialogText can be adjusted during the runtime
/// The pivot can be altered based on the positioning required - Especially in the case of microphone barks
/// </summary>
public class DialogUI : MonoBehaviour {

    public RectTransform panel;
    public Text dialogText;
    Transform speakerTransform;
    float duration;

    public Transform SpeakerTransform
    {
        get
        {
            return speakerTransform;
        }
        set
        {
            speakerTransform = value;
        }
    }
    
    /// <summary>
    /// Gets or sets the duration. After duration ends, the gameobject gets destroyed
    /// </summary>
    /// <value>The duration.</value>
    public float Duration
    {
        get
        {
            return duration;
        }
        set
        {
            duration = value;
        }
    }

    /// <summary>
    /// Sets the pivot of the panel for dynamic positioning needs
    /// </summary>
    /// <param name="pivot">Pivot.</param>
    public void SetPivot(Vector2 pivot)
    {
        panel.pivot = pivot;
    }
}
}
