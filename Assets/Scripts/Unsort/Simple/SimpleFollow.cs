using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class SimpleFollow : MonoBehaviour
    {
        private Transform m_transform;
        [SerializeField] private Transform target;
        private Vector3 offset;
        private enum Mode
        {
            Exact,
            Offset
        }
        [SerializeField] private Mode mode;

        [SerializeField] private bool smoothFollow = false;
        private Vector3 targetPosition;
        private Vector3 currentPosition;
        private Vector3 velocityPosition;
        [SerializeField] private float smoothTime = 1f;
        private void Awake()
        {
            m_transform = transform;
        }

        private void FixedUpdate()
        {
            if (target == null) return;
            switch (mode)
            {
                case Mode.Exact:
                    targetPosition = target.position;
                    break;
                case Mode.Offset:
                    targetPosition = offset + target.position;
                    break;
                default:
                    break;
            }

            if (smoothFollow)
            {
                if (currentPosition == targetPosition) return;
                currentPosition = Vector3.SmoothDamp(currentPosition,targetPosition,ref velocityPosition,smoothTime);
                m_transform.position = currentPosition;
            }
            else
            {
                m_transform.position = targetPosition;
            }
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
            switch (mode)
            {
                case Mode.Exact:
                    break;
                case Mode.Offset:
                    offset = m_transform.position - target.position;
                    break;
                default:
                    break;
            }
        }
    }
}
