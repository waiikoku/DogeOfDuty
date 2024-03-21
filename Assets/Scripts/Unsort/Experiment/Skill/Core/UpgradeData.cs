using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "Custom/Create Upgrade")]
    public class UpgradeData : ScriptableObject
    {
        [SerializeField] protected SkillCard card;
    }
}
