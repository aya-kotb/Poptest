using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Poptropica2.SFXModule;
/// <summary>
/// UIController : Used to test all features of SFXModule
/// </summary>
public class UIController : MonoBehaviour 
{
    #region Variables 
	public InputField audioclipname;
	public Slider listenerrvolume;
	public Toggle mutetoggle;
	public Dropdown islands;
    public Slider ambientSlider1;
    public Slider ambientSlider2;
    public Slider ambientSlider3;
    public Transform ambient1;
    public Transform ambient2;
    public Transform ambient3;
    public string scene;
    private AmbientSound [] ambientSounds;
    private AudioSource audioSource;
    #endregion


    void Awake () 
    {
        ambientSounds = FindObjectsOfType <AmbientSound>();
        Debug.Log (ambientSounds.Length);
    }

	public void OnPlayClipButtonPress ()
    {
		if (string.IsNullOrEmpty(audioclipname.text) == false) 
        {
            if (audioSource == null)
            {
                this.gameObject.AddComponent<AudioSource>();
                audioSource = this.GetComponent<AudioSource>();
            }

            SFXManager.PlayClip (audioSource, SFXManager.GetClip (audioclipname.text), false);
		}
        else {
            
		}

	}

	public IslandScore[] islandscores;
   
	public void OnIslandScoreStart () {
		
        IslandScoreController.GetInstance.PlayIslandScore();
	}

	public void OnIslandScoreStop () {
        IslandScoreController.GetInstance.StopIslandScore();

	}

	public void OnIslandScoreChange ()
    {

        IslandScoreController.GetInstance.SetIslandScorePriority(islands.value);
        IslandScoreController.GetInstance.PlayIslandScore();
	}

	public void FadeInAudioBtnPress ()
    {
       
        if (IslandScoreController.GetInstance.currentIslandScore != null)
        {
            StartCoroutine(IslandScoreController.GetInstance.currentIslandScore.FadeInAudio (IslandScoreController.GetInstance.currentIslandScore.islandAudioSource, false));
		} 
	}
	
	public void FadeOutAudioBtnPress ()
    {
        if (IslandScoreController.GetInstance.currentIslandScore != null) {
            StartCoroutine(IslandScoreController.GetInstance.currentIslandScore.FadeOutAudio (IslandScoreController.GetInstance.currentIslandScore.islandAudioSource));
        } 

	}

	public void OnVolumeSliderValueChange ()
    {
		SFXManager.GetInstance.AudioListener_AdjustVolume(listenerrvolume.value);
	}

	public void OnMuteButton () 
    {
		if (!mutetoggle.isOn) {
			SFXManager.GetInstance.AudioListener_Mute();
		} else {
			SFXManager.GetInstance.AudioListener_UnMute();
		}
	}

    public void OnSliderChange1 () 
    {
        Vector3 pos = ambient1.position;
        pos.x = ambientSlider1.value;
        ambient1.position = pos;
    }

    public void OnSliderChange2 ()
    {
        Vector3 pos = ambient2.position;
        pos.x = ambientSlider2.value;
        ambient2.position = pos;
    }
    public void OnSilderChange3 () 
    {
        Vector3 pos = ambient3.position;
        pos.x = ambientSlider3.value;
        ambient3.position = pos;
    }

    public void StartAmbientSounds ()
    {
        AmbientSoundController.GetInstance.PlayAmbientSounds();
    }

    public void StopAmbientSounds ()
    {
        if (ambientSounds == null || ambientSounds.Length == 0)
            return;

        foreach (var amsds in ambientSounds)
        {
            amsds.gameObject.SetActive (false);

        }
    }

    public void GotoNextScene ()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
//        if (IslandScoreController.GetInstance.currentIslandScore != null) {
//            IslandScoreController.GetInstance.currentIslandScore.StopIslandScore (true, scene);
//        } 
    }
}
