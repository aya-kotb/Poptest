using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialog : MonoBehaviour
{
    public string displayName;
    public Color dialogColor;
    public Vector2 portraitOffset;

    public List<DialogData> dialog;

    [HideInInspector]
    public DialogData current;

    bool isSpeaking;

    List<DialogBubble> bubbles;
    [Tooltip("Unique Dialog Prefab")]
    public GameObject dialogTemplate;

    void Start()
    {
        bubbles = new List<DialogBubble>();
        if (dialog.Count > 0)
        {
            current = dialog[0];
            for (int i = 0; i < dialog.Count; i++)
            {
                // if the dialogs event == true then set it as the current dialog
            }
        }

    }

    public void RemoveBubble(DialogBubble bubble)
    {
        bubbles.Remove(bubble);
        GameObject.Destroy(bubble.gameObject);
    }

    public void removeBubbleById(string dialogId)
    {
        for(int i = 0; i < bubbles.Count; i++)
        {
            DialogBubble bubble = bubbles[i];
            if(bubble.dialogData.id == dialogId)
            {
                bubbles.Remove(bubble);
                GameObject.Destroy(bubble.gameObject);
                return;
            }
        }
    }

    public void removeAll()
    {
        for (int i = 0; i < bubbles.Count; i++)
        {
            DialogBubble bubble = bubbles[i];
            bubbles.Remove(bubble);
            i--;
            GameObject.Destroy(bubble.gameObject);
        }
    }

    public void setCurrentById(string id)
    {
        for(int i = 0; i < dialog.Count; i++)
        {
            DialogData data = dialog[i];
            if (data.id == id || data.eventListener == id)
                current = data;
        }
    }

    public void sayById(string id)
    {
        for (int i = 0; i < dialog.Count; i++)
        {
            DialogData data = dialog[i];
            if (data.id == id || data.eventListener == id)
            {
                say(data);
                return;
            }
        }
        Debug.Log("could not find dialog of id: " + id);
    }

    public void sayCurrent()
    {
        say(current);
    }

    public void say(DialogData data)
    {
        if (data == null)
            return;
        GameObject dialogBubble = DialogManager.instance.CreateDialog(data, this);
        bubbles.Add(dialogBubble.GetComponent<DialogBubble>());
    }

    public DialogData next(DialogData step)
    {
        if (step.link == "")//there is no next step
        {
            return null;
        }
        if(step.link.ToLower() == "next")//should say my next line of dialog
        {
            int index = dialog.IndexOf(step) + 1;
            if (index < dialog.Count && index > 0)// there is a next line
            {
                return dialog[index];
            }
        }
        else
        {
            if (step.linkEntityId != "")//some one else is supposed to speak
            {
                GameObject entity = GameObject.Find(step.linkEntityId);
                if(entity)// they exist
                {
                    Dialog entityDialog = entity.GetComponent<Dialog>();
                    if(entityDialog)//they got something to say
                    {
                        return entityDialog.GetDialogById(step.link);
                    }
                }
            }
            else// I have a specific dialog out of order to say
            {
                return GetDialogById(step.link);
            }
        }
        return null;
    }
    
    public DialogData GetDialogById(string id)
    {
        for (int i = 0; i < dialog.Count; i++)
        {
            if (dialog[i].id == id)
                return dialog[i];
        }
        return null;
    }
}