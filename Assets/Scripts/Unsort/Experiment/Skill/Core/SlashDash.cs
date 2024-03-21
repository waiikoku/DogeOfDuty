using Roguelike;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dod
{
    public class SlashDash : BaseSkill
    {
        public Animator characterAnim;
        public GameObject dashEffect;
        public AudioClip clip;

        public LayerMask targetLayer;
        public string targetTag = "Untagged";

        public Transform center;
        public Vector3 hitBox;
        public float dmg;

        private Collider[] currentColliders;
        private Collider[] previousColliders;
        private Dictionary<Collider, bool> colliderDictionary;

        protected override void Awake()
        {
            base.Awake();
            colliderDictionary = new Dictionary<Collider, bool>();
        }

        public override void Activation(bool active)
        {
            throw new System.NotImplementedException();
        }

        protected override void ActiveStart()
        {
            characterAnim.SetTrigger("");
            dashEffect.SetActive(true);
            AudioSource.PlayClipAtPoint(clip, center.position);
        }

        protected override void ActiveEnd()
        {
            colliderDictionary.Clear();
            characterAnim.SetTrigger("");
            dashEffect.SetActive(false);
        }

        protected override void ActiveUpdate()
        {
            currentColliders = Physics.OverlapBox(center.position, hitBox / 2f, center.rotation, targetLayer);
            foreach (Collider col in currentColliders)
            {
                if (CheckTag(col))
                {
                    if(colliderDictionary.ContainsKey(col))
                    {
                        continue;
                    }
                    else
                    {
                        VirtualTriggerEnter(col);
                        colliderDictionary.Add(col,true);
                    }
                }
                else
                {
                    continue;
                }
            }
            IEnumerable<Collider> lostChildren = previousColliders.Except(currentColliders);
            foreach (var child in lostChildren)
            {
                VirtualTriggerExit(child);
            }
            previousColliders = currentColliders;
        }

        private bool CheckTag(Collider col)
        {
            return col.CompareTag(targetTag);
        }

        private void VirtualTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(dmg);
            }
        }

        private void VirtualTriggerExit(Collider other)
        {
            colliderDictionary.Remove(other);
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
