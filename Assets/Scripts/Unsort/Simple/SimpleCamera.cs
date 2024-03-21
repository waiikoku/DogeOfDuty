using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class SimpleCamera : MonoBehaviour
    {
        private Transform m_transform;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotateSpeed;
        [SerializeField] private AnimationCurve curve;

        private const string HORIZONTAL = "Horizontal";
        private const string VERTICAL = "Vertical";
        private const string YAW = "Yaw";

        private Vector2 input;
        private Vector2 inputRot;
        private Vector3 intend;
        private Vector3 camF;
        private Vector3 camR;
        private float rot;

        private void Awake()
        {
            m_transform = transform;
        }

        private void Update()
        {
            input.x = Axis(HORIZONTAL);
            input.y = Axis(VERTICAL);
            input.Normalize();
            inputRot.x = Axis(YAW);
        }

        private float Axis(string key)
        {
            return Input.GetAxis(key);
        }

        private void FixedUpdate()
        {
            camF = m_transform.forward;
            camR = m_transform.right;
            camF.y = 0;
            camR.y = 0;
            camF.Normalize();
            camR.Normalize();
            intend = (camF * input.y) + (camR * input.x);   
            m_transform.position += moveSpeed * Time.fixedDeltaTime * intend;
            if (inputRot.x != 0f)
            {
                m_transform.Rotate(Vector3.up, inputRot.x * rotateSpeed,Space.World);
            }         
        }
    }
}
