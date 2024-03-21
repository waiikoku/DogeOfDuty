using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    [System.Serializable]
    public struct AbilityData
    {
        public AbilityIdentity identity;
        public AbilityTime time;
        public AbilityStat stat;
    }

    [System.Serializable]
    public struct AbilityIdentity
    {
        public int ID;
        public string Name;
    }

    [System.Serializable]
    public struct AbilityTime
    {
        public float Cooldowntime;
        public float ActiveTime;
    }

    [System.Serializable]
    public struct AbilityStat
    {
        public float strength; //Power
        public float effiency; //CD
        public float duration; //Time
        public float range; //Length
    }
}
