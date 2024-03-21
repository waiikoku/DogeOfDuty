using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private GameObject manager;
        [SerializeField] private string menuName;
        private void Awake()
        {
            DontDestroyOnLoad(manager);
            SceneLoader.LoadScene(menuName,true);
        }
    }
}
