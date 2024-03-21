using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dod
{
    public class SimplePlayer : MonoBehaviour
    {
        private Transform m_transform;
        [SerializeField] private Transform cam;
        private Vector3 intend;
        private Vector3 camF;
        private Vector3 camR;

        private const string HORIZONTAL = "Horizontal";
        private const string VERTICAL = "Vertical";
        private Vector2 input;
        private Vector2 currentInput;
        private Vector2 inputVelocity;

        [SerializeField] private float speed = 1f;
        [SerializeField] private float smoothTime = 0.3f;

        private Vector3 lastPos;
        private Vector3 currentPos;
        public Vector3 Delta;
        public float Magnitude;
        public float magPerMinute;

        private void Awake()
        {
            m_transform = transform;
        }

        private void Update()
        {
            input.x = Axis(HORIZONTAL);
            input.y = Axis(VERTICAL);
            input.Normalize();

            currentInput = Vector2.SmoothDamp(currentInput, input, ref inputVelocity, smoothTime);
            magPerMinute = Magnitude * Application.targetFrameRate;
        }

        private void FixedUpdate()
        {
            camF = cam.forward;
            camR = cam.right;
            camF.y = 0;
            camR.y = 0;
            camF.Normalize();
            camR.Normalize();
            intend = (camF * currentInput.y) + (camR * currentInput.x);
            m_transform.position += speed * Time.fixedDeltaTime * intend;
            currentPos = m_transform.position;
            Delta = lastPos - currentPos;
            Magnitude = Delta.magnitude;
            lastPos = currentPos;
        }
        private float Axis(string key)
        {
            return Input.GetAxis(key);
        }
    }
}
