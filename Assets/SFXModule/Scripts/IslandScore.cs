using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Poptropica2.SFXModule 
{
    /// <summary>
    /// IslandScore : Component for playing the IslandScore
    /// islandScoreClip : Clip need to assign for the Island Score
    /// IslandScorePriority : Get or Set the Island Priority- If there are multiple IslandScore component, then the IslandScore has much priority will play and rest will be inactive
    /// FadeInAudio (AudioSource _fadeaudio, float _offset, bool isstop) : FadeIn the current playing track
    /// FadeOutAudio (AudioSource _fadeoutaudio, float _offset) : Fadeout the FadeIn track
    /// </summary>
    public class IslandScore : MonoBehaviour {

        #region Variables
        public AudioClip islandScoreClip;
        public int islandPriority;
        public AudioSource islandAudioSource;
        public bool isPlaying;
        public float fadeInOffset = 0.3f; //Default value : 0.3f
        public float fadeOutOffset = 0.03f; //Default value : 0.03f

        private bool gotAudioClip = false;
        #endregion

        #region External

        /// <summary>
        /// Fade in the current playing Island score.
        /// isstop : Will stop the IslandScore after fading
        /// </summary>
        /// <returns>The in audio.</returns>
        public IEnumerator FadeInAudio (AudioSource fadeaudio, bool isstop, bool isDestroy = false)
        {
            if (fadeaudio == null)
            {
                fadeaudio = islandAudioSource;
            }
            while (fadeaudio != null &&fadeaudio.volume > 0 && fadeaudio.isPlaying)
            {
                yield return null;
                if (fadeaudio != null)
                {
                    fadeaudio.volume -= fadeInOffset * Time.deltaTime;
                }

            }
            if (isstop)
            {
                islandAudioSource.Stop();
                islandAudioSource.clip = null;
                this.gameObject.SetActive(false);
            }
            if(isDestroy)
            {
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// Fade out the current playing Island score
        /// </summary>
        public IEnumerator FadeOutAudio (AudioSource fadeoutaudio)
        {
            if (fadeoutaudio == null)
            {
                fadeoutaudio = islandAudioSource;
            }
            while (fadeoutaudio != null &&fadeoutaudio.volume < 1)
            {
                yield return null;
                if (fadeoutaudio != null)
                {
                    fadeoutaudio.volume += fadeOutOffset * Time.deltaTime;            
                }
            }
        }

        /// <summary>
        /// Get or set the IslandScorePriority
        /// </summary>
        /// <value><c>true</c> if this instance island score priority; otherwise, <c>false</c>.</value>
        public int IslandScorePriority 
        {
            get
            {
                return islandPriority;
            }
            set
            {
                islandPriority = value;
            }
        }
        /// <summary>
        /// Stops the island score
        /// If in case to stop the current IslandScore being played
        /// </summary>
        public void StopIslandScore ()
        {
            StartCoroutine(FadeInAudio (islandAudioSource, true));
        }
        /// <summary>
        /// Stops the island score and move to the specified scene
        /// </summary>
        /// <param name="goNextScene">If set to <c>true</c>will move to the next scene</param>
        /// <param name="sceneName">Scene name.</param>
        public void StopIslandScore (bool isDestroy)
        {
            StartCoroutine(FadeInAudio (islandAudioSource, true, true));
        }
        #endregion

        #region Internal

        void OnEnable ()
        {
            if (islandAudioSource == null)
            {
                if (GetComponent<AudioSource> () == null)
                {
                    this.gameObject.AddComponent <AudioSource>();
                }
                islandAudioSource =  GetComponent<AudioSource> ();
            }
            //Get the audio clip if clip is empty
            if (!gotAudioClip)
            {
                GetIslandScore ();
            }
            //Play
            PlayIslandScore ();
        }

        /// <summary>
        /// Gets the island score.
        /// </summary>
        private void GetIslandScore ()
        {
            islandAudioSource.clip = islandScoreClip;
            if (islandAudioSource.clip != null)
            {
                gotAudioClip = true;
            }
        }
        /// <summary>
        /// Play the island score.
        /// </summary>
        private void PlayIslandScore ()
        {
            islandAudioSource.loop = true;
            islandAudioSource.volume = 0.0f;
            islandAudioSource.Play();
            StartCoroutine (FadeOutAudio (islandAudioSource));
        }
        #endregion
    }
}


