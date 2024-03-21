using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Save")]
[System.Serializable]
public class SaveData : XmlData<SaveData>
{
    public float money;
}
