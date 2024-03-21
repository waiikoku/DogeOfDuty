using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Audio;

namespace Dod
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField] private AudioMixer mixer;

        [Header("Players")]
        [SerializeField] private AudioSource bgmPlayer;
        [SerializeField] private AudioSource sfxPlayer;
        [SerializeField] private AudioSource uiPlayer;

        [Header("Album")]
        [SerializeField] private SoundAlbum m_bgm;
        [SerializeField] private SoundAlbum m_ambient;
        [SerializeField] private SoundAlbum m_sfx;
        [SerializeField] private SoundAlbum m_ui;

        public enum Type
        {
            MST,
            BGM,
            SFX,
            UI,
        }


        [Header("Volumes")]
        [Range(0f, 1f)]
        public float Master;
        [Range(0f, 1f)]
        public float Bgm;
        [Range(0f, 1f)]
        public float Sfx;
        [Range(0f, 1f)]
        public float Ui;

        public void Init()
        {
            bgmPlayer.volume = Master * Bgm;
            sfxPlayer.volume = Master * Sfx;
            uiPlayer.volume = Master * Ui;
        }
        public void LoadConfig(ConfigData data)
        {
            Master = data.Config.masterVolume;
            Bgm = data.Config.bgmVolume;
            Sfx = data.Config.sfxVolume;
            Ui = data.Config.uiVolume;
        }

        #region Listener
        //Events
        private event Action<float> OnMaster;
        private event Action<float> OnBGM;
        private event Action<float> OnSFX;
        private event Action<float> OnUI;

        public void AddMasterListener(Action<float> callback)
        {
            OnMaster += callback;
        }
        public void AddBGMListener(Action<float> callback)
        {
            OnBGM += callback;
        }
        public void AddSFXListener(Action<float> callback)
        {
            OnSFX += callback;
        }
        public void AddUIListener(Action<float> callback)
        {
            OnUI += callback;
        }
        #endregion
        #region Adjust
        public void ChangeMasterVolume(float volume)
        {
            Master = volume;
            OnMaster?.Invoke(Master);
        }
        public void ChangeBgmVolume(float volume)
        {
            Bgm = volume;
            OnBGM?.Invoke(Bgm * Master);
        }
        public void ChangeSfxVolume(float volume)
        {
            Sfx = volume;
            OnSFX?.Invoke(Sfx * Master);
        }
        public void ChangeUIVolume(float volume)
        {
            Ui = volume;
            OnUI?.Invoke(Ui * Master);
        }
        #endregion
        #region Functional
        public void Play()
        {
            bgmPlayer.Play();
        }

        public void Stop()
        {
            bgmPlayer?.Stop();
        }

        public void Pause()
        {
            bgmPlayer.Pause();
        }

        public void Unpause()
        {
            bgmPlayer.UnPause();
        }

        public void Change(Sound sound)
        {
            print("Change BGM " + sound.Name);

            if (bgmPlayer.isPlaying)
            {
                Stop();
            }
            bgmPlayer.clip = sound.Clip;
            bgmPlayer.volume = sound.Volume * Bgm * Master;
            bgmPlayer.Play();
        }

        public void PlayBgmById(int id)
        {
            Change(SearchBGM(id));
        }

        public void PlaySfxById(int id)
        {
            SfxPlay(SearchSFX(id));
        }

        public void PlayUiById(int id)
        {
            UiPlay(SearchUI(id));
        }

        public void SfxPlay(Sound sound)
        {
            sfxPlayer.PlayOneShot(sound.Clip, sound.Volume * Sfx * Master);
        }

        public void UiPlay(Sound sound)
        {
            uiPlayer.PlayOneShot(sound.Clip, sound.Volume * Ui * Master);
        }

        private Sound GenericSearch(SoundAlbum album, int id)
        {
            return album.sounds.Single(sound => sound.Id == id);
        }

        private Sound SearchBGM(int id)
        {
            return GenericSearch(m_bgm, id);
        }

        private Sound SearchSFX(int id)
        {
            return GenericSearch(m_sfx, id);
        }

        private Sound SearchUI(int id)
        {
            return GenericSearch(m_ui, id);
        }

        public SoundAlbum[] GetAlbums()
        {
            return new SoundAlbum[3] { m_bgm, m_sfx, m_ui };
        }

        public float GetVolume(Type type)
        {
            switch (type)
            {
                case Type.MST:
                    return Master;
                case Type.BGM:
                    return Bgm;
                case Type.SFX:
                    return Sfx;
                case Type.UI:
                    return Ui;
                default:
                    return 0;
            }
        }
        #endregion

        public void AdjustmentV2(Type type,float volume)
        {
            string keyName = "";
            switch (type)
            {
                case Type.MST:
                    keyName = "Master";
                    break;
                case Type.BGM:
                    keyName = "BGM";
                    break;
                case Type.SFX:
                    keyName = "Effect";
                    break;
                case Type.UI:
                    keyName = "UI";
                    break;
                default:
                    break;
            }
            mixer.SetFloat(keyName, volume);
        }
    }
}
