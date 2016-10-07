using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
[XmlRoot("scene")]
public class SceneDialogData
{
    [XmlArray("dialog")]
    [XmlArrayItem("text")]
    public List<DialogTextData> dialogData;
}
