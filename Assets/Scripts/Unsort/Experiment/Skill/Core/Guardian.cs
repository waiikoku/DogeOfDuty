using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class Guardian : BaseSkill
    {
        [Header("Position")]
        private Transform m_transform;
        private Vector3 currentPosition;
        private Vector3 targetPosition;
        private Vector3 velocityPosition;
        [SerializeField] private float smoothTime = 0.1f;
        [SerializeField] private Vector3 offset;

        [Header("Spinner")]
        [SerializeField] private float speed;
        [SerializeField] private Vector3 axis;
        [SerializeField] private GameObject prefab;
        [SerializeField] private float radius = 1;
        private const int initSize = 1;
        private List<GameObject> orbiters;

        protected override void Awake()
        {
            base.Awake();
            m_transform = transform;
            InitCircle();
        }

        private void FixedUpdate()
        {
            Follow();
            Rotate();
        }

        private Vector3 direction;
        private const float followSpeed = 10f;
        private void Follow()
        {
            if (m_param.center == null) return;
            targetPosition = m_param.center.position + offset;
            /*
            if (currentPosition == targetPosition) return;
            currentPosition = Vector3.SmoothDamp(currentPosition, targetPosition, ref velocityPosition, smoothTime);
            m_transform.position = currentPosition;
            */
            direction = m_transform.position - targetPosition;
            m_transform.position += followSpeed * Time.fixedDeltaTime * direction.normalized;
        }

        private void Rotate()
        {
            m_transform.Rotate(speed * Time.fixedDeltaTime * axis);
        }

        private void InitCircle()
        {
            orbiters = new List<GameObject>();
            float step = 360f / (float)initSize;
            float angle = 0;
            for (int i = 0; i < initSize; i++)
            {
                GameObject go = Instantiate(prefab, m_transform);
                go.transform.localPosition = PositionCircle(radius, angle * Mathf.Deg2Rad);
                angle += step;
                orbiters.Add(go);
            }
        }

        private void UpgradeCircle(int size)
        {
            float step = 360f / (float)size;
            float angle = 0;
            for (int i = orbiters.Count - 1; i < size; i++)
            {
                GameObject go = Instantiate(prefab, m_transform);
                go.transform.localPosition = PositionCircle(radius, angle * Mathf.Deg2Rad);
                angle += step;
                orbiters.Add(go);
            }
        }

        private void UpgradeCircleByLevel()
        {
            float step = 360f / (float)m_level;
            float angle = 0;
            int count = Mathf.Clamp(m_level - orbiters.Count, 0, 5);
            for (int i = 0; i < count; i++)
            {
                GameObject go = Instantiate(prefab, m_transform);
                go.transform.localPosition = PositionCircle(radius, angle * Mathf.Deg2Rad);
                angle += step;
                orbiters.Add(go);
            }
        }

        private void SpawnOrbiter()
        {
            GameObject go = Instantiate(prefab, m_transform);
            orbiters.Add(go);
            UpdateCircle();
        }

        private void UpdateCircle()
        {
            float step = 360f / (float)orbiters.Count;
            float angle = 0;
            for (int i = 0; i < orbiters.Count; i++)
            {
                orbiters[i].transform.localPosition = PositionCircle(radius, angle * Mathf.Deg2Rad);
                angle += step;
            }
        }

        private Vector3 PositionCircle(float radius, float angle)
        {
            return new Vector3(Mathf.Sin(angle) * radius, 0, Mathf.Cos(angle) * radius);
        }

        public override void Activation(bool active)
        {
            throw new System.NotImplementedException();
        }

        public override void Upgrade()
        {
            base.Upgrade();
            SpawnOrbiter();
        }

        public override void Degrade()
        {
            base.Degrade();
        }
    }
}
