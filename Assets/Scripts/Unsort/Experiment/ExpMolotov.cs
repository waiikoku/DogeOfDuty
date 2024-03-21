using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class ExpMolotov  : BaseSkill
    {
        public Transform center;
        public Rigidbody molotov;

        public float spawnRate = 1;
        public float subSpawnRate = 2;
        public AudioClip clip;

        private float spawnTime;
        private float spawnInterval;
        private Vector3 direction;

        public string targetTag = "Untagged";
        public LayerMask targetLayer;
        public float radius = 1;
        public float angleOffset = 15;
        public float speed = 1;

        public override void Activation(bool active)
        {
            if (isCooldown) return;
            if (active)
            {
                OnCooldown.coroutine = StartCoroutine(Cooldown());
                OnActive.coroutine = StartCoroutine(Active());
            }
            else
            {
                if (OnActive.coroutine != null)
                {
                    StopCoroutine(OnActive.coroutine);
                }
            }
        }

        protected override void ActiveStart()
        {
            spawnInterval = 1f / spawnRate;
        }

        protected override void ActiveEnd()
        {
            base.ActiveEnd();
        }

        protected override void ActiveUpdate()
        {
            if (spawnTime > Time.time) return;
            spawnTime = Time.time + spawnInterval;
            Spawn();
        }

        [SerializeField] private float size = 1f;
        [SerializeField] private float angle = 45f;
        private Vector3 angleEuler;
        private Quaternion throwRot;
        private Vector3 throwAngle;

        private void Spawn()
        {
            Collider[] cols = Physics.OverlapSphere(center.position, radius, targetLayer);
            float dist = Mathf.Infinity;
            Collider least = null;
            foreach (var col in cols)
            {
                if (col.CompareTag(targetTag))
                {
                    direction = col.transform.position - center.position;
                    direction.y = 0;
                    if (direction.magnitude < dist)
                    {
                        dist = direction.magnitude;
                        least = col;
                    }
                }
            }
            if (least == null) return;
            direction = least.transform.position - center.position;
            direction.y = 0;
            float angle = ConvertAngle();
            Vector3 localCenter = center.position;
            angleEuler.x = angle;
            angleEuler.y = center.rotation.eulerAngles.y;
            throwRot = Quaternion.Euler(angleEuler);
            throwAngle = throwRot * Vector3.up;

            for (int i = 0; i < subSpawnRate; i++)
            {
                Vector3 pos = CircleXZ(size, angle);
                angle += angleOffset;
                MolotovV2(localCenter,pos, throwAngle);
            }
        }

        private void MolotovV1()
        {
            Rigidbody go = Instantiate(molotov, center.position + size * direction.normalized, Quaternion.LookRotation(Vector3.up, direction));
            angleEuler.x = angle;
            angleEuler.y = center.rotation.eulerAngles.y;
            throwRot = Quaternion.Euler(angleEuler);
            throwAngle = throwRot * Vector3.up;
            go.velocity = speed * (direction.normalized + throwAngle);
        }

        private void MolotovV2(Vector3 center, Vector3 dir,Vector3 throwAngle = default)
        {
            Vector3 origin = center + dir;
            Rigidbody go = Instantiate(molotov, origin, Quaternion.identity);
            go.velocity = speed * (direction.normalized + throwAngle);
        }

        private Vector3 CircleXZ(float radius, float angle)
        {
            return new Vector3(Mathf.Sin(angle) * radius, 0, Mathf.Cos(angle) * radius);
        }

        private const float fullCircle = 360;
        private float ConvertAngle()
        {
            float angle = Vector3.Angle(center.forward, direction);
            Vector3 crossProduct = Vector3.Cross(center.forward, direction);
            if (crossProduct.y < 0)
            {
                angle = fullCircle - angle; // Adjust the angle if needed
            }
            return angle;
        }


        [SerializeField] private UnityEngine.Color[] colors;
        private void OnDrawGizmos()
        {
            Gizmos.color = colors[0];
            Gizmos.DrawWireSphere(center.position, radius);
            Gizmos.color = colors[1];
            Gizmos.DrawLine(center.position, center.position + throwAngle * 1f);
        }
        protected override void CooldownEnd()
        {
            base.CooldownEnd();
        }

        protected override void CooldownUpdate()
        {
            base.CooldownUpdate();
        }
    }
}
