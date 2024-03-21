using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dod
{
    public class SceneLoader : MonoBehaviour
    {
        private const float HOLDPROGRESS = 0.9f;
        private const int TENMS = 10;

        public static Action<float> LoadProgress;

        public static void LoadScene(string sceneName, bool instant = false, Action callback = null)
        {
            if (instant)
            {
                SceneManager.LoadScene(sceneName);
                callback?.Invoke();
            }
            else
            {
                LoadAsync(sceneName,callback);
            }
        }

        private static async void LoadAsync(string sceneName, Action callback)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation = false;
            float progress = 0;
            while (op.isDone)
            {
                progress = op.progress;
                print(progress);
                LoadProgress?.Invoke(progress);
                if(progress >= HOLDPROGRESS)
                {
                    op.allowSceneActivation = true;
                }
                await Task.Delay(TENMS);
            }
            //Instant Loaded. Only happend when scene is small So it's fast to load
            if(progress == 0)
            {
                op.allowSceneActivation = true;
            }
            LoadProgress?.Invoke(1f);
            callback?.Invoke();
            await Task.CompletedTask;
        }
    }
}
