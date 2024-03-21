using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    [RequireComponent(typeof(Rigidbody))]
    public class RocketBullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private GameObject explode;
        public string targetTag = "Untagged";
        public float dmg = 1;
        private bool isHit = false;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetTag) == false) return;
            if (isHit)
            {

            }
            else
            {
                isHit = true;
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
                Instantiate(explode, transform.position, Quaternion.identity);
                Destroy(gameObject, 0.2f);
            }
            if (other.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(dmg);
            }
        }
    }
}
