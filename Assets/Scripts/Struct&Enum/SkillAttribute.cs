using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SkillAttribute
{
    public float minDmg;
    public float maxDmg;
    public float minRange;
    public float maxRange;
    public float castingTime;
    public float duration;
    public float cooldown;
    public float speed;

    [Header("External")]
    public string command;
}
