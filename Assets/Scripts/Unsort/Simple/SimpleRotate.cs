using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class SimpleRotate : MonoBehaviour
    {
        private Transform m_transform;
        public Vector3 axis;
        [SerializeField] private float speed;
        private void Awake()
        {
            m_transform = transform;
        }

        private void FixedUpdate()
        {
            m_transform.Rotate(speed * Time.fixedDeltaTime * axis);
        }
    }
}
