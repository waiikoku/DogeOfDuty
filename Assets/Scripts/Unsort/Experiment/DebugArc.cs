using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Dod
{
    public class DebugArc : MonoBehaviour
    {
        [SerializeField] private float angle = 0;
        [SerializeField] private float radius = 1;
        [SerializeField] private Color color;
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = color;
            Handles.DrawWireArc(transform.position, transform.up, -transform.right, angle, radius);
        }
#endif
    }
}
