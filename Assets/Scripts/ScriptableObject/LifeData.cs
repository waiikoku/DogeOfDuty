using UnityEngine;

namespace Roguelike
{
    [CreateAssetMenu(fileName = "Life Data",menuName = "Custom/Data/Create LifeProfile")]
    public class LifeData : ScriptableObject
    {
        //One to One Relationship
        [SerializeField] private uint profileId; //Primary Key
        [SerializeField] private Life m_stat;
        public Life Stat
        {
            get
            {
                return m_stat;
            }
        }
    }
}
