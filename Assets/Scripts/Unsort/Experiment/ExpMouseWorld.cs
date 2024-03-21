using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class ExpMouseWorld : MonoBehaviour
    {
        [SerializeField] Camera cam;
        public Vector3 mousePos;
        public Vector3 mp3;
        public Vector3 worldPos;
        public Vector3 worldPos2;
        public float z;
        public Vector3 offset;
        public float size = 1;
        public Color[] colour;
        private Vector3 camPos;
        private void Start()
        {
            camPos = cam.transform.position;
            z = camPos.x + camPos.z;
        }

        private void Update()
        {
            mousePos = Input.mousePosition;
        }
        private void LateUpdate()
        {
            offset = cam.transform.TransformPoint(new Vector3(0, 0, -z));

            mousePos.z = -z;
            worldPos = cam.ScreenToWorldPoint(mousePos);
            mp3.x = worldPos.x;
            mp3.z = worldPos.y;
            worldPos2 = offset + mp3;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = colour[0];
            Gizmos.DrawSphere(worldPos, size);
            Gizmos.color = colour[1];
            Gizmos.DrawSphere(worldPos2, size);
            Gizmos.color = colour[2];
            Gizmos.DrawSphere(offset, size);
        }
    }
}
