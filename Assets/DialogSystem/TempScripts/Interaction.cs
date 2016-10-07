using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    public UnityEvent click;

    bool selected;
    
    void OnMouseDown()
    {
        selected = true;
    }

    void OnMouseUp()
    {
        if(selected)
            click.Invoke();
        selected = false;
    }
}
