using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class ExpMagnet1 : BaseSkill
    {
        public LayerMask targetLayer;
        public float radius = 1f;
        public float speed = 1f;

        private Collider[] cols;
        private int detect;
        private Vector3 dir;
        private const int size = 64;
        private float delta;
        private float deltaSpeed;

        private bool activation = false;
        protected override void Awake()
        {
            base.Awake();
            cols = new Collider[size];
        }

        private void Start()
        {
            IEnumerator Thread()
            {
                while (m_param.center == null)
                {
                    yield return new WaitForEndOfFrame();
                }
                print("Active Magnet");
                Activation(true);
            }

            StartCoroutine(Thread());
        }
        private void FixedUpdate()
        {
            if (activation == false) return;
            detect = Physics.OverlapSphereNonAlloc(m_param.center.position, radius,cols, targetLayer);
            delta = Time.fixedDeltaTime;
            deltaSpeed = speed * delta;
            for (int i = 0; i < detect; i++)
            {
                dir = m_param.center.position - cols[i].transform.position;
                cols[i].transform.position += deltaSpeed * dir.normalized;
            }
        }

#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] private Color color;
        private void OnDrawGizmos()
        {
            if (m_param.center == null) return;
            Gizmos.color = color;
            Gizmos.DrawWireSphere(m_param.center.position, radius);
        }
#endif
        public override void Activation(bool active)
        {
            activation = active;
        }

        public override void Upgrade()
        {
            base.Upgrade();
            radius += 1f;
        }
    }
}
