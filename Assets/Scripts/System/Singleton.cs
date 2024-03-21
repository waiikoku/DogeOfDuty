using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T m_instance;
        public static T Instance
        {
            get { return m_instance; }
        }

        protected virtual void Awake()
        {
            if(m_instance == null)
            {
                m_instance = this as T;
            }
            else
            {
                Debug.LogWarning("[Self Destruct] Found Duplicate of " + nameof(T));
                Destroy(gameObject);
                return;
            }
        }

        public void SelfDestruct()
        {
            m_instance = null;
        }
    }
}
