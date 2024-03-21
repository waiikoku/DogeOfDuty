using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public enum SkillType
    {
        Undefined,
        Passive,
        Active
    }

    [CreateAssetMenu(fileName = "Skill", menuName = "Custom/Create Skill")]
    public class SkillCard : ScriptableObject
    {
        public Identity Identity;
        [TextArea]
        public string SkillDesc;
        public Sprite SkillIcon;
        public SkillStatTier SkillStats;
        public SkillType SkillType = SkillType.Undefined;
    }
}
