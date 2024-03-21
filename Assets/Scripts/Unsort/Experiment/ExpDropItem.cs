using Roguelike;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class ExpDropItem : MonoBehaviour
    {
        private Transform m_transform;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Collider col;
        [SerializeField] private bool isActive;
        [SerializeField] private string targetTag = "Untagged";

        private bool above;
        private bool isClear;
        private int exp;
        private void Awake()
        {
            m_transform = transform;
        }

        private void Start()
        {
            Activate(isActive);
        }
        private const float height = 0.1f;
        private void FixedUpdate()
        {
            above = m_transform.position.y > height;
            if (above == false && isClear == false)
            {
                isClear = true;
                if (rb != null)
                {
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;
                    rb.isKinematic = true;
                }
                Vector3 pos = m_transform.position;
                pos.y = 0;
                m_transform.position = pos;
            }
        }

        public void Activate(bool active)
        {
            if (active)
            {
                if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
                col.enabled = true;
            }
            else
            {
                col.enabled = false;
                if (rb != null) Destroy(rb);
            }
            isActive = active;
        }

        public void Knockback(Vector3 direction, float speed, float duration, Action callback = null)
        {
            StartCoroutine(Simulate(direction, speed, duration, callback));
        }

        private IEnumerator Simulate(Vector3 direction, float speed, float duration, Action callback)
        {
            float time = 0;
            while (duration > time)
            {
                time += Time.deltaTime;
                m_transform.position += speed * Time.fixedDeltaTime * direction;
                yield return new WaitForFixedUpdate();
            }
            callback?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isActive == false) return;
            if (other.CompareTag(targetTag))
            {
                AddExp();
                Destroy(gameObject);
            }
        }

        private void AddExp()
        {
            if (SurvivalManager.Instance == null) return;
            SurvivalManager.Instance.AddXp(exp);
        }

        public void SetExp(int xp)
        {
            exp = xp;
        }
    }
}
