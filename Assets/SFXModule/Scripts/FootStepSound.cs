using UnityEngine;
using System.Collections;
using Poptropica2.SFXModule;
/// <summary>
/// FootStepSound : For playing the foot step sound
/// soundName : Name of the footstep sound
/// </summary>
public class FootStepSound : MonoBehaviour {
	/// <summary>
	/// Assign a clip to play for the foot step
	/// </summary>
	public string soundName;
    private AudioSource audioSource;
	/// <summary>
	/// Method to play foot step sound
	/// </summary>
	public void PlayFootStepSound (bool isOverride) {
        if (audioSource == null)
        {
            this.gameObject.AddComponent<AudioSource>();
            audioSource = this.GetComponent<AudioSource>();
        }
        SFXManager.PlayClip(audioSource,SFXManager.GetClip (soundName), isOverride);
	}
}
