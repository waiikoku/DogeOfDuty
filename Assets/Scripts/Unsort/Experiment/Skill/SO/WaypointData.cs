using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    [CreateAssetMenu(fileName = "Point", menuName = "Custom/Create Point")]
    public class WaypointData : ScriptableObject
    {
        public static Dictionary<int, Transform> Points = new Dictionary<int, Transform>();
        
        public int pointID;
        public string pointName;
    }
}
