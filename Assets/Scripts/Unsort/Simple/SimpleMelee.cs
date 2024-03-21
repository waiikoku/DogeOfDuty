using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class SimpleMelee : MonoBehaviour
    {
        public GameObject weapon;
        public BoxCollider hitBox;
        public string targetTag = "Untagged";
        public LayerMask targetLayer;
        [SerializeField] private Animator avatarAnim;
        [SerializeField] private float attackSpeed = 1;

        public float dmg;
        private Collider[] cols;

        public ParticleSystem vfx;
        public AudioClip sfx;
        private void Awake()
        {
            Subscribe();
        }

        private void Subscribe()
        {

        }

        private void Detect()
        {
            cols = Physics.OverlapBox(hitBox.transform.position, hitBox.size / 2f, hitBox.transform.rotation);
            foreach (Collider col in cols)
            {
                if (col.CompareTag(targetTag) == false) continue;
                Dealt(col);
            }
        }

        private void Dealt(Collider col)
        {
            if(col.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(dmg);
            }
        }

        private void Play(Vector3 pos)
        {
            vfx.Play();
            AudioSource.PlayClipAtPoint(sfx, pos);
        }
    }
}
