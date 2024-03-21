using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class ThemeSignal : MonoBehaviour
    {
        [SerializeField] private bool playOnStart;
        [SerializeField] private Soundbank.BGM bgm;

        private void Start()
        {
            if (playOnStart) Play();
        }
        public void Play()
        {
            SoundManager.Instance.PlayBgmById((int)bgm);
        }
    }
}
