using UnityEngine;
using System.Collections;

public class DonotDestroyOnLoad : MonoBehaviour
{
    void Awake ()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
