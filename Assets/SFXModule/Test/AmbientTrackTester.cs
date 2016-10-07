using UnityEngine;
using System.Collections;
using Poptropica2.SFXModule;
using UnityEngine.UI;
public class AmbientTrackTester : MonoBehaviour 
{
    public string AmbientTrackName;
    public Slider volumeSlider;
	// Use this for initialization
	void Start ()
    {
        AmbientTrack.GetInstance.CurrentAmbientTrack = AmbientTrackName;
	}

    public void OnSliderValueChange ()
    {
        AmbientTrack.GetInstance.SetAmbientTrackVolume(volumeSlider.value);
    }
}
