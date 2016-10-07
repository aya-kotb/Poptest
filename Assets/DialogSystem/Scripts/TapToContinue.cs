using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class TapToContinue : MonoBehaviour
{
    public UnityEvent onTap;
    
    // Update is called once per frame
    void Update ()
    {
        bool tapped = false;

        if (Input.GetMouseButtonDown(0))
            tapped = true;
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
                tapped = true;
        }

        if (tapped)
        {
            onTap.Invoke();
        }
    }
}
