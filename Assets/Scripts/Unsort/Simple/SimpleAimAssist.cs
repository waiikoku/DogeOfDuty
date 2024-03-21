using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class SimpleAimAssist : MonoBehaviour
    {
        [SerializeField] private Transform center;
        [SerializeField] private float range;
        [SerializeField] private Transform target;
        private RaycastHit[] cacheHits;
        private const int size = 16;
        private int detect;
        private void Awake()
        {
            cacheHits = new RaycastHit[size];
        }

        private void FixedUpdate()
        {
            detect = Physics.RaycastNonAlloc(center.position, center.forward, cacheHits, range);
            if (detect == 0) return;
            target.position = cacheHits[0].point;
        }
    }
}
