using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace  Poptropica2.SFXModule
{
    /// <summary>
    /// This Controller is used to play the ambient tracks
    /// </summary>
    public class AmbientTrack : MonoBehaviour
    {
        [Tooltip("Add Ambient tracks with name. The same name will be used to change the tracks")]
        public List<AmbientTracks> ambientTracks;
        public float fadeInOffset = 0.3f; //Default value : 0.3f
        public float fadeOutOffset = 0.3f; //Default value : 0.03f

        private AudioSource ambientTrackAudioSource;
        private static AmbientTrack instance;
        private string currentTrackName;
        #region External
        /// <summary>
        /// Gets the get instance.
        /// </summary>
        /// <value>The get instance.</value>
        public static AmbientTrack GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AmbientTrack>();
                    if (instance == null)
                    {
                        Debug.LogError ("AmbientTrackController Component is missing. Add component and add ambient tracks to it");
                    }
                }
                return instance;
            }
        }
        /// <summary>
        /// Sets the ambient track volume.
        /// </summary>
        /// <param name="volume">Volume.</param>
        public void SetAmbientTrackVolume (float volume)
        {
            SetAudioSource();
            volume = Mathf.Clamp(volume, 0.0f, 1.0f);
            ambientTrackAudioSource.volume = volume;
        }
        /// <summary>
        /// Gets or sets the current ambient track.
        /// By changing this variable, new track will be playing with the particualar name. If the track not founc, it will play the default one.
        /// </summary>
        /// <value>The current ambient track.</value>
        public string CurrentAmbientTrack
        {
            get
            {
                return currentTrackName;
            }
            set
            {
                if (currentTrackName != value)
                {
                    currentTrackName = value;
                    PlayAmbientTrack(currentTrackName);
                }
            }
        }

        #endregion

        #region Internal
        void Awake ()
        {
            instance = this;
            this.gameObject.AddComponent<DonotDestroyOnLoad>();
            SetAudioSource();
        }

        private void SetAudioSource ()
        {
            if (ambientTrackAudioSource == null)
            {
                ambientTrackAudioSource = this.gameObject.AddComponent<AudioSource>();
            }
        }

        private void PlayAmbientTrack (string trackName)
        {
            StartCoroutine(FadeInAudio(true, trackName));
        }

        private IEnumerator FadeOutAudio ()
        {

            while (ambientTrackAudioSource != null &&ambientTrackAudioSource.volume < 1)
            {
                yield return null;
                if (ambientTrackAudioSource != null)
                {
                    ambientTrackAudioSource.volume += fadeOutOffset * Time.deltaTime;            
                }
            }
        }

        private IEnumerator FadeInAudio (bool isPlayNext, string trackName)
        {
            while (ambientTrackAudioSource != null &&ambientTrackAudioSource.volume > 0 && ambientTrackAudioSource.isPlaying)
            {
                yield return null;
                if (ambientTrackAudioSource != null)
                {
                    ambientTrackAudioSource.volume -= fadeInOffset * Time.deltaTime;
                }

            }
            if (isPlayNext)
            {
                if (ambientTrackAudioSource.isPlaying)
                {
                    ambientTrackAudioSource.Stop();
                }
                ambientTrackAudioSource.clip = GetAmbientTrack(trackName);
                ambientTrackAudioSource.volume = 0;
                ambientTrackAudioSource.Play();
                StartCoroutine(FadeOutAudio());
            }
        }

        private AudioClip GetAmbientTrack (string trackName)
        {
            AudioClip returnClip = null;
            if (ambientTracks != null && ambientTracks.Count > 0)
            {
                foreach (var ambientTrack in ambientTracks)
                {
                    if (ambientTrack.trackName.Equals(trackName))
                    {
                        returnClip = ambientTrack.audioClip;
                        break;
                    }
                }
                if (returnClip == null)
                {
                    returnClip = ambientTracks[0].audioClip;
                    Debug.LogWarning(trackName + " did not found. Returning the default track");
                }
            }
            else
            {
                Debug.LogError("There are no tracks play");
            }
            return returnClip;
        }
        #endregion
    }

    [System.Serializable]
    public class AmbientTracks
    {
        public string trackName;
        public AudioClip audioClip;
    }
}