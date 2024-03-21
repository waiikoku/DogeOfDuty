using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class SimpleOrbiter : MonoBehaviour
    {
        private Transform m_transform;
        [SerializeField] private GameObject orbiter;
        [SerializeField] private int initSize = 1;
        private List<GameObject> orbiters;

        [SerializeField] private float radius = 1;
        private float lastRadius;
        [SerializeField] private bool fullCircle = false;
        [SerializeField] private float stepAngle = 90;
        private void Awake()
        {
            m_transform = transform;
            orbiters = new List<GameObject>(initSize);
            if (fullCircle)
            {
                float step = 360f / (float)initSize;
                float angle = 0;
                for (int i = 0; i < initSize; i++)
                {
                    GameObject go = Instantiate(orbiter, m_transform);
                    go.transform.localPosition = PositionCircle2(radius, angle * Mathf.Deg2Rad);
                    angle += step;
                    orbiters.Add(go);
                }
            }
            else
            {
                for (int i = 0; i < initSize; i++)
                {
                    GameObject go = Instantiate(orbiter, m_transform);
                    go.transform.localPosition = PositionCircle(radius, i);
                    orbiters.Add(go);
                }
            }
            lastRadius = radius;
  
        }

        private void OnValidate()
        {
            if (orbiters == null) return;
            if(radius != lastRadius)
            {
                UpdateCircle();
                lastRadius = radius;
            }
        }

        private void UpdateCircle()
        {
            float step = 360f / (float)orbiters.Count;
            float angle = 0;
            for (int i = 0; i < orbiters.Count; i++)
            {
                orbiters[i].transform.localPosition = PositionCircle2(radius, angle * Mathf.Deg2Rad);
                angle += step;
            }
        }

        private Vector3 PositionCircle(float radius, float step)
        {
            float angleSection = Mathf.PI * 2f / radius;
            float trueAngle = step * angleSection;
            print($"AS:{angleSection} | TA:{trueAngle}");
            return new Vector3(Mathf.Cos(trueAngle) * radius, 0, Mathf.Sin(trueAngle) * radius);
        }

        private Vector3 PositionCircle2(float radius, float angle)
        {
            return new Vector3(Mathf.Sin(angle) * radius, 0, Mathf.Cos(angle) * radius);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (other.TryGetComponent(out IDamagable damagable))
                {
                    damagable.TakeDamage(1);
                }
            }
        }
    }
}
