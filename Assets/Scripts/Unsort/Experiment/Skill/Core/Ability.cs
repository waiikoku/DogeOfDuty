using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    [CreateAssetMenu(menuName = "Custom/Create Ability")]
    public class Ability : ScriptableObject
    {
        public AbilityData Data;

        public virtual void Activate()
        {

        }
    }
}
