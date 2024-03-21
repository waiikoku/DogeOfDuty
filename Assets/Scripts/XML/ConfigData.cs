using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Settings")]
[System.Serializable]
public class ConfigData : XmlData<ConfigData>
{
    public Config Config;
}

[System.Serializable]
public struct Config
{
    //Screen
    public bool isFullscreen;
    public bool isBorderless;
    public bool isVsync;
    public Vector2 Resolution_Fullscreen;
    public Vector2 Resolution_Windowed;

    //Sound
    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;
    public float uiVolume;

    //Graphic
    public int targetFps;
    public float mouseSensitivity;
    public int fieldOfView;
}