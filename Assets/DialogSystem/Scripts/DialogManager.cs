using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    
    [SerializeField]
    GameObject dialogTemplate;
    [SerializeField]
    DialogUi ui;
    bool useUi = false;
    
    void Awake()
    {
        instance = this;
    }

    public GameObject CreateDialog(DialogData data, Dialog dialog)
    {
        if (data.openPopup)
        {
            useUi = true;
            ui.gameObject.SetActive(true);
        }

        if(useUi)
            return ui.CreateDialog(data, dialog);
        
        GameObject template = dialog.dialogTemplate != null ? dialog.dialogTemplate : dialogTemplate;
        GameObject dialogBubble = GameObject.Instantiate(template);
        dialogBubble.transform.SetParent(template.transform.parent, false);

        DialogBubble bubble = dialogBubble.GetComponent<DialogBubble>();
        bubble.dialogData = data;
        bubble.dialog = dialog;
        bubble.text.text = data.dialog;

        dialogBubble.SetActive(true);

        return dialogBubble;
    }
    
    public void DialogComplete(DialogBubble bubble)
    {
        DialogData next = bubble.dialog.next(bubble.dialogData);
        bubble.dialog.say(next);
        if (useUi)
        {
            ui.DialogComplete(bubble, next);
        }
    }
}