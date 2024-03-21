using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class Rocket : BaseSkill
    {
        public Transform center;
        public Rigidbody rocket;

        public float spawnRate = 1;
        public AudioClip clip;

        private float spawnTime;
        private float spawnInterval;
        private Vector3 direction;

        public string targetTag = "Untagged";
        public LayerMask targetLayer;
        public float radius = 1;

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
            Rigidbody go = Instantiate(rocket, center.position, Quaternion.LookRotation(Vector3.up, direction));
            go.velocity = speed * direction.normalized;
        }

        protected override void CooldownEnd()
        {
            base.CooldownEnd();
        }

        protected override void CooldownUpdate()
        {
            base.CooldownUpdate();
        }

        public override void Upgrade()
        {
            base.Upgrade();
            spawnRate += 1f;
        }
    }
}
