using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.Events;
using System.Collections.Generic;

public class SceneDialog : MonoBehaviour
{
    const string PREFIX = "/Localization/Dialog/";
    const string XML = ".xml";
    string island;
    string scene;
    string language;
    SceneDialogData dialogData;

    Dictionary<string, string> dialog;

    // Use this for initialization    
    void Start ()
    {
        dialog = new Dictionary<string, string>();
        loadDialog();
    }
    // this should be done via asset bundles, but until then load directly
    public void loadDialog()
    {
        island = PlayerProfileLanguageExample.island;
        scene = gameObject.name;
        language = PlayerProfileLanguageExample.preferedLanuage;

        XmlSerializer serializer = new XmlSerializer(typeof(SceneDialogData));

        string url = PREFIX + island + "/" + language + "/" + scene + XML;

        string fullUrl = Application.dataPath + url;

        Debug.Log("loading path: " + fullUrl);

        try
        {
            //load in data
            FileStream stream = new FileStream(fullUrl, FileMode.Open);

            dialogData = serializer.Deserialize(stream) as SceneDialogData;

            stream.Close();

            Debug.Log("data loaded successfully!");

            for(int i = 0; i < dialogData.dialogData.Count; i++)
            {
                dialog[dialogData.dialogData[i].key] = dialogData.dialogData[i].text;
            }

            updateDialog();
        }
        catch
        {
            Debug.Log(fullUrl + " does not exist. Did you make a typo or format incorrectly?");
        }
    }

    void updateDialog()
    {
        Dialog[] speakers = GameObject.FindObjectsOfType<Dialog>();
        foreach (Dialog speaker in speakers)
        {
            for(int i = 0; i < speaker.dialog.Count; i++)
            {
                DialogData statement = speaker.dialog[i];
                string text = dialog[statement.dialogKey];
                statement.setDialog( text);
                Debug.Log(text);
            }
        }
    }
}