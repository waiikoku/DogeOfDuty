using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class ExpMolotovObj2 : MonoBehaviour
    {
        private Transform m_transform;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private GameObject fire;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private CapsuleCollider capsuleCollider;
        [SerializeField] private SphereCollider sphereCollider;
        public float dmg = 1;
        public float fireRadius = 3;
        float radius;
        float height;
        Vector3 origin;
        Vector3 direction;
        const float maxDistance = 1f;
        bool hit;
        RaycastHit hitInfo;

        private bool hasHit = false;
        private void Awake()
        {
            m_transform = transform;
            //radius = capsuleCollider.radius;
            radius = sphereCollider.radius;
            height = capsuleCollider.height;
        }

        private void FixedUpdate()
        {
            m_transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
            if (hasHit) return;
            /*
            origin = transform.position + capsuleCollider.center;
            direction = transform.forward;
            hit = Physics.CapsuleCast(origin, origin + height * transform.up, radius, direction, out hitInfo, maxDistance,targetLayer);
            */
            origin = sphereCollider.transform.position;
            direction = sphereCollider.transform.forward;
            hit = Physics.SphereCast(origin, radius, direction, out hitInfo, maxDistance, groundLayer);
            if (hit)
            {
                hasHit = true;
                Explode(hitInfo.point);
                Collider[] cols = Physics.OverlapSphere(m_transform.position, fireRadius, targetLayer);
                foreach (var col in cols)
                {
                    if (col.TryGetComponent(out IDamagable damagable))
                    {
                        damagable.TakeDamage(dmg);
                    }
                }
            }
        }

        private void Explode(Vector3 pos)
        {
            GameObject flame = Instantiate(fire, pos, Quaternion.identity);
            Destroy(gameObject, 0.2f);
            Destroy(flame, 5);
        }

        public void SetDamage(float damage)
        {
            dmg = damage;
        }
    }
}
