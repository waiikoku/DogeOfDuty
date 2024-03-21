using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class ExpMolotovObj : MonoBehaviour
    {
        [SerializeField] private GameObject firePrefab;
        public string targetTag = "Untagged";

        [SerializeField] private Rigidbody rb;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private CapsuleCollider zone;
        private bool isHit;

        [SerializeField] private bool constantVelocity = false;
        public Vector3 desireVelocity;

        private Transform m_transform;
        private void Awake()
        {
            m_transform = transform;
        }

        private void FixedUpdate()
        {
            if (isHit) return;
            transform.rotation = Quaternion.LookRotation(rb.velocity,Vector3.up);
            //transform.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);
        }
        /*
        private bool isGround;

        private void FixedUpdate()
        {
            Collider[] cols = Physics.OverlapSphere(m_transform.position,zone.radius,groundLayer);
            isGround = cols.Length > 0;
            if (isGround && isHit == false)
            {
                isHit = true;
                Explode();
            }
        }
        */

        /*
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetTag) == false) return;
            Explode();
        }
        */

        private void OnCollisionEnter(Collision collision)
        {
            if (isHit) return;
            isHit = true;
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
            Explode();
        }

        private void Explode()
        {
            Vector3 pos = transform.position;
            pos.y = 0;
            Instantiate(firePrefab, pos, Quaternion.identity);
            Destroy(gameObject, 0.2f);
        }
    }
}
