using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dod
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Button playBtn;
        [SerializeField] private string nextScene;
        private void Awake()
        {
            if (playBtn != null)
            {
                playBtn.onClick.AddListener(Play);
            }
        }

        public void Play()
        {
            playBtn.interactable = false;
            SceneLoader.LoadScene(nextScene,false,() =>
            {
                GameManager.Instance.StartGame();
            });
        }

        public void DirectLoad(string sceneName)
        {
            SceneLoader.LoadScene(sceneName,false);
        }

        public void ReturnMenu(string sceneName)
        {
            SceneLoader.LoadScene(sceneName, false, Despawn);
        }
        public void RestartGame(string sceneName)
        {
            SceneLoader.LoadScene(sceneName, false, DespawnAndPlay);
        }
        private async void Despawn()
        {
            GameManager.Instance.Despawn();
            await UIManager.Instance.Despawn();
        }

        private async void DespawnAndPlay()
        {
            GameManager.Instance.Despawn();
            await UIManager.Instance.Despawn();
            GameManager.Instance.StartGame();
        }
    }
}
