using UnityEngine;
using System.Collections;
using Poptropica2.SFXModule;
/// <summary>
/// AmbientSoundController : Controlls all the Ambient Sounds in the scene
/// GetInstance : Get the AmbientSoundController instance
/// PlayAmbientSounds () : Plays all ambient component is the scene
/// </summary>
public class AmbientSoundController : MonoBehaviour
{
    #region Variables
    private AmbientSound [] ambientSounds;
    private static AmbientSoundController instance;
    #endregion

    void Start ()
    {
        Init ();
    }

    private void Init ()
    {
        ambientSounds = FindObjectsOfType<AmbientSound> ();
        Debug.Log ("Ambient sounds " + ambientSounds.Length);
        foreach (var ambientSound in ambientSounds)
        {
            ambientSound.gameObject.SetActive(false);
        }
    }

    public void PlayAmbientSounds ()
    {
        foreach (var ambientSound in ambientSounds)
        {
            ambientSound.gameObject.SetActive(true);
        }
    }

    public static AmbientSoundController GetInstance
    {
        get
        {
            if (instance == null)
            {
                AmbientSoundController ambientSoundController = FindObjectOfType<AmbientSoundController>() ;
                if (ambientSoundController == null)
                {
                    GameObject ng = new GameObject ();
                    ng.name = "AmbientSoundController";
                    ng.AddComponent <AmbientSoundController>();
                    instance = ng.GetComponent<AmbientSoundController>();
                }
                else 
                {
                    instance =  ambientSoundController;
                }
            }
            return instance;
        }
    }
}
