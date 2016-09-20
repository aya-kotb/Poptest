using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Poptropica2.SFXModule;
using System.Linq;
namespace Poptropica2.SFXModule
{
    /// <summary>
    /// SFXManager : Provides the functionality for Audio look up and play 
    /// Init () : Initialising the audio clip array with the sound clips from zone and resource bundles
    /// GetInstance : Returns the SFXManager instance
    /// AudioListener_Mute () : Mute the Audio Listener
    /// AudioListener_UnMute (): UnMute the Audio Listener
    /// AudioListener_AdjustVolume (float _vol) : Change the volume of Audio Listener
    /// GetClip (string _clipName) : Perform the audio look up and returns a matched clip
    /// PlayClip (AudioClip _audioClip) : Play an audio clip passed to the method
    /// Testing
    /// : Set the value of isLoadSampleIslandBundle to True in inspector
    /// </summary>
    public class SFXManager : MonoBehaviour {

        #region internal
        /// <summary>
        /// Variables for getting the resources.
        /// </summary>
        public string generalBundlePath = "General/Sounds";
        public string sampleIslandPath = "SampleIsland/Sounds";
        public bool isLoadSampleIslandBundle;
        public int maxSounds = 5;

        private string currentIsland;
        private Dictionary <AudioData, AudioClip> audioDict;
        private Dictionary <string, string> audioResourcePaths;
        private static SFXManager instance;
        private bool isinitialised;
        private AudioSource sfxAudioSource;
        private List <AudioSource> activeAudioSources;

        private void LoadSFXClips (string islandPath)
        {
            if (audioDict == null)
            {
                audioDict = new Dictionary<AudioData, AudioClip>();
                AddContents(ref audioDict, generalBundlePath, true);
            }
            //Removing old zone clips
            audioDict= audioDict.Where(t => t.Key.isFromGeneraBundle).ToDictionary(t=>t.Key,t=>t.Value);
            Resources.UnloadUnusedAssets();
            //Adding new zone clips
            AddContents(ref audioDict, islandPath, false);
        }

        private void AddContents (ref Dictionary<AudioData, AudioClip> audioDicRef, string audioLoadPath , bool isFromGeneralBundle)
        {
            foreach (var clip in Resources.LoadAll<AudioClip>(audioLoadPath))
            {
                AudioData audioData = new AudioData();
                audioData.isFromGeneraBundle = isFromGeneralBundle;
                audioData.fileName = clip.name;
                audioDicRef.Add(audioData, clip);
            }
        }

        void Awake ()
        {
            instance = this;
            instance.Init ();
        }

        private void OnCurrentIslandChange (string currentIsland)
        {
            if (audioResourcePaths != null)
            {
                if (audioResourcePaths.ContainsKey(currentIsland))
                {
                    LoadSFXClips(audioResourcePaths[currentIsland]);
                }
                else
                {
                    Debug.Log("This Island is not added to the dictionary \n Add the Islands before using");
                }
            }
            else
            {
                Debug.Log("There are not audio paths available \n try adding the Island details");
            }
        }
        private void RemoveInactiveAudioSource (ref List <AudioSource> audioSources)
        {
            if (audioSources == null || audioSources.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < audioSources.Count; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                    audioSources.RemoveAt(i);
                }
            }
        }

        private void PlayAudio(AudioSource audioSource, AudioClip audioClip)
        {
            if (audioSource != null)
            {
                audioSource.clip = audioClip;
                audioSource.loop = false;
                audioSource.Play();
            }
        }

        #endregion

        #region External Region
        /// <summary>
        /// Return SFXManager instance
        /// </summary>
        public static SFXManager GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SFXManager>();
                    instance.Init ();
                }
                return instance;
            }
        }
        /// <summary>
        /// Fill all arrays with sound clips from the resources
        /// </summary>
        public void Init ()
        {
            if (!instance.isinitialised)
            {
                if (instance.isLoadSampleIslandBundle)
                {
                    AddIslandAudioDetails("SampleIsland", sampleIslandPath);
                    CurrentIsland = "SampleIsland";
                }

                if (instance.sfxAudioSource == null) {
                    instance.gameObject.AddComponent<AudioSource>();
                    instance.sfxAudioSource = instance.GetComponent<AudioSource>();
                }
                instance.isinitialised = true;
            }
        }

        /// <summary>
        /// Get SFX Audio clip from a clip name
        /// </summary>
        /// <returns>The clip.</returns>
        /// <param name="_clipname">Clipname.</param>
        public static AudioClip GetClip (string clipName)
        {
            bool isClipFound = false;
            if (string.IsNullOrEmpty(clipName))
            {
                Debug.Log ("Requested clip is null");
                return null;
            }
            ///Searching for the audio clip in zone bundle
            var searches = instance.audioDict.Where(t => t.Key.fileName.Contains(clipName) && !t.Key.isFromGeneraBundle).Select(t => t.Value).ToList();

            if (searches == null || searches.Count() <= 0)
            {
                ///Searching for the audio clip in general bundle
                searches = instance.audioDict.Where(t => t.Key.fileName.Contains(clipName) && t.Key.isFromGeneraBundle).Select(t => t.Value).ToList();
            }
            else
            {
                isClipFound = true;
            }

            if (searches != null || searches.Count() > 0)
            {
                isClipFound = true;
            }

            AudioClip retunClip = null;

            if (isClipFound)
            {
                if (searches.Count() > 1)
                {
                    int range = Random.Range(0, searches.Count() - 1);
                    Debug.Log ("Clip index " + range);
                    retunClip = searches[range];
                }
                else
                {
                    retunClip = searches[0];   
                }
            }
            else
            {
                Debug.Log ("No clip found with " + clipName + " name");
                return null;
            }
            return retunClip;
        }

        /// <summary>
        /// Adds the island audio details.
        /// Add more Island Details to this method to use with different Islands
        /// </summary>
        /// <param name="islandName">Island name.</param>
        /// <param name="islandPath">Island path.</param>
        public void AddIslandAudioDetails (string islandName, string islandPath)
        {
            if (string.IsNullOrEmpty(islandName) == false &&
                string.IsNullOrEmpty(islandPath) == false)
            {
                if (audioResourcePaths == null)
                {
                    audioResourcePaths = new Dictionary<string, string>();
                }

                if (audioResourcePaths.ContainsKey(islandName))
                {
                    Debug.Log("Already exist in the dictionary");
                    return;
                }
                audioResourcePaths.Add(islandName, islandPath);               
            }
        }

        /// <summary>
        /// For Muting
        /// </summary>
        public void AudioListener_Mute ()
        {
            AudioListener.volume = 0.0f;
        }

        /// <summary>
        /// For Unmuting
        /// </summary>
        public void AudioListener_UnMute ()
        {
            AudioListener.volume = 1.0f;
        }

        /// <summary>
        /// Adjusting the listener volume
        /// </summary>
        /// <param name="_vol">Vol.</param>
        public void AudioListener_AdjustVolume (float vol)
        {
            vol = Mathf.Clamp (vol,0.0f, 1.0f);
            AudioListener.volume = vol;
        }

        /// <summary>
        /// Gets or Setting the CurrentIsland
        /// Changing this will load the mentioned Island Audio clips
        /// </summary>
        /// <value>The current island.</value>
        public string CurrentIsland 
        {
            get 
            {
                return currentIsland;
            }
            set
            {
                if (currentIsland != value)
                {
                    OnCurrentIslandChange(value);
                    currentIsland = value;
                }
            }
        }
      /// <summary>
      /// Play the Audio clip passed to the function using the AudioSource.
      /// It will compare with maximum number of audio sources can play
      /// </summary>
      /// <param name="audioSource">Audio source.</param>
      /// <param name="audioClip">Audio clip.</param>
      /// <param name="isOverRide">If set to <c>true</c> then it will override current clip on the same source and play</param>
        public static void PlayClip (AudioSource audioSource, AudioClip audioClip, bool isOverRide)
        {
            if (instance.activeAudioSources == null)
            {
                instance.activeAudioSources = new List<AudioSource>();
            }
            instance.RemoveInactiveAudioSource(ref instance.activeAudioSources);
            if (instance.activeAudioSources.Count > 0)
            {
                if (instance.activeAudioSources.Contains(audioSource))
                {
                    //Ask for override
                    if (isOverRide)
                    {
                        instance.PlayAudio(audioSource, audioClip);
                    }
                    else
                    {
                        Debug.Log("The audio Source is already being played");
                    }
                }
                else
                {
                    if (instance.activeAudioSources.Count <= instance.maxSounds)
                    {
                        instance.activeAudioSources.Add(audioSource);
                        instance.PlayAudio(audioSource, audioClip);
                    }
                    else
                    {
                        Debug.Log("Maximum Sounds are playing");
                    }
                }
            }
            else
            {
                instance.activeAudioSources.Add(audioSource);
                instance.PlayAudio(audioSource, audioClip);
            }
        }
        #endregion
    }
    /// <summary>
    /// Audio data class
    /// </summary>
    public class AudioData
    {
        public string fileName;
        public bool isFromGeneraBundle;
    }
        
}