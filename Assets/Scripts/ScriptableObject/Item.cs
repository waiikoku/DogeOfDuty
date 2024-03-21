using UnityEngine;

namespace Roguelike
{
    [CreateAssetMenu(fileName = "Item",menuName = "Custom/Data/Create Item")]
    public class Item : ScriptableObject
    {
        public int ItemId; //Must be Unique ID not duplicate So it's can be use to Find/Search
        public string ItemName;
        [TextArea]
        public string ItemDesc;
        public Sprite ItemIcon;
    }
}
