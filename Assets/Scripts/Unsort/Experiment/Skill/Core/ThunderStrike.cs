using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class ThunderStrike : BaseSkill
    {
        public Transform center;
        public GameObject lightning;

        public float spawnRate = 1;
        public float radius = 1;
        public AudioClip clip;

        private float spawnTime;
        private float spawnInterval;

        public float dmg = 1;
        public float lifeTime = 3;
        private const string targetTag = "Enemy";
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
                if(OnActive.coroutine != null)
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
            Vector3 offset = Vector3.zero;
            offset.x = Random.Range(-1.0f,1.0f) * radius;
            offset.z = Random.Range(-1.0f, 1.0f) * radius;
            Vector3 pos = center.position + offset;
            Destroy(Instantiate(lightning, pos, Quaternion.identity), lifeTime);
            DealtDamage(pos);
        }

        private void DealtDamage(Vector3 pos)
        {
            Collider[] cols = Physics.OverlapSphere(pos, 1);
            foreach (var col in cols)
            {
                if (col.CompareTag(targetTag) == false) continue;
                if (col.TryGetComponent(out IDamagable damagable))
                {
                    damagable.TakeDamage(dmg);
                }
            }
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
