using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    [CreateAssetMenu(fileName = "Album",menuName = "Custom/Create Album")]
    public class SoundAlbum : ScriptableObject
    {
        public int albumId;
        public string albumName;
        public Sound[] sounds;

        private void OnValidate()
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].Id = i;
                if (string.IsNullOrEmpty(sounds[i].Name))
                {
                    sounds[i].Name = $"Sound_{i}";
                }
            }
        }
    }
}
