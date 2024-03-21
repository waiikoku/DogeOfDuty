using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    [CreateAssetMenu(fileName = "Enemy",menuName = "Custom/Create Enemy")]
    public class EnemyData : ScriptableObject
    {
        public Identity identity;
        public int health; //unit hp
        public float speed; //unit m/s
    }
}
