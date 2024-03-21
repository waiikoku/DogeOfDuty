using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class Forcefield : BaseSkill
    {
        public Transform center;
        public SphereCollider zone;
        public GameObject vfx;
        [SerializeField] private ParticleSystem particle;
        public float dmg;
        public float interval;

        private float time;
        public LayerMask targetLayer;
        public string targetTag = "Untagged";

        ParticleSystem.MainModule psMain;

        protected override void Awake()
        {
            base.Awake();
            psMain = particle.main;
        }
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
            UpdateSize();
            vfx.SetActive(true);
        }

        protected override void ActiveEnd()
        {
            vfx.SetActive(false);
        }

        protected override void ActiveUpdate()
        {
            if (time > Time.time) return;
            time = Time.time + interval;
            DealtDamage();
        }

        private void DealtDamage()
        {
            Collider[] cols = Physics.OverlapSphere(center.position, zone.radius, targetLayer);
            foreach (var col in cols)
            {
                if (col.CompareTag(targetTag) == false) continue;
                if (col.TryGetComponent(out IDamagable damagable))
                {
                    damagable.TakeDamage(dmg);
                }
            }
        }

        private void UpdateSize()
        {
            IEnumerator LocalSize()
            {
                float value = zone.radius * 2 * 2;
                ParticleSystem.MinMaxCurve curve = new ParticleSystem.MinMaxCurve();
                curve.constant = value;
                yield return new WaitForEndOfFrame();
                psMain.startSize = curve;
                yield return new WaitForEndOfFrame();
                print($"Update Size: C({value}) R({psMain.startSize.constant})");
            }
            StartCoroutine(LocalSize());
        }

        public override void Upgrade()
        {
            base.Upgrade();
            zone.radius += 0.5f;
            UpdateSize();
        }

    }
}
