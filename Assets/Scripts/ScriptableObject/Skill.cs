using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Custom/Data/Create Skill")]
public class Skill : ScriptableObject
{
    public uint skill_ID;
    public string skillName;
    public Sprite skillIcon;
    [TextArea]
    public string skillDesc;
    public SkillType skillType;
    public SkillAttribute[] skillTiers;

    public virtual void Activate()
    {

    }

    public virtual void Deactivate()
    {

    }
}
