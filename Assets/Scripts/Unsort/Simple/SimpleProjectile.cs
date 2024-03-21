using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    [RequireComponent(typeof(Rigidbody))]
    public class SimpleProjectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private bool stopOnCollision;
        private void OnCollisionEnter(Collision collision)
        {
            if (stopOnCollision == false) return;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                rb.isKinematic = true;
                if (other.TryGetComponent(out IDamagable damagable))
                {
                    damagable.TakeDamage(1);
                }
                Destroy(gameObject);
            }
        }
    }
}
