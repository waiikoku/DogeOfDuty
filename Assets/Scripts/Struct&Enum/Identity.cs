using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    [System.Serializable]
    public struct Identity
    {
        public int ID;
        public string Name;

        /// <summary>
        /// Compare Name
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool Compare(string target)
        {
            return this.Name.Equals(target);
        }

        /// <summary>
        /// Compare ID
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool Compare(int target)
        {
            return this.ID.Equals(target);
        }
    }
}
