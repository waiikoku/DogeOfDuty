using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    [System.Serializable]
    public struct Sound
    {
        public int Id;
        [Range(0f,1f)]
        public float Volume;
        public string Name;
        public AudioClip Clip;

    }
}
