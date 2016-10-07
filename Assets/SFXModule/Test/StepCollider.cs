/// <summary>
/// Step collider Sample Class
/// </summary>
using UnityEngine;
using System.Collections;
using Poptropica2.SFXModule;
public class StepCollider : MonoBehaviour {
   
    public bool isOverride;
    private FootStepSound footstep;
    private AudioSource audioSource;

    void Awake ()
    {
        footstep = this.gameObject.GetComponent<FootStepSound>();
    }
    public void OnStepInCollider ()
    {
        if (this.gameObject.GetComponent<FootStepSound>() == null)
        {
            if (audioSource == null)
            {
                this.gameObject.AddComponent<AudioSource>();
                audioSource = this.gameObject.GetComponent<AudioSource>();
            }
            SFXManager.PlayClip(audioSource, SFXManager.GetClip ("DefaultFootStep"), isOverride);
        } 
        else
        {
            if (footstep == null)
            {
                footstep = this.gameObject.GetComponent<FootStepSound>();
            }
            footstep.PlayFootStepSound(isOverride);
        }
    }
}
