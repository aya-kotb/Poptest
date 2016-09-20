using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using Poptropica2.CharacterDialogUI;

namespace Poptropica2.CharacterDialogUI {
   
/// <summary>
/// The plugin has designed Bark UI to be under the NPC hierarchy itself. This class transfers those callbacks to the DialogUICanvas component
/// The plugin also asks for individual UI components for each bark. This class communicates with the DialogUICanvas to create the barks necessary at runtime
/// However this class needs to be attached to any object which can bark - Along with the BarkOnIdle script from the plugin
/// For mic announcements .etc. Bark System is a feature inside the "Dialogue System For Unity" plugin which suits this need
/// Implements the IBarkUI interface from the plugin
/// </summary>
public class BarkToUI : MonoBehaviour, IBarkUI {

    public DialogUICanvas dialogUICanvas;
    public Vector3 barkOffsetFromCenter;
    public Vector2 pivot = new Vector2(0.5f, 0.5f);                         //The 0.5f on both axes means that the pivot is at the center of the Bark UI Dialog
    BarkOnIdle barkOnIdle;
    float doneTime;


    void Awake()
    {
        barkOnIdle = GetComponent<BarkOnIdle>();   
    }


    #region IBarkUI implementation
    /// <summary>
    /// Transfers the subtitle, position of the barker, duration and pivot to the DialogUICanvas component
    /// The DialogUICanvas component creates the necessary UI from the data received
    /// 
    /// The pivot of the bark UI dialog can be set based on requirement.  
    /// </summary>
    /// <param name="subtitle">Subtitle.</param>
    public void Bark(Subtitle subtitle)
    {
       float duration = barkOnIdle.minSeconds;
       Vector3 barkPosition = transform.position + barkOffsetFromCenter;
       dialogUICanvas.CreateBark(barkPosition, subtitle, duration, pivot);
       doneTime = DialogueTime.time + duration;
    }
       
    public bool IsPlaying
    {
        get
        {
            return (DialogueTime.time < doneTime); 
        }
    }

    #endregion


}
}
