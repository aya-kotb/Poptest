using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogBubble : MonoBehaviour
{
    public Text text;
    [HideInInspector]
    public Dialog dialog;
    [HideInInspector]
    public DialogData dialogData;

    public UnityEvent onStart,onComplete;

    bool completed;

    void Start()
    {
        onStart.Invoke();
    }

    public void complete()
    {
        if(!completed)
        {
            completed = true;
            DialogManager.instance.DialogComplete(this);
            onComplete.Invoke();
        }
    }

    public void Remove()
    {
        dialog.RemoveBubble(this);
    }
}