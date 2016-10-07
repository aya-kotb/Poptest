using UnityEngine;
using System.Collections;
namespace Poptropica2.SFXModule 
{
    /// <summary>
    /// AmbientSound : This component used to play the Ambient Sound
    /// ambientSound : Ambient sound clip for playing the Ambient Music
    /// maxListeningDistance : Sets the Maximum listener distance
    /// </summary>
    public class AmbientSound : MonoBehaviour
    {
        public AudioClip ambientSound;
        public float maxListeningDistance = 12.0f;
        private AudioSource ambientSoundSource;

        void OnEnable ()
        {
            PlayAmbientSound ();
        }

        private void PlayAmbientSound ()
        {
            if (ambientSoundSource == null)
            {
                this.gameObject.AddComponent<AudioSource>();
                ambientSoundSource = GetComponent<AudioSource>();
                ambientSoundSource.spatialBlend = 1.0f;
                ambientSoundSource.rolloffMode = AudioRolloffMode.Linear;
            }
            if (ambientSound != null)
            {
                ambientSoundSource.clip = ambientSound;
                ambientSoundSource.loop = true;
                ambientSoundSource.Play();
                ambientSoundSource.maxDistance = maxListeningDistance;
            }
            else
            {
                Debug.Log ("Clip is null");
            }
        }
    }   
}

