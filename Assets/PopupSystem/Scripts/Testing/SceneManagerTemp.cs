using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Poptropica2.Characters;

// author: Rick Hocker

/// <summary>
/// Scene manager - temporary
/// </summary>
public class SceneManagerTemp : MonoBehaviour
{
    public static SceneManagerTemp instance = null;  // static instance which allows it to be accessed by any other scripts

    public GameObject scene;
    public GameObject player;

    void Awake()
    {
        // check if instance already exists
        if (instance == null)
        {
            // if not, set instance to this
            instance = this;
        }
        // if instance already exists and it's not this:
        else if (instance != this)
        {
            // then destroy this - this enforces our singleton pattern, meaning there can only ever be one instance
            Destroy(gameObject);
        }

        // sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void ShowScene(bool enable)
    {
        // disable all colliders in scene
        EnableColliders(scene, enable);
        scene.SetActive(enable);
        // make player kinematic and disable player
        player.GetComponent<Rigidbody2D>().isKinematic = !enable;
        player.GetComponent<StandardCharacterModel>().isDisabled = !enable;
    }

    private void EnableColliders(GameObject item, bool enable)
    {
        // get collider if any and enable/disable it
        BoxCollider2D collider = item.GetComponent<BoxCollider2D>();
        if (collider)
        {
            collider.enabled = enabled;
        }
        // get children
        int count = item.transform.childCount;
        if (count != 0)
        {
            for (int i = 0; i != count; i++)
            {
                EnableColliders(item.transform.GetChild(i).gameObject, enable);
            }
        }
    }
}