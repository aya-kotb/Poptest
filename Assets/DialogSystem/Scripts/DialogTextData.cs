using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

public class DialogTextData
{
    [XmlAttribute("key")]
    public string key;
    [XmlText]
    public string text;
}
