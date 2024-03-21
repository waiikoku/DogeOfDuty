using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class DebugSphere : MonoBehaviour
    {
        [SerializeField] private SphereCollider col;
        [SerializeField] private Color color;
        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(transform.position, col.radius);
        }
    }
}
