using UnityEngine;
using System.Collections;
[System.Serializable]
public class DialogData
{
    public bool openPopup;
    public string id;
    public string dialogKey;
    public string speaker;
    public string link;
    public string linkEntityId;
    public string eventListener;
    public string triggerEvent;
    public string dialog
    {
        get { return text; }
    }
    string text;

    public void setDialog(string text)
    {
        this.text = text;
    }
}
