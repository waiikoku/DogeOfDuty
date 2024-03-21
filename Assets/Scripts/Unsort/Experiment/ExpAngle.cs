using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class ExpAngle : MonoBehaviour
    {
        public float length = 1;
        public float angle = 0;
        public Vector2 angle2D;
        public Vector3 angle3D;

        public float atan2;
        public float degA2;
        public Vector3 point;
        public Vector3 targetPoint;
        public Vector3 dir;
        public Quaternion rot;
        public Quaternion rotEuler;


        [SerializeField] private Transform target;

        [Header("Ex2")]
        public Vector3 direction;
        public float v3Angle;
        public float convertAngle;
        private enum CircleMode
        {
            Semi,
            Full
        }
        [SerializeField] private CircleMode mode;
        public Vector3 crossProduct;
        private void OnDrawGizmos()
        {
            atan2 = Mathf.Atan2(angle2D.x, angle2D.y);
            degA2 = atan2 * Mathf.Rad2Deg;
            point = Quaternion.AngleAxis(angle, Vector3.right) * Vector3.forward;
            rot = Quaternion.LookRotation(point, Vector3.up);
            rotEuler = Quaternion.Euler(angle, 0, 0);
            target.rotation = rotEuler;
            targetPoint = transform.position + (rotEuler * Vector3.up) * length;
            dir = targetPoint - transform.position;
            Gizmos.DrawLine(transform.position, targetPoint);

            direction = target.position - transform.position;
            v3Angle = Vector3.Angle(transform.forward, direction);
            convertAngle = Vector3.Angle(Vector3.zero, direction.normalized);
            crossProduct = Vector3.Cross(transform.forward, direction);
            switch (mode)
            {
                case CircleMode.Semi:
                    if (crossProduct.y < 0)
                    {
                        v3Angle = -v3Angle; // Adjust the angle if needed
                    }
                    break;
                case CircleMode.Full:
                    if (crossProduct.y < 0)
                    {
                        v3Angle = 360f - v3Angle; // Adjust the angle if needed
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
